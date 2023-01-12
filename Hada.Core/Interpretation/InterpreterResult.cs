using Hada.Core.Errors;

namespace Hada.Core.Interpretation;

public class InterpreterResult
{
    public object? Result { get; }
    public ErrorsBag ErrorsBag { get; }

    public InterpreterResult(object? result, ErrorsBag errorsBag)
    {
        Result = result;
        ErrorsBag = errorsBag;
    }
}