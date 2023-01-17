namespace Ash.Core.Interpretation.Symbols;

public sealed class SymbolTableV1
{
    private readonly Dictionary<string, object?> _symbols;
    public SymbolTableV1? Parent { get; }

    public object? this[string name]
    {
        get
        {
            _symbols.TryGetValue(name, out var value);
            if (value is null && Parent is not null)
                return Parent[name];
            return value;
        }
        set => _symbols[name] = value;
    }

    public SymbolTableV1(SymbolTableV1? parent = null)
    {
        _symbols = new Dictionary<string, object?>();
        Parent = parent;
    }

    public void Remove(string name)
    {
        _symbols.Remove(name);
    }
}