using Ash.Core.LexicalAnalysis;

namespace Ash.Core.Errors;

public class DivisionByZeroError : Error
{
    public override ErrorType ErrorType => ErrorType.DivisionByZero;

    public DivisionByZeroError(string message, Position start, Position end) : base(message, start, end)
    {
    }
}