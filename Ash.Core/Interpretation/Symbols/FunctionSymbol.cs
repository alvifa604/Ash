using Ash.Core.SyntaxAnalysis.Statements;

namespace Ash.Core.Interpretation.Symbols;

internal class FunctionSymbol : Symbol
{
    public List<ParameterNode> ParametersList { get; }
    public override BlockStatement Value { get; }


    public FunctionSymbol(List<ParameterNode> parametersList, BlockStatement body) : base(SymbolType.Function)
    {
        ParametersList = parametersList;
        Value = body;
    }
}