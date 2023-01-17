using Ash.Core.Errors;
using Ash.Core.Interpretation;
using Ash.Core.LexicalAnalysis;
using Ash.Core.SyntaxAnalysis.Expressions;
using Ash.Core.SyntaxAnalysis.Statements;

namespace Ash.Core.SyntaxAnalysis;

internal sealed class Parser
{
    private ErrorsBag ErrorsBag { get; } = new();
    private readonly Token[] _tokens;
    private int _position;

    private Token Current => Peek();

    public Parser(Token[] tokens)
    {
        _tokens = tokens;
    }

    public SyntaxTree Parse()
    {
        var root = ParseAll();
        var endOfFileToken = MatchToken(TokenKind.EndOfFileToken);
        return new SyntaxTree(root, ErrorsBag, endOfFileToken);
    }

    private Statement? ParseAll()
    {
        return Current.Kind == TokenKind.FunctionKeyword
            ? ParseFunctionDeclarationStatement()
            : ParseStatement();
    }

    private Statement ParseStatement()
    {
        return Current.Kind switch
        {
            TokenKind.LetKeyword or TokenKind.IntegerKeyword or TokenKind.BooleanKeyword or TokenKind.DoubleKeyword =>
                ParseDeclarationStatement(),
            TokenKind.IfKeyword => ParseIfStatement(),
            TokenKind.ForKeyword => ParseForStatement(),
            TokenKind.WhileKeyword => ParseWhileStatement(),
            TokenKind.OpenBraceToken => ParseBlockStatement(),
            TokenKind.BreakKeyword => ParseBreakStatement(),
            _ => ParseExpressionStatement()
        };
    }

    private DeclarationStatement ParseDeclarationStatement()
    {
        return Current.Kind switch
        {
            TokenKind.IntegerKeyword => ParseDeclaration(TokenKind.IntegerKeyword, SymbolType.Integer),
            TokenKind.DoubleKeyword => ParseDeclaration(TokenKind.DoubleKeyword, SymbolType.Double),
            TokenKind.BooleanKeyword => ParseDeclaration(TokenKind.BooleanKeyword, SymbolType.Boolean),
            _ => ParseDeclaration(TokenKind.LetKeyword, SymbolType.Any)
        };
    }

    private DeclarationStatement ParseDeclaration(TokenKind keyword, SymbolType symbolType)
    {
        var keywordToken = MatchToken(keyword);
        var identifierToken = MatchToken(TokenKind.IdentifierToken);
        if (Current.Kind is TokenKind.SemicolonToken)
        {
            var semicolonToken = MatchToken(TokenKind.SemicolonToken);
            return new DeclarationStatement(keywordToken, identifierToken, null, null, keywordToken.Start,
                semicolonToken.End, symbolType);
        }

        var equalsToken = MatchToken(TokenKind.AssignmentToken);
        var expression = ParseExpression();
        MatchToken(TokenKind.SemicolonToken);
        return new DeclarationStatement(keywordToken, identifierToken, equalsToken, expression, keywordToken.Start,
            expression.End, symbolType);
    }

    private IfStatement ParseIfStatement()
    {
        var ifToken = MatchToken(TokenKind.IfKeyword);
        var condition = ParseExpression();
        var body = ParseStatement();

        if (Current.Kind is not TokenKind.ElseKeyword)
            return new IfStatement(ifToken, condition, body);

        var elseStatement = ParseElseStatement();
        return new IfStatement(ifToken, condition, body, elseStatement);
    }

    private ElseStatement ParseElseStatement()
    {
        var elseToken = MatchToken(TokenKind.ElseKeyword);
        var body = ParseStatement();
        return new ElseStatement(elseToken, body);
    }

    private ForStatement ParseForStatement()
    {
        var forKeyword = MatchToken(TokenKind.ForKeyword);
        MatchToken(TokenKind.OpenParenthesisToken);
        var lowerBoundDeclaration = ParseDeclarationStatement();
        var toKeyword = MatchToken(TokenKind.ToKeyword);
        var upperBound = ParseExpression();
        Statement body;

        if (Peek(1).Kind is TokenKind.StepKeyword)
        {
            MatchToken(TokenKind.SemicolonToken);
            var stepKeyword = MatchToken(TokenKind.StepKeyword);
            var step = ParseExpression();
            MatchToken(TokenKind.CloseParenthesisToken);
            body = ParseStatement();

            return new ForStatement(forKeyword, lowerBoundDeclaration, toKeyword, upperBound, body, stepKeyword, step);
        }

        MatchToken(TokenKind.CloseParenthesisToken);
        body = ParseStatement();
        return new ForStatement(forKeyword, lowerBoundDeclaration, toKeyword, upperBound, body);
    }

