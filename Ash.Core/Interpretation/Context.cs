using Ash.Core.LexicalAnalysis;

namespace Ash.Core.Interpretation;

public class Context
{
    public string Name { get; }
    public Context? Parent { get; }
    public Position? ParentPosition { get; }

    public Dictionary<string, object?> Variables { get; } = new();

    public Context(string name, Context? parent = null, Position? parentPosition = null)
    {
        Name = name;
        Parent = parent;
        ParentPosition = parentPosition;
    }
}