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
    LogicalNotToken,
    LogicalAndToken,
    NotEqualsToken,
    LogicalOrToken,
    EndOfFileToken,
    GreaterThanOrEqualToken,
    GreaterThanToken,
    LessThanToken,
    LessThanOrEqualToken,
    BadToken,

    // Expressions
    LiteralExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
    VariableExpression,

    // Statements
    DeclarationStatement,
    AssignmentStatement,
    ExpressionStatement,
    IfStatement,

    // Keywords
    LetKeyword,
    TrueKeyword,
    FalseKeyword
}