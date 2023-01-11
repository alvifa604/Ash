using Hada.Core.LexicalAnalysis;
using Hada.Core.SyntaxAnalysis;
using Hada.Core.SyntaxAnalysis.Expressions;
using Hada.Core.Text;

namespace Hada.Core;

public sealed class Compiler
{
    private readonly SourceText _sourceText;

    public Compiler(string text)
    {
        _sourceText = new SourceText(text, "Example.hada");
    }

    public SyntaxTree? Run()
    {
        var tokens = SetTokens();
        return tokens.Length == 0
            ? null
            : SetSyntaxTree(tokens);
    }

    private SyntaxTree? SetSyntaxTree(Token[] tokens)
    {
        var parser = new Parser(tokens);
        var tree = parser.Parse();
        if (!tree.ErrorsBag.Any())
            return tree;

        tree.ErrorsBag.WriteErrors(_sourceText);
        return null;
    }

    private Token[] SetTokens()
    {
        var lexer = new Lexer(_sourceText);
        var tokens = lexer.GenerateTokens();
        if (!lexer.ErrorsBag.Any())
            return tokens;

        lexer.ErrorsBag.WriteErrors(_sourceText);
        return Array.Empty<Token>();
    }
}