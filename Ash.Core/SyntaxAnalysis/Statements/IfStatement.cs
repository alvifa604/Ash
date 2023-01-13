using Ash.Core.LexicalAnalysis;
using Ash.Core.SyntaxAnalysis.Expressions;

namespace Ash.Core.SyntaxAnalysis.Statements;

public class IfStatement : Statement
{
    public Token IfKeywordToken { get; }
    public Expression Condition { get; }
    public Statement BodyStatement { get; }
    public override TokenKind Kind => TokenKind.IfStatement;
    public override Position Start => IfKeywordToken.Start;
    public override Position End { get; }
    public ElseStatement? ElseStatement { get; }

    public IfStatement(Token ifKeywordToken, Expression condition, Statement bodyStatement, Position end,
        ElseStatement? elseStatement = null)
    {
        IfKeywordToken = ifKeywordToken;
        Condition = condition;
        BodyStatement = bodyStatement;
        End = end;
        ElseStatement = elseStatement;
    }
}

public class ElseStatement : Statement
{
    public Token ElseKeyword { get; }
    public Statement BodyStatement { get; }
    public override TokenKind Kind => TokenKind.ElseStatement;
    public override Position Start { get; }
    public override Position End { get; }

    public ElseStatement(Token elseKeyword, Statement bodyStatement, Position end)
    {
        ElseKeyword = elseKeyword;
        BodyStatement = bodyStatement;
        Start = elseKeyword.Start;
        End = end;
    }
}