using Ash.Core.LexicalAnalysis;
using Ash.Core.SyntaxAnalysis.Expressions;

namespace Ash.Core.SyntaxAnalysis.Statements;

public class IfStatement : Statement
{
    public Token IfKeywordToken { get; }
    public Expression Condition { get; }
    public Statement ThenStatement { get; }
    public override TokenKind Kind => TokenKind.IfStatement;
    public override Position Start => IfKeywordToken.Start;
    public override Position End => ThenStatement.End;

    public IfStatement(Token ifKeywordToken, Expression condition, Statement thenStatement)
    {
        IfKeywordToken = ifKeywordToken;
        Condition = condition;
        ThenStatement = thenStatement;
    }
}