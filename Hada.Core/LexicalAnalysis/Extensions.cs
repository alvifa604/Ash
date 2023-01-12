namespace Hada.Core.LexicalAnalysis;

public static class Extensions
{
    public static string? GetText(this TokenKind kind)
    {
        return kind switch
        {
            TokenKind.PlusToken => "+",
            TokenKind.MinusToken => "-",
            TokenKind.MultiplicationToken => "*",
            TokenKind.DivisionToken => "/",
            TokenKind.OpenParenthesisToken => "(",
            TokenKind.CloseParenthesisToken => ")",
            _ => null
        };
    }

    public static int GetBinaryOperatorPriority(this TokenKind kind)
    {
        return kind switch
        {
            TokenKind.ExponentiationToken => 6,
            TokenKind.MultiplicationToken or TokenKind.DivisionToken => 5,
            TokenKind.PlusToken or TokenKind.MinusToken => 4,
            _ => 0
        };
    }

    public static int GetUnaryOperatorPriority(this TokenKind kind)
    {
        return kind switch
        {
            TokenKind.PlusToken or TokenKind.MinusToken => 7,
            _ => 0
        };
    }
}