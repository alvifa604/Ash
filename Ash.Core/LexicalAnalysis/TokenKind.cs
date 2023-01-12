namespace Ash.Core.LexicalAnalysis;

public enum TokenKind
{
    // Tokens
    IntegerToken,
    DoubleToken,
    PlusToken,
    MinusToken,
    MultiplicationToken,
    ExponentiationToken,
    DivisionToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    WhiteSpaceToken,
    KeywordToken,
    IdentifierToken,
    EqualsToken,
    EndOfFileToken,
    BadToken,

    // Expressions
    NumberExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,

    // Keywords
    LetKeyword,
    IntegerKeyword,
    DoubleKeyword
}