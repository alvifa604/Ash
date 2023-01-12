using Ash.Core.LexicalAnalysis;

namespace Ash.Core.Errors;

public sealed class IllegalCharacterError : Error
{
    public override ErrorType ErrorType => ErrorType.IllegalCharacter;

    public IllegalCharacterError(string message, Position posStart, Position posEnd) : base(message, posStart, posEnd)
    {
    }
}