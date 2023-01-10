using Hada.Core.LexicalAnalysis;

namespace Hada.Core.Errors;

public sealed class InvalidNumberError : Error
{
    private readonly string _details;
    public override ErrorType ErrorType => ErrorType.InvalidNumber;

    public InvalidNumberError(string details, Position posStart, Position posEnd) : base(posStart, posEnd)
    {
        _details = details;
    }

    public override string ToString()
    {
        return $"ERROR {(int)ErrorType}: {_details}. File {PosStart.FileName}. ({PosStart.Line}, {PosStart.Column})";
    }
}