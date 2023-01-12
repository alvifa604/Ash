using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Expressions;

public class NumberExpression : Expression
{
    public Token Token { get; }
    public override TokenKind Kind => TokenKind.NumberExpression;
    public override Position Start { get; }
    public override Position End { get; }

    public NumberExpression(Token token, Position start, Position end)
    {
        Token = token;
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return Token.ToString();
    }
}