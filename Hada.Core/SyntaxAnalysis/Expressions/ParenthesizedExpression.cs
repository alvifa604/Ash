using Hada.Core.LexicalAnalysis;

namespace Hada.Core.SyntaxAnalysis.Expressions;

public class ParenthesizedExpression : Expression
{
    public Token OpenParenthesisToken { get; }
    public Expression Expression { get; }
    public Token CloseParenthesisToken { get; }
    public override TokenKind Kind => TokenKind.ParenthesizedExpression;
    public override Position Start { get; }
    public override Position End { get; }

    public ParenthesizedExpression(Token openParenthesisToken, Expression expression, Token closeParenthesisToken,
        Position start, Position end)
    {
        OpenParenthesisToken = openParenthesisToken;
        Expression = expression;
        CloseParenthesisToken = closeParenthesisToken;
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return $"({Expression})";
    }
}