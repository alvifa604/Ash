using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Expressions;

internal class ReAssignmentExpression : Expression
{
    public Token IdentifierToken { get; }
    public Token EqualsToken { get; }
    public Expression Expression { get; }
    public override TokenKind Kind => TokenKind.ReAssignmentExpression;
    public override Position Start { get; }
    public override Position End { get; }

    public ReAssignmentExpression(Token identifierToken, Token equalsToken, Expression expression, Position start,
        Position end)
    {
        IdentifierToken = identifierToken;
        EqualsToken = equalsToken;
        Expression = expression;
        Start = start;
        End = end;
    }
}