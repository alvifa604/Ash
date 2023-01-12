using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Expressions;

public class VariableExpression : Expression
{
    public override TokenKind Kind { get; }
    public Token IdentifierToken { get; }
    public override Position Start { get; }
    public override Position End { get; }

    public VariableExpression(Token identifierToken, Position start, Position end)
    {
        IdentifierToken = identifierToken;
        Start = start;
        End = end;
    }
}