namespace Hada.Core.Errors;

public enum ErrorType
{
    // Lexer errors
    IllegalCharacter = 101,
    InvalidNumber = 102,

    // Parser errors
    InvalidSyntax = 201
}