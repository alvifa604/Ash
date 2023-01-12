using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Expressions;

public class LiteralExpression : Expression
{
    public Token Token { get; }
    public override TokenKind Kind => TokenKind.LiteralExpression;
    public override Position Start { get; }
    public override Position End { get; }
    public object? Value { get; }

    public LiteralExpression(Token token, Position start, Position end) : this(token, start, end, token.Value)
    {
    }

    public LiteralExpression(Token token, Position start, Position end, object? value)
    {
        Token = token;
        Start = start;
        End = end;
        Value = value;
    }

    public override string ToString()
    {
        return Token.ToString();
    }
}