using Hada.Core.LexicalAnalysis;

namespace Hada.Core.Errors;

public class InvalidUnaryOperatorError : Error
{
    public InvalidUnaryOperatorError(string message, Position start, Position end) : base(message, start, end)
    {
    }

    public override ErrorType ErrorType => ErrorType.InvalidUnaryOperator;
}