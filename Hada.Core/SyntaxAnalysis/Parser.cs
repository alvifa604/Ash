using Hada.Core.Errors;
using Hada.Core.LexicalAnalysis;
using Hada.Core.SyntaxAnalysis.Expressions;

namespace Hada.Core.SyntaxAnalysis;

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
        var root = ParseBinaryExpression();
        var endOfFileToken = MatchToken(TokenKind.EndOfFileToken);
        return new SyntaxTree(root, ErrorsBag, endOfFileToken);
    }

    private Expression ParseExpression()
    {
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
            TokenKind.IntegerToken or TokenKind.DoubleToken => ParseNumberExpression(),
            TokenKind.OpenParenthesisToken => ParseParenthesizedExpression(),
            TokenKind.PlusToken or TokenKind.MinusToken => ParseUnaryExpression(),
            _ => ParseLiteralExpression()
        };
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

    private Expression ParseLiteralExpression()
    {
        var literalToken = MatchToken(TokenKind.IdentifierToken);
        return new LiteralExpression(literalToken, literalToken.Start, literalToken.End);
    }

    private Expression ParseNumberExpression()
    {
        var numberToken = NextToken();
        return new NumberExpression(numberToken, numberToken.Start, numberToken.End);
    }

    private Token Peek()
    {
        return _position < _tokens.Length
            ? _tokens[_position]
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

        ErrorsBag.ReportInvalidSyntax($"Expected token of kind {kind}, but found {Current.Kind}", Current.Start!,
            Current.End!);
        return new Token(kind, Current.Text, Current.Start, Current.End, "");
    }
}