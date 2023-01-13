using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis;

public abstract class Node
{
    public abstract TokenKind Kind { get; }
    public abstract Position Start { get; }
    public abstract Position End { get; }
}