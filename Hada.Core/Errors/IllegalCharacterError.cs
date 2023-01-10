using Hada.Core.LexicalAnalysis;

namespace Hada.Core.Errors;

public sealed class IllegalCharacterError : Error
{
    public char Character { get; }
    public override ErrorType ErrorType => ErrorType.IllegalCharacter;

    public IllegalCharacterError(char character, Position posStart, Position posEnd) : base(posStart, posEnd)
    {
        Character = character;
    }

    public override string ToString()
    {
        return
            $"ERROR {(int)ErrorType}: Illegal character '{Character}' in file {PosStart.FileName}. ({PosStart.Line}, {PosStart.Column})";
    }
}