namespace Ash.Core.Interpretation.Symbols;

internal class VariableSymbol : Symbol
{
    public VariableSymbol(SymbolType symbolType, object? value) : base(symbolType, value)
    {
    }
}