using Hada.Core.LexicalAnalysis;

namespace Hada.Core.SyntaxAnalysis.Expressions;

public class NumberExpression : Expression
{
    public Token Token { get; }
    public override TokenKind Kind => TokenKind.NumberExpression;

    public NumberExpression(Token token)
    {
        Token = token;
    }

    public override string ToString()
    {
        return Token.ToString();
    }
}