    private WhileStatement ParseWhileStatement()
    {
        var whileToken = MatchToken(TokenKind.WhileKeyword);
        MatchToken(TokenKind.OpenParenthesisToken);
        var condition = ParseExpression();
        MatchToken(TokenKind.CloseParenthesisToken);
        var body = ParseStatement();
        return new WhileStatement(whileToken, condition, body);
    }

    private BlockStatement ParseBlockStatement()
    {
        var openBraceToken = MatchToken(TokenKind.OpenBraceToken);
        var body = ParseStatement();
        var closeBraceToken = MatchToken(TokenKind.CloseBraceToken);
        return new BlockStatement(body, openBraceToken, closeBraceToken);
    }

    private BreakStatement ParseBreakStatement()
    {
        var breakToken = MatchToken(TokenKind.BreakKeyword);
        MatchToken(TokenKind.SemicolonToken);
        return new BreakStatement(breakToken);
    }

    private FunctionDeclarationStatement? ParseFunctionDeclarationStatement()
    {
        var keyword = MatchToken(TokenKind.FunctionKeyword);
        var identifier = MatchToken(TokenKind.IdentifierToken);
        MatchToken(TokenKind.OpenParenthesisToken);
        var parameters = ParseParameterList();
        if (parameters is null)
            return null;
        MatchToken(TokenKind.CloseParenthesisToken);
        var body = ParseBlockStatement();

        return new FunctionDeclarationStatement(keyword, identifier, parameters, body);
    }

    private List<ParameterNode>? ParseParameterList()
    {
        var parameters = new List<ParameterNode>();
        var parseMore = true;

        while (parseMore && Current.Kind is not (TokenKind.CloseParenthesisToken or TokenKind.EndOfFileToken))
        {
            var parameter = ParseParameter();
            if (parameter is null)
                return null;
            if (Current.Kind is TokenKind.CloseParenthesisToken)
            {
                parameters.Add(parameter);
                parseMore = false;
            }
            else
            {
                MatchToken(TokenKind.CommaToken);
                parameters.Add(parameter);
            }
        }

        return parameters;
    }

    private ParameterNode? ParseParameter()
    {
        var tokenType = ParseType();
        if (tokenType is null)
        {
            ErrorsBag.ReportInvalidSyntax("Parameters must specify the type", Current.Start, Current.End);
            return null;
        }

        var type = tokenType.Kind.GetSymbolType();
        if (type is null or SymbolType.Any)
        {
            ErrorsBag.ReportInvalidSyntax("Parameters must specify the type", Current.Start, Current.End);
            return null;
        }

        var identifier = MatchToken(TokenKind.IdentifierToken);
        return new ParameterNode(identifier, type.Value);
    }

    private Token? ParseType()
    {
        return Current.Kind is TokenKind.IntegerKeyword or TokenKind.DoubleKeyword or TokenKind.BooleanKeyword
            ? NextToken()
            : null;
    }

    private Statement ParseExpressionStatement()
    {
        var expression = ParseExpression();
        MatchToken(TokenKind.SemicolonToken);
        return new ExpressionStatement(expression);
    }

    private Expression ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    private Expression ParseAssignmentExpression()
    {
        if (Current.Kind is TokenKind.IdentifierToken)
            switch (Peek(1).Kind)
            {
                case TokenKind.AssignmentToken:
                {
                    var identifier = MatchToken(TokenKind.IdentifierToken);
                    var equalsToken = MatchToken(TokenKind.AssignmentToken);
                    var expression = ParseExpression();
                    return new AssignmentExpression(identifier, equalsToken, expression);
                }
                case TokenKind.OpenParenthesisToken:
                {
                    var identifier = MatchToken(TokenKind.IdentifierToken);
                    MatchToken(TokenKind.OpenParenthesisToken);
                    var arguments = ParseArgumentList();
                    MatchToken(TokenKind.CloseParenthesisToken);
                    return new FunctionCallExpression(identifier, arguments);
                }
            }

        return ParseBinaryExpression();
    }

    private List<ArgumentExpression> ParseArgumentList()
    {
        var arguments = new List<ArgumentExpression>();
        var parseMore = true;

        while (parseMore && Current.Kind is not (TokenKind.CloseBraceToken or TokenKind.EndOfFileToken))
        {
            //var argumentToken = NextToken();
            var aExpression = ParseExpression();
            if (Current.Kind is TokenKind.SemicolonToken)
                break;

            var argumentExpression = new ArgumentExpression(aExpression);
            if (Current.Kind is TokenKind.CloseParenthesisToken)
            {
                arguments.Add(argumentExpression);
                parseMore = false;
            }

            else
            {
                MatchToken(TokenKind.CommaToken);
                arguments.Add(argumentExpression);
            }
        }

        return arguments;
    }

    private Expression ParseBinaryExpression(int parentPriority = 0)
    {
        Expression left;
        var unaryPriority = Current.Kind.GetUnaryOperatorPriority();

        // If there's an operator before the expression
        if (unaryPriority != 0 && unaryPriority >= parentPriority)
        {
            var operatorToken = NextToken();
            var expression = ParseBinaryExpression(unaryPriority);
            left = new UnaryExpression(operatorToken, expression, expression.Start, expression.End);
        }
        else
        {
            left = ParsePrimaryExpression();
        }

        while (true)
        {
            // Gets the priority of the current operator
            var binaryPriority = Current.Kind.GetBinaryOperatorPriority();
            if (binaryPriority == 0 || binaryPriority <= parentPriority) break;

            var operatorToken = NextToken();
            var right = ParseBinaryExpression(binaryPriority);
            left = new BinaryExpression(left, operatorToken, right, left.Start, right.End);
        }

        return left;
    }

    private Expression ParsePrimaryExpression()
    {
        return Current.Kind switch
        {
            TokenKind.IntegerToken or TokenKind.DoubleToken => ParseLiteralExpression(),
            TokenKind.TrueKeyword or TokenKind.FalseKeyword => ParseBoolean(),
            TokenKind.OpenParenthesisToken => ParseParenthesizedExpression(),
            TokenKind.PlusToken or TokenKind.MinusToken => ParseUnaryExpression(),
            _ => ParseVariableExpression()
        };
    }

    private Expression ParseBoolean()
    {
        var booleanToken = NextToken();
        var value = booleanToken.Kind == TokenKind.TrueKeyword;
        return new LiteralExpression(booleanToken, booleanToken.Start, booleanToken.End, value);
    }

    private Expression ParseUnaryExpression()
    {
        var operatorToken = NextToken();
        var expression = ParsePrimaryExpression();
        return new UnaryExpression(operatorToken, expression, operatorToken.Start, expression.End);
    }

    private Expression ParseParenthesizedExpression()
    {
        var openToken = MatchToken(TokenKind.OpenParenthesisToken);
        var expression = ParseExpression();
        var closeToken = MatchToken(TokenKind.CloseParenthesisToken);
        return new ParenthesizedExpression(openToken, expression, closeToken, openToken.Start, openToken.End);
    }

    private Expression ParseVariableExpression()
    {
        var variableToken = MatchToken(TokenKind.IdentifierToken);
        return new VariableExpression(variableToken, variableToken.Start, variableToken.End);
    }

    private Expression ParseLiteralExpression()
    {
        var literalToken = NextToken();
        return new LiteralExpression(literalToken, literalToken.Start, literalToken.End);
    }

    private Token Peek(int offset = 0)
    {
        var index = _position + offset;
        return index < _tokens.Length
            ? _tokens[index]
            : _tokens.Last();
    }

    private Token NextToken()
    {
        var current = Current;
        _position++;
        return current;
    }

    private Token MatchToken(TokenKind kind)
    {
        if (Current.Kind == kind) return NextToken();

        var expected = kind == TokenKind.IdentifierToken
            ? "identifier"
            : kind.GetText();
        var got = Current.Kind.GetText() ?? Current.Text;

        ErrorsBag.ReportInvalidSyntax($"Expected '{expected}', but found '{got}'",
            Current.Start,
            Current.End);
        return new Token(kind, Current.Text, Current.Start, Current.End, "");
    }
}