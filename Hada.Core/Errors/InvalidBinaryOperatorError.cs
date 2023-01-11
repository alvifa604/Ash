using Hada.Core.LexicalAnalysis;

namespace Hada.Core.Errors;

public class InvalidBinaryOperatorError : Error
{
    public override ErrorType ErrorType => ErrorType.InvalidBinaryOperator;

    public InvalidBinaryOperatorError(string message, Position start, Position end) : base(message, start, end)
    {
    }
}