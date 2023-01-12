namespace Hada.Core.LexicalAnalysis;

public enum TokenKind
{
    // Tokens
    IntegerToken,
    DoubleToken,
    IdentifierToken,
    PlusToken,
    MinusToken,
    MultiplicationToken,
    ExponentiationToken,
    DivisionToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    WhiteSpaceToken,
    EndOfFileToken,
    BadToken,

    // Expressions
    NumberExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
}