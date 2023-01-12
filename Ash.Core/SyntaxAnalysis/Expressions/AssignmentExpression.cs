using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Expressions;

public class AssignmentExpression : Expression
{
    public override TokenKind Kind => TokenKind.AssignmentExpression;
    public Token LetToken { get; }
    public Token IdentifierToken { get; }
    public Token EqualsToken { get; }
    public Expression Expression { get; }
    public override Position Start { get; }
    public override Position End { get; }

    public AssignmentExpression(Token letToken, Token identifierToken, Token equalsToken, Expression expression,
        Position start,
        Position end)
    {
        LetToken = letToken;
        IdentifierToken = identifierToken;
        EqualsToken = equalsToken;
        Expression = expression;
        Start = start;
        End = end;
    }
}