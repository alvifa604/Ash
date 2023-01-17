using Ash.Core.LexicalAnalysis;
using Ash.Core.SyntaxAnalysis.Expressions;

namespace Ash.Core.SyntaxAnalysis.Statements;

public class WhileStatement : Statement
{
    public Token WhileToken { get; }
    public Expression Condition { get; }
    public Statement Body { get; }
    public override TokenKind Kind => TokenKind.WhileStatement;
    public override Position Start { get; }
    public override Position End { get; }

    public WhileStatement(Token whileToken, Expression condition, Statement body)
    {
        WhileToken = whileToken;
        Condition = condition;
        Body = body;
        Start = whileToken.Start;
        End = body.End;
    }
}