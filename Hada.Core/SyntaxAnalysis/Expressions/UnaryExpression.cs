using Hada.Core.LexicalAnalysis;

namespace Hada.Core.SyntaxAnalysis.Expressions;

public class UnaryExpression : Expression
{
    public override TokenKind Kind => TokenKind.UnaryExpression;
    public override Position Start { get; }
    public override Position End { get; }
    public Token OperatorToken { get; }
    public Expression Expression { get; }

    public UnaryExpression(Token operatorToken, Expression expression, Position start, Position end)
    {
        OperatorToken = operatorToken;
        Expression = expression;
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return $"{OperatorToken} {Expression}";
    }
}