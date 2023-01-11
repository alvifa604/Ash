using Hada.Core.LexicalAnalysis;
using Hada.Core.SyntaxAnalysis.Expressions;

namespace Hada.Core.SyntaxAnalysis;

public class LiteralExpression : Expression
{
    private readonly Token _literalToken;

    public LiteralExpression(Token literalToken)
    {
        _literalToken = literalToken;
    }

    public override TokenKind Kind { get; }
}