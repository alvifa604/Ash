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
}