using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Expressions;

public class LiteralExpression : Expression
{
    public Token Token { get; }
    public override TokenKind Kind => TokenKind.NumberExpression;
    public override Position Start { get; }
    public override Position End { get; }
    public object? Value { get; }

    public LiteralExpression(Token token, Position start, Position end)
    {
        Token = token;
        Start = start;
        End = end;
    }

    public LiteralExpression(Token token, Position start, Position end, object value) : this(token, start, end)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Token.ToString();
    }
}