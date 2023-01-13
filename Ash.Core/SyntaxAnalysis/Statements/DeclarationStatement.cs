using Ash.Core.LexicalAnalysis;
using Ash.Core.SyntaxAnalysis.Expressions;

namespace Ash.Core.SyntaxAnalysis.Statements;

public sealed class DeclarationStatement : Statement
{
    public override TokenKind Kind => TokenKind.DeclarationStatement;
    public Token LetToken { get; }
    public Token IdentifierToken { get; }
    public Token EqualsToken { get; }
    public Expression Expression { get; }
    public override Position Start { get; }
    public override Position End { get; }

    public DeclarationStatement(Token letToken, Token identifierToken, Token equalsToken, Expression expression,
        Position start,
        Position end)
    {
        LetToken = letToken;
        IdentifierToken = identifierToken;
        EqualsToken = equalsToken;
        Expression = expression;
        Start = start;
        End = end;
    }
}