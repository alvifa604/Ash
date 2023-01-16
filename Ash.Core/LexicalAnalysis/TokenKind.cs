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
    GreaterThanOrEqualToken,
    GreaterThanToken,
    LessThanToken,
    LessThanOrEqualToken,
    OpenBraceToken,
    CloseBraceToken,
    SemicolonToken,
    EndOfFileToken,
    BadToken,

    // Expressions
    LiteralExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
    VariableExpression,
    AssignmentExpression,

    // Statements
    ProgramStatement,
    DeclarationStatement,
    ExpressionStatement,
    IfStatement,
    ElseStatement,
    ForStatement,
    WhileStatement,
    BlockStatement,
    BreakStatement,

    // Keywords
    LetKeyword,
    TrueKeyword,
    FalseKeyword,
    IfKeyword,
    ElseKeyword,
    ForKeyword,
    WhileKeyword,
    ToKeyword,
    StepKeyword,
    BreakKeyword
}