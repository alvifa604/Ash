using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Expressions;

internal class AssignmentExpression : Expression
{
    public Token IdentifierToken { get; }
    public Token EqualsToken { get; }
    public Expression Expression { get; }
    public override TokenKind Kind => TokenKind.AssignmentExpression;
    public override Position Start { get; }
    public override Position End { get; }

    public AssignmentExpression(Token identifierToken, Token equalsToken, Expression expression)
    {
        IdentifierToken = identifierToken;
        EqualsToken = equalsToken;
        Expression = expression;
        Start = identifierToken.Start;
        End = expression.End;
    }
}