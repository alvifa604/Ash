using Ash.Core.Interpretation;
using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Statements;

internal class ParameterNode : Node
{
    public Token IdentifierToken { get; }
    public SymbolType SymbolType { get; }
    public override TokenKind Kind => TokenKind.Parameter;
    public override Position Start { get; }
    public override Position End { get; }

    public ParameterNode(Token parameter, SymbolType symbolType)
    {
        IdentifierToken = parameter;
        SymbolType = symbolType;
        Start = parameter.Start;
        End = parameter.End;
    }
}