using Hada.Core.LexicalAnalysis;

namespace Hada.Core.Errors;

public abstract class Error
{
    public string Message { get; }
    public string ErrorSource { get; }
    public Position PosStart { get; }
    public Position PosEnd { get; }
    public abstract ErrorType ErrorType { get; }

    protected Error(string message, Position posStart, Position posEnd)
    {
        Message = message;
        PosStart = posStart;
        PosEnd = posEnd;

        var errorLength = PosEnd.Line == PosStart.Line
            ? PosEnd.Column - PosStart.Column
            : posStart.Source.Length - PosStart.Column;
        ErrorSource = posStart.Source.Substring(PosStart.Column, errorLength);
    }

    public override string ToString()
    {
        return
            $"ERROR {(int)ErrorType}: {Message}, in file {PosStart.FileName}. ({PosStart.Line}, {PosStart.Column})";
    }
}