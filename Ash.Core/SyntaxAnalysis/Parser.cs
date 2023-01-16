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
        var root = ParseStatement();
        var endOfFileToken = MatchToken(TokenKind.EndOfFileToken);
        return new SyntaxTree(root, ErrorsBag, endOfFileToken);
    }

    private Statement ParseStatement()
    {
        return Current.Kind switch
        {
            TokenKind.LetKeyword => ParseDeclarationStatement(),
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
        var letToken = MatchToken(TokenKind.LetKeyword);
        var identifier = MatchToken(TokenKind.IdentifierToken);
        var equalsToken = MatchToken(TokenKind.AssignmentToken);
        var expression = ParseExpression();
        MatchToken(TokenKind.SemicolonToken);
        return new DeclarationStatement(letToken, identifier, equalsToken, expression, letToken.Start,
            expression.End);
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

    private Statement ParseForStatement()
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

    private Statement ParseWhileStatement()
    {
        var whileToken = MatchToken(TokenKind.WhileKeyword);
        MatchToken(TokenKind.OpenParenthesisToken);
        var condition = ParseExpression();
        MatchToken(TokenKind.CloseParenthesisToken);
        var body = ParseStatement();
        return new WhileStatement(whileToken, condition, body);
    }

    private Statement ParseBlockStatement()
    {
        var openBraceToken = MatchToken(TokenKind.OpenBraceToken);
        var body = ParseStatement();
        var closeBraceToken = MatchToken(TokenKind.CloseBraceToken);
        return new BlockStatement(body, openBraceToken, closeBraceToken);
    }

    private Statement ParseBreakStatement()
    {
        var breakToken = MatchToken(TokenKind.BreakKeyword);
        MatchToken(TokenKind.SemicolonToken);
        return new BreakStatement(breakToken);
    }

    private Statement ParseExpressionStatement()
    {
        var expression = ParseExpression();
        return new ExpressionStatement(expression);
    }

    private Expression ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    private Expression ParseAssignmentExpression()
    {
        if (Current.Kind is TokenKind.IdentifierToken && Peek(1).Kind is TokenKind.AssignmentToken)
        {
            var identifier = MatchToken(TokenKind.IdentifierToken);
            var equalsToken = MatchToken(TokenKind.AssignmentToken);
            var expression = ParseExpression();
            MatchToken(TokenKind.SemicolonToken);
            return new AssignmentExpression(identifier, equalsToken, expression);
        }

        return ParseBinaryExpression();
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
            Current.Start!,
            Current.End);
        return new Token(kind, Current.Text, Current.Start, Current.End, "");
    }
}