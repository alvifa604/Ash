using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Statements;

public class BreakStatement : Statement
{
    public Token BreakToken { get; }
    public override TokenKind Kind => TokenKind.BreakStatement;
    public override Position Start { get; }
    public override Position End { get; }

    public BreakStatement(Token breakToken)
    {
        BreakToken = breakToken;
        Start = BreakToken.Start;
        End = BreakToken.End;
    }
}