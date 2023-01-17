using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Statements;

public class BlockStatement : Statement
{
    public Statement Body { get; }
    public Token OpenBraceToken { get; }
    public Token CloseBraceToken { get; }
    public override TokenKind Kind => TokenKind.BlockStatement;
    public override Position Start { get; }
    public override Position End { get; }

    public BlockStatement(Statement body, Token openBraceToken, Token closeBraceToken)
    {
        OpenBraceToken = openBraceToken;
        Body = body;
        CloseBraceToken = closeBraceToken;
        Start = openBraceToken.Start;
        End = closeBraceToken.End;
    }
}