namespace Ash.Core.Interpretation.Symbols;

public abstract class Symbol
{
    public SymbolType SymbolType { get; }
    public virtual object? Value { get; }

    protected Symbol(SymbolType symbolType, object? value = null)
    {
        SymbolType = symbolType;
        Value = value;
    }
}