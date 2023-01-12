using Ash.Core.LexicalAnalysis;

namespace Ash.Core.Interpretation;

public sealed class Context
{
    public string Name { get; }
    public Context? Parent { get; }
    public Position? ParentPosition { get; }

    public SymbolTable Symbols { get; }

    public Context(string name, SymbolTable symbols, Context? parent = null, Position? parentPosition = null)
    {
        Name = name;
        Symbols = symbols;
        Parent = parent;
        ParentPosition = parentPosition;
    }
}