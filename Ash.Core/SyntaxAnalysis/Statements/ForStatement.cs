using Ash.Core.LexicalAnalysis;
using Ash.Core.SyntaxAnalysis.Expressions;

namespace Ash.Core.SyntaxAnalysis.Statements;

public class ForStatement : Statement
{
    public Token ForKeyword { get; }
    public DeclarationStatement LowerBoundDeclaration { get; }
    public Token ToKeyword { get; }
    public Expression UpperBound { get; }
    public Token? StepKeyword { get; }
    public Statement Body { get; }
    public Expression? Step { get; }
    public override TokenKind Kind => TokenKind.ForStatement;
    public override Position Start { get; }
    public override Position End { get; }

    public ForStatement(Token forKeyword, DeclarationStatement lowerBoundDeclaration, Token toKeyword,
        Expression upperBound, Statement body, Token? stepKeyword = null, Expression? step = null)
    {
        ForKeyword = forKeyword;
        LowerBoundDeclaration = lowerBoundDeclaration;
        ToKeyword = toKeyword;
        UpperBound = upperBound;
        Body = body;
        StepKeyword = stepKeyword;
        Step = step;
        Start = forKeyword.Start;
        End = body.End;
    }
}