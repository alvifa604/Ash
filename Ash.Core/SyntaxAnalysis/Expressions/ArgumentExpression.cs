using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Expressions;

public class ArgumentExpression : Expression
{
    public Expression Argument { get; }
    public override TokenKind Kind => TokenKind.ArgumentExpression;
    public override Position Start { get; }
    public override Position End { get; }

    public ArgumentExpression(Expression argument)
    {
        Argument = argument;
        Start = argument.Start;
        End = argument.End;
    }
}