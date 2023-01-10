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

    public Expression? Run()
    {
        var tokens = SetTokens();
        return tokens.Length == 0
            ? null
            : SetSyntaxTree(tokens);
    }

    private Expression? SetSyntaxTree(Token[] tokens)
    {
        var parser = new Parser(tokens);
        var expression = parser.Parse();
        if (!parser.ErrorsBag.Any())
            return expression;

        parser.ErrorsBag.WriteErrors(_sourceText);
        return null;
    }

    private Token[] SetTokens()
    {
        var lexer = new Lexer(_sourceText);
        var tokens = lexer.GenerateTokens().ToArray();
        if (!lexer.ErrorsBag.Any())
            return tokens;

        lexer.ErrorsBag.WriteErrors(_sourceText);
        return Array.Empty<Token>();
    }
}