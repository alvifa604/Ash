using Hada.Core.LexicalAnalysis;

namespace Hada.Core.Errors;

public sealed class InvalidNumberError : Error
{
    public override ErrorType ErrorType => ErrorType.InvalidNumber;

    public InvalidNumberError(string message, Position posStart, Position posEnd) : base(message, posStart, posEnd)
    {
    }
}