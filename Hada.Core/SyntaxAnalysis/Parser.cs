using Hada.Core.Errors;
using Hada.Core.LexicalAnalysis;
using Hada.Core.Text;

namespace Hada.Core.SyntaxAnalysis;

internal sealed class Parser
{
    public ErrorsBag ErrorsBag { get; } = new();
    private readonly Token[] _tokens;

    public Parser(SourceText source)
    {
        var lexer = new Lexer(source);
        if (lexer.ErrorsBag.Any())
            ErrorsBag.AddRange(lexer.ErrorsBag);
        _tokens = lexer.GenerateTokens().ToArray();
    }
}