using Ash.Core.Errors;
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
            _ => ParseExpressionStatement()
        };
    }

    private DeclarationStatement ParseDeclarationStatement()
    {
        var letToken = MatchToken(TokenKind.LetKeyword);
        var identifier = MatchToken(TokenKind.IdentifierToken);
        var equalsToken = MatchToken(TokenKind.AssignmentToken);
        var expression = ParseExpression();
        return new DeclarationStatement(letToken, identifier, equalsToken, expression, letToken.Start,
            expression.End);
    }

    private IfStatement ParseIfStatement()
    {
        var ifToken = MatchToken(TokenKind.IfKeyword);
        var condition = ParseExpression();
        MatchToken(TokenKind.OpenBraceToken);
        var body = ParseStatement();
        var closeBraceToken = MatchToken(TokenKind.CloseBraceToken);

        if (Current.Kind is not TokenKind.ElseKeyword)
            return new IfStatement(ifToken, condition, body, closeBraceToken.End);

        var elseStatement = ParseElseStatement();
        return new IfStatement(ifToken, condition, body, ifToken.Start, elseStatement);
    }

    private ElseStatement ParseElseStatement()
    {
        var elseToken = MatchToken(TokenKind.ElseKeyword);
        MatchToken(TokenKind.OpenBraceToken);
        var body = ParseStatement();
        var closeBraceToken = MatchToken(TokenKind.CloseBraceToken);
        return new ElseStatement(elseToken, body, closeBraceToken.End);
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
            left = Current.Kind is TokenKind.LetKeyword
                ? ParseAssignmentExpression()
                : ParsePrimaryExpression();
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

        ErrorsBag.ReportInvalidSyntax($"Expected '{kind.GetText()}', but found '{Current.Kind.GetText()}'",
            Current.Start!,
            Current.End);
        return new Token(kind, Current.Text, Current.Start, Current.End, "");
    }
}

internal class ExpressionStatement : Statement
{
    public override TokenKind Kind => TokenKind.ExpressionStatement;
    public Expression Expression { get; }
    public override Position Start { get; }
    public override Position End { get; }

    public ExpressionStatement(Expression expression)
    {
        Expression = expression;
        Start = expression.Start;
        End = expression.End;
    }
}