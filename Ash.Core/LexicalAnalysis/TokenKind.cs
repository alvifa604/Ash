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
    OpenBraceToken,
    CloseBraceToken,
    BadToken,

    // Expressions
    LiteralExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
    VariableExpression,
    AssignmentExpression,

    // Statements
    DeclarationStatement,
    ExpressionStatement,
    IfStatement,
    ElseStatement,

    // Keywords
    LetKeyword,
    TrueKeyword,
    FalseKeyword,
    IfKeyword,
    ElseKeyword
}