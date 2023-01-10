using Hada.Core.LexicalAnalysis;

namespace Hada.Core.Errors;

public abstract class Error
{
    public Position PosStart { get; }
    public Position PosEnd { get; }
    public abstract ErrorType ErrorType { get; }

    protected Error(Position posStart, Position posEnd)
    {
        PosStart = posStart;
        PosEnd = posEnd;
    }
}