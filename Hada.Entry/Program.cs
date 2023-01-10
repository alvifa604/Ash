using Hada.Core.Errors;
using Hada.Core.LexicalAnalysis;
using Hada.Core.Text;

namespace Hada.Entry;

public static class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Use file? (y/n)");
            var answer = Console.ReadLine();
            var text = "";
            var fileName = "";
            switch (answer)
            {
                case "y":
                {
                    Console.Write("Enter file name: ");
                    fileName = Console.ReadLine();
                    var path = $"/Users/alberto/Desktop/C#/Hada/Samples/{fileName}";
                    if (!IsValidExtension(path))
                    {
                        Console.WriteLine("Invalid extension");
                        continue;
                    }

                    if (!File.Exists(path))
                    {
                        Console.WriteLine("File not found");
                        continue;
                    }

                    text = File.ReadAllText(path);
                    break;
                }
                case "n":
                    Console.WriteLine("Enter your code:");
                    Console.Write(">> ");
                    text = Console.ReadLine();
                    fileName = "Console";
                    break;
                default:
                    continue;
            }

            if (string.IsNullOrEmpty(text))
                continue;

            var source = new SourceText(text, fileName);
            var lexerResult = RunLexer(source);
            if (lexerResult.errors.Any())
            {
                lexerResult.errors.WriteErrors();
                Console.WriteLine("Errors written");
            }
            else
            {
                foreach (var token in lexerResult.tokens)
                    Console.WriteLine(token.ToString());
            }
        }
    }


    private static bool IsValidExtension(string path)
    {
        return Path.GetExtension(path) == ".hada";
    }

    private static (Token[] tokens, ErrorsBag errors) RunLexer(SourceText source)
    {
        var lexer = new Lexer(source);
        var tokens = lexer.GenerateTokens().ToArray();
        var errors = lexer.ErrorsBag;
        return (tokens, errors);
    }
}