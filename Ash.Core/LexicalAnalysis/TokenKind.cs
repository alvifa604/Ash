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
    CommaToken,
    EndOfFileToken,
    BadToken,

    // Expressions
    LiteralExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
    VariableExpression,
    AssignmentExpression,
    FunctionCallExpression,
    ArgumentExpression,

    // Statements
    DeclarationStatement,
    ExpressionStatement,
    IfStatement,
    ElseStatement,
    ForStatement,
    WhileStatement,
    BlockStatement,
    BreakStatement,
    FunctionDeclarationStatement,

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
    BreakKeyword,
    FunctionKeyword,

    //Types
    IntegerKeyword,
    DoubleKeyword,
    BooleanKeyword,

    // Others
    Parameter,
    BooleanToken
}