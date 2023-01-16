using Ash.Core.LexicalAnalysis;

namespace Ash.Core.Interpretation;

public sealed class Context
{
    public string Name { get; }
    public Context? Parent { get; }
    public Position? ParentPosition { get; }
    public SymbolTable Symbols { get; }

    public Context(string name, Context? parent, SymbolTable? symbols = null, Position? parentPosition = null)
    {
        Name = name;
        Symbols = symbols ?? new SymbolTable(parent?.Symbols);
        Parent = parent;
        ParentPosition = parentPosition;
    }
}