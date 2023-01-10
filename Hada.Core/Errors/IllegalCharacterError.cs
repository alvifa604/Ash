using Hada.Core.LexicalAnalysis;

namespace Hada.Core.Errors;

public sealed class IllegalCharacterError : Error
{
    private readonly char _character;
    public override ErrorType ErrorType => ErrorType.IllegalCharacter;

    public IllegalCharacterError(char character, Position posStart, Position posEnd) : base(posStart, posEnd)
    {
        _character = character;
    }

    public override string ToString()
    {
        return
            $"ERROR {(int)ErrorType}: Illegal character '{_character}' in file {PosStart.FileName}. ({PosStart.Line}, {PosStart.Column})";
    }
}