using Hada.Core.LexicalAnalysis;

namespace Hada.Core.Errors;

public sealed class InvalidNumberError : Error
{
    public string Details { get; }
    public override ErrorType ErrorType => ErrorType.InvalidNumber;

    public InvalidNumberError(string details, Position posStart, Position posEnd) : base(posStart, posEnd)
    {
        Details = details;
    }

    public override string ToString()
    {
        return $"ERROR {(int)ErrorType}: {Details}. File {PosStart.FileName}. ({PosStart.Line}, {PosStart.Column})";
    }
}