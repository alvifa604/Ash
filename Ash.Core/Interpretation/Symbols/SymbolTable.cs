namespace Ash.Core.Interpretation.Symbols;

public class SymbolTable
{
    private readonly Dictionary<string, Symbol?> _symbols;
    public SymbolTable? Parent { get; }

    public Symbol? this[string name]
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
        _symbols = new Dictionary<string, Symbol?>();
        Parent = parent;
    }

    public void Remove(string name)
    {
        _symbols.Remove(name);
    }

    public void Add(string variableName, Symbol symbol)
    {
        if (!_symbols.ContainsKey(variableName))
            _symbols.Add(variableName, symbol);
        else
            Update(variableName, symbol);
    }

    public void Update(string variableName, Symbol symbol)
    {
        _symbols[variableName] = symbol;
    }
}