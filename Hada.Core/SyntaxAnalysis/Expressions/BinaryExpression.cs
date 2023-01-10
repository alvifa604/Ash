using Hada.Core.LexicalAnalysis;

namespace Hada.Core.SyntaxAnalysis.Expressions;

public sealed class BinaryExpression : Expression
{
    public override TokenKind Kind => TokenKind.BinaryExpression;
    private readonly Expression _left;
    private readonly Token _operatorToken;
    private readonly Expression _right;

    public BinaryExpression(Expression left, Token operatorToken, Expression right)
    {
        _left = left;
        _operatorToken = operatorToken;
        _right = right;
    }

    public override string ToString()
    {
        return $"({_left} {_operatorToken} {_right})";
    }
}

public abstract class Expression
{
    public abstract TokenKind Kind { get; }
}