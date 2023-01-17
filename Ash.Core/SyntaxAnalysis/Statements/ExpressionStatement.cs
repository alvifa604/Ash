using Ash.Core.LexicalAnalysis;
using Ash.Core.SyntaxAnalysis.Expressions;

namespace Ash.Core.SyntaxAnalysis.Statements;

internal class ExpressionStatement : Statement
{
    public override TokenKind Kind => TokenKind.ExpressionStatement;
    public Expression Expression { get; }
    public override Position Start { get; }
    public override Position End { get; }

    public ExpressionStatement(Expression expression)
    {
        Expression = expression;
        Start = expression.Start;
        End = expression.End;
    }
}