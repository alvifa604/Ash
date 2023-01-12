using Ash.Core.LexicalAnalysis;

namespace Ash.Core.Errors;

public class InvalidSyntaxError : Error
{
    public override ErrorType ErrorType => ErrorType.InvalidSyntax;

    public InvalidSyntaxError(string message, Position posStart, Position posEnd) : base(message, posStart, posEnd)
    {
    }
}