using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Expressions;

public class LiteralExpression : Expression
{
    public override TokenKind Kind { get; }
    public Token LiteralToken { get; }
    public override Position Start { get; }
    public override Position End { get; }

    public LiteralExpression(Token literalToken, Position start, Position end)
    {
        LiteralToken = literalToken;
        Start = start;
        End = end;
    }
}