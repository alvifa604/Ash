namespace Ash.Core.Interpretation;

public static class Extensions
{
    public static SymbolType MatchType(this object item)
    {
        return item switch
        {
            int => SymbolType.Integer,
            double => SymbolType.Double,
            bool => SymbolType.Boolean,
            null => SymbolType.Null,
            _ => SymbolType.Any
        };
    }

    public static object DefaultValue(this SymbolType symbolType)
    {
        switch (symbolType)
        {
            case SymbolType.Boolean:
                return false;
            case SymbolType.Any:
            case SymbolType.Null:
            case SymbolType.Integer:
                return 0;
            case SymbolType.Double:
                return 0.0;
            default:
                return 0;
        }
    }
}