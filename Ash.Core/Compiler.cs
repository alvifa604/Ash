using Ash.Core.Errors;
using Ash.Core.Interpretation;
using Ash.Core.LexicalAnalysis;
using Ash.Core.SyntaxAnalysis;
using Ash.Core.Text;

namespace Ash.Core;

public sealed class Compiler
{
    private readonly Context _context;
    private SourceText _sourceText = new("", "");

    public Compiler()
    {
        var globalSymbolTable = new SymbolTable
        {
            ["null"] = 0
        };
        _context = new Context("main", null, globalSymbolTable);
    }

    public InterpreterResult? Run(string text)
    {
        _sourceText = new SourceText(text, "Example.hada");
        var tokens = SetTokens();
        var tree = tokens.Length == 0
            ? null
            : SetSyntaxTree(tokens);

        return tree is null
            ? null
            : SetInterpreterResult(tree);
    }

    private InterpreterResult? SetInterpreterResult(SyntaxTree tree)
    {
        var result = new Interpreter(tree, _context).Interpret();

        if (result.Result is not null)
            return result;

        PrintErrorsInConsole(result.ErrorsBag);
        result.ErrorsBag.WriteErrorsInFile(_sourceText);
        return null;
    }

    private SyntaxTree? SetSyntaxTree(Token[] tokens)
    {
        var parser = new Parser(tokens);
        var tree = parser.Parse();
        if (!tree.ErrorsBag.Any())
            return tree;

        PrintErrorsInConsole(tree.ErrorsBag);
        tree.ErrorsBag.WriteErrorsInFile(_sourceText);
        return null;
    }

    private Token[] SetTokens()
    {
        var lexer = new Lexer(_sourceText);
        var tokens = lexer.GenerateTokens();
        if (!lexer.ErrorsBag.Any())
            return tokens;

        PrintErrorsInConsole(lexer.ErrorsBag);
        lexer.ErrorsBag.WriteErrorsInFile(_sourceText);
        return Array.Empty<Token>();
    }

    private void PrintErrorsInConsole(ErrorsBag errors)
    {
        Console.WriteLine(_sourceText.Text);
        foreach (var error in errors)
        {
            Console.Write($"{error}");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            //Console.Write($"Error: '{error.ErrorSource}'");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}