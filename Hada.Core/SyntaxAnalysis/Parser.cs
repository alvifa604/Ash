using Hada.Core.Errors;
using Hada.Core.LexicalAnalysis;
using Hada.Core.SyntaxAnalysis.Expressions;

namespace Hada.Core.SyntaxAnalysis;

internal sealed class Parser
{
    public ErrorsBag ErrorsBag { get; } = new();
    private readonly Token[] _tokens;
    private int _position;

    private Token Current => Peek();

    public Parser(Token[] tokens)
    {
        _tokens = tokens;
    }

    public SyntaxTree Parse()
    {
        var root = ParseExpression();
        var endOfFileToken = MatchToken(TokenKind.EndOfFileToken);
        return new SyntaxTree(root, ErrorsBag, endOfFileToken);
    }

    private Expression ParseExpression()
    {
        return ParseBinaryExpression(ParseTerm, TokenKind.PlusToken, TokenKind.MinusToken);
    }

    private Expression ParseTerm()
    {
        return ParseBinaryExpression(ParseFactor, TokenKind.MultiplicationToken, TokenKind.DivisionToken);
    }

    private Expression ParseBinaryExpression(Func<Expression> func, params TokenKind[] kinds)
    {
        var left = func();
        while (kinds.Contains(Current.Kind))
        {
            var operatorToken = NextToken();
            var right = func();
            left = new BinaryExpression(left, operatorToken, right);
        }

        return left;
    }

    private Expression? ParseFactor()
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
        var expression = ParseExpression();
        return new UnaryExpression(operatorToken, expression);
    }

    private Expression ParseParenthesizedExpression()
    {
        var openParenthesisToken = MatchToken(TokenKind.OpenParenthesisToken);
        var expression = ParseExpression();
        var closeParenthesisToken = MatchToken(TokenKind.CloseParenthesisToken);
        return new ParenthesizedExpression(openParenthesisToken, expression, closeParenthesisToken);
    }

    private Expression ParseLiteralExpression()
    {
        var literalToken = MatchToken(TokenKind.IdentifierToken);
        return new LiteralExpression(literalToken);
    }

    private Expression ParseNumberExpression()
    {
        var numberToken = NextToken();
        return new NumberExpression(numberToken);
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
        return new Token(kind, Current.Text, "");
    }
}