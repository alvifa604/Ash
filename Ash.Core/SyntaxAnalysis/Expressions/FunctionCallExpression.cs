using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Expressions;

public class FunctionCallExpression : Expression
{
    public override TokenKind Kind => TokenKind.FunctionCallExpression;
    public override Position Start { get; }
    public override Position End { get; }
    public List<ArgumentExpression> Arguments { get; }
    public Token IdentifierToken { get; }

    public FunctionCallExpression(Token identifierToken, List<ArgumentExpression> arguments)
    {
        IdentifierToken = identifierToken;
        Arguments = arguments;
        Start = identifierToken.Start;
        End = arguments[^1].End;
    }
}