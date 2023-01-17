using Ash.Core.LexicalAnalysis;

namespace Ash.Core.SyntaxAnalysis.Statements;

internal class FunctionDeclarationStatement : Statement
{
    public Token FunctionKeyword { get; }
    public Token IdentifierToken { get; }
    public List<ParameterNode> ParametersList { get; }
    public BlockStatement Body { get; }
    public override TokenKind Kind => TokenKind.FunctionDeclarationStatement;
    public override Position Start { get; }
    public override Position End { get; }

    public FunctionDeclarationStatement(Token functionKeyword, Token identifierToken,
        List<ParameterNode> parametersList,
        BlockStatement body)
    {
        FunctionKeyword = functionKeyword;
        IdentifierToken = identifierToken;
        ParametersList = parametersList;
        Body = body;
        Start = functionKeyword.Start;
        End = body.End;
    }
}