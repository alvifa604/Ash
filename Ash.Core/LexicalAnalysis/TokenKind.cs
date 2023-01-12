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
    IdentifierToken,
    AssignmentToken,
    EqualsToken,
    EndOfFileToken,
    BadToken,

    // Expressions
    NumberExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
    AssignmentExpression,
    ReAssignmentExpression,

    // Keywords
    LetKeyword,
    TrueKeyword,
    FalseKeyword
}