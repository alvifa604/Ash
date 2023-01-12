namespace Ash.Core.Interpretation;

public sealed class SymbolTable
{
    private readonly Dictionary<string, object?> _symbols;
    public SymbolTable? Parent { get; }

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

    public SymbolTable(SymbolTable? parent = null)
    {
        _symbols = new Dictionary<string, object?>();
        Parent = parent;
    }

    public void Remove(string name)
    {
        _symbols.Remove(name);
    }
}