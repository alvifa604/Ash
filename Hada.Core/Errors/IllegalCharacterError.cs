using Hada.Core.LexicalAnalysis;

namespace Hada.Core.Errors;

public sealed class IllegalCharacterError : Error
{
    public override ErrorType ErrorType => ErrorType.IllegalCharacter;

    public IllegalCharacterError(string message, Position posStart, Position posEnd) : base(message, posStart, posEnd)
    {
    }
}