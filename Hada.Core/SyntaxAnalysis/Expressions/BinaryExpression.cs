using Hada.Core.LexicalAnalysis;

namespace Hada.Core.SyntaxAnalysis.Expressions;

public sealed class BinaryExpression : Expression
{
    public Expression Left { get; }
    public Token OperatorToken { get; }
    public Expression Right { get; }
    public override Position Start { get; }
    public override Position End { get; }
    public override TokenKind Kind => TokenKind.BinaryExpression;


    public BinaryExpression(Expression left, Token operatorToken, Expression right, Position start, Position end)
    {
        Left = left;
        OperatorToken = operatorToken;
        Right = right;
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return $"({Left} {OperatorToken} {Right})";
    }
}

public abstract class Expression
{
    public abstract TokenKind Kind { get; }
    public abstract Position Start { get; }
    public abstract Position End { get; }
}