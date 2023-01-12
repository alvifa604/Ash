using Hada.Core.LexicalAnalysis;

namespace Hada.Core.Interpretation;

public class Context
{
    public string Name { get; }
    public Context? Parent { get; }
    public Position? ParentPosition { get; }

    public Context(string name, Context? parent = null, Position? parentPosition = null)
    {
        Name = name;
        Parent = parent;
        ParentPosition = parentPosition;
    }
}