using System.Text;
using Ash.Core.Interpretation;
using Ash.Core.LexicalAnalysis;

namespace Ash.Core.Errors;

public class RunTimeError : Error
{
    public Context? Context { get; }

    public RunTimeError(string message, Position posStart, Position posEnd, Context? context = null) : base(message,
        posStart,
        posEnd)
    {
        Context = context;
    }

    public override ErrorType ErrorType => ErrorType.RunTime;

    public override string ToString()
    {
        var result = new StringBuilder();

        result.AppendLine(GenerateTraceBack());
        result.AppendLine($"ERROR {(int)ErrorType}: {Message}.");
        return $"Traceback (most recent call last): \n {result}";
    }

    private string GenerateTraceBack()
    {
        var result = new StringBuilder();

        var currContext = Context;
        while (currContext != null)
        {
            result.AppendLine($"File {PosStart.FileName}, in {currContext.Name}. {PosStart.Line}:{PosStart.Column}");
            currContext = currContext.Parent;
        }

        return result.ToString();
    }
}