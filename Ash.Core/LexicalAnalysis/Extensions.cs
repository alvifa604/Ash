namespace Ash.Core.LexicalAnalysis;

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
            TokenKind.LetKeyword => "let",
            _ => null
        };
    }

    public static int GetBinaryOperatorPriority(this TokenKind kind)
    {
        return kind switch
        {
            TokenKind.ExponentiationToken => 5,
            TokenKind.MultiplicationToken or TokenKind.DivisionToken => 4,
            TokenKind.PlusToken or TokenKind.MinusToken => 3,
            TokenKind.EqualsToken or TokenKind.NotEqualsToken => 2,
            _ => 0
        };
    }

    public static int GetUnaryOperatorPriority(this TokenKind kind)
    {
        return kind switch
        {
            TokenKind.LogicalNotToken => 6,
            TokenKind.PlusToken or TokenKind.MinusToken => 6,
            _ => 0
        };
    }

    public static TokenKind GetKeywordOrIdentifierKind(this string text)
    {
        return text switch
        {
            "let" => TokenKind.LetKeyword,
            "true" => TokenKind.TrueKeyword,
            "false" => TokenKind.FalseKeyword,
            _ => TokenKind.IdentifierToken
        };
    }
}