using Hada.Core.LexicalAnalysis;

namespace Hada.Core.SyntaxAnalysis.Expressions;

public class ParenthesizedExpression : Expression
{
    public Token OpenParenthesisToken { get; }
    public Expression Expression { get; }
    public Token CloseParenthesisToken { get; }
    public override TokenKind Kind => TokenKind.ParenthesizedExpression;

    public ParenthesizedExpression(Token openParenthesisToken, Expression expression, Token closeParenthesisToken)
    {
        OpenParenthesisToken = openParenthesisToken;
        Expression = expression;
        CloseParenthesisToken = closeParenthesisToken;
    }
    
    public override string ToString() => $"({Expression})";
}