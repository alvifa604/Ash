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
            TokenKind.IfStatement => "if",
            TokenKind.ElseStatement => "else",
            TokenKind.EndOfFileToken => "end of file",
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
            TokenKind.EqualsToken or TokenKind.NotEqualsToken or TokenKind.GreaterThanToken
                or TokenKind.GreaterThanOrEqualToken or TokenKind.LessThanToken or TokenKind.LessThanOrEqualToken => 3,
            TokenKind.LogicalAndToken => 2,
            TokenKind.LogicalOrToken => 1,
            _ => 0
        };
    }

    public static int GetUnaryOperatorPriority(this TokenKind kind)
    {
        return kind switch
        {
            TokenKind.LogicalNotToken or TokenKind.PlusToken or TokenKind.MinusToken => 7,
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
            "if" => TokenKind.IfKeyword,
            "else" => TokenKind.ElseKeyword,
            _ => TokenKind.IdentifierToken
        };
    }
}