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
            TokenKind.ExponentiationToken => "^",
            TokenKind.OpenParenthesisToken => "(",
            TokenKind.CloseParenthesisToken => ")",
            TokenKind.OpenBraceToken => "{",
            TokenKind.CloseBraceToken => "}",
            TokenKind.SemicolonToken => ";",
            TokenKind.CommaToken => ",",
            TokenKind.AssignmentToken => "=",
            TokenKind.GreaterThanToken => ">",
            TokenKind.LessThanToken => "<",
            TokenKind.GreaterThanOrEqualToken => ">=",
            TokenKind.LessThanOrEqualToken => "<=",
            TokenKind.EqualsToken => "==",
            TokenKind.LogicalNotToken => "!",
            TokenKind.LogicalAndToken => "&&",
            TokenKind.LogicalOrToken => "||",
            TokenKind.TrueKeyword => "true",
            TokenKind.FalseKeyword => "false",
            TokenKind.IntegerKeyword => "integer",
            TokenKind.DoubleKeyword => "double",
            TokenKind.BooleanKeyword => "boolean",
            TokenKind.LetKeyword => "let",
            TokenKind.IfKeyword => "if",
            TokenKind.ElseKeyword => "else",
            TokenKind.ForKeyword => "for",
            TokenKind.ToKeyword => "to",
            TokenKind.StepKeyword => "step",
            TokenKind.WhileKeyword => "while",
            TokenKind.BreakKeyword => "break",
            TokenKind.FunctionKeyword => "function",
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
            "integer" => TokenKind.IntegerKeyword,
            "double" => TokenKind.DoubleKeyword,
            "boolean" => TokenKind.BooleanKeyword,
            "let" => TokenKind.LetKeyword,
            "true" => TokenKind.TrueKeyword,
            "false" => TokenKind.FalseKeyword,
            "if" => TokenKind.IfKeyword,
            "else" => TokenKind.ElseKeyword,
            "for" => TokenKind.ForKeyword,
            "to" => TokenKind.ToKeyword,
            "step" => TokenKind.StepKeyword,
            "while" => TokenKind.WhileKeyword,
            "break" => TokenKind.BreakKeyword,
            "function" => TokenKind.FunctionKeyword,
            _ => TokenKind.IdentifierToken
        };
    }

    public static TokenKind? GetDataType(this TokenKind kind)
    {
        return kind switch
        {
            TokenKind.IntegerToken => TokenKind.IntegerKeyword,
            TokenKind.DoubleToken => TokenKind.DoubleKeyword,
            TokenKind.TrueKeyword => TokenKind.BooleanKeyword,
            TokenKind.FalseKeyword => TokenKind.BooleanKeyword,
            _ => null
        };
    }
}