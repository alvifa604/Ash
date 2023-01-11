namespace Hada.Core.Errors;

public enum ErrorType
{
    // Lexer errors
    IllegalCharacter = 101,
    InvalidNumber = 102,

    // Parser errors
    InvalidSyntax = 201,

    // Interpreter errors
    InvalidBinaryOperator = 301,
    InvalidUnaryOperator = 302,

    // Runtime errors
    DivisionByZero = 401
}