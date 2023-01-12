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
            TokenKind.ExponentiationToken => 6,
            TokenKind.MultiplicationToken or TokenKind.DivisionToken => 5,
            TokenKind.PlusToken or TokenKind.MinusToken => 4,
            TokenKind.EqualsToken => 3,
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

    public static TokenKind GetKeywordOrIdentifierKind(this string text)
    {
        return text switch
        {
            "let" => TokenKind.LetKeyword,
            "integer" => TokenKind.IntegerKeyword,
            "double" => TokenKind.DoubleKeyword,
            "true" => TokenKind.TrueKeyword,
            "false" => TokenKind.FalseKeyword,
            _ => TokenKind.IdentifierToken
        };
    }
}