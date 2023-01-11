using Hada.Core.LexicalAnalysis;

namespace Hada.Core.SyntaxAnalysis.Expressions;

public sealed class BinaryExpression : Expression
{
    public Expression Left { get; }
    public Token OperatorToken { get; }
    public Expression Right { get; }
    public override TokenKind Kind => TokenKind.BinaryExpression;


    public BinaryExpression(Expression left, Token operatorToken, Expression right)
    {
        Left = left;
        OperatorToken = operatorToken;
        Right = right;
    }

    public override string ToString()
    {
        return $"({Left} {OperatorToken} {Right})";
    }
}

public abstract class Expression
{
    public abstract TokenKind Kind { get; }
}