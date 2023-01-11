using Hada.Core.LexicalAnalysis;

namespace Hada.Core.SyntaxAnalysis.Expressions;

public class UnaryExpression : Expression
{
    public override TokenKind Kind => TokenKind.UnaryExpression;
    public Token OperatorToken { get; }
    public Expression Expression { get; }

    public UnaryExpression(Token operatorToken, Expression expression)
    {
        OperatorToken = operatorToken;
        Expression = expression;
    }

    public override string ToString()
    {
        return $"{OperatorToken} {Expression}";
    }   
}