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
    LiteralExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
    AssignmentExpression,
    ReAssignmentExpression,
    VariableExpression,

    // Keywords
    LetKeyword,
    TrueKeyword,
    FalseKeyword
}