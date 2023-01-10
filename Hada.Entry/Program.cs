using Hada.Core.Errors;
using Hada.Core.LexicalAnalysis;

namespace Hada.Entry;

public static class Program
{
    public static void Main(string[] args)
    {
        var file = new FileStream("/Users/alberto/Desktop/C#/Hada/Samples/Addition.hada", FileMode.Open);
        var fileName = file.Name.Split('/').Last();
        var extension = fileName.Split('.').Last();
        if (extension != "hada")
        {
            Console.WriteLine("Invalid file extension");
            return;
        }

        using (var stream = new StreamReader(file))
        {
            var text = stream.ReadToEnd();
            if (string.IsNullOrEmpty(text))
                return;

            var lexerResult = RunLexer(text, fileName);
            if (lexerResult.errors.Any())
                foreach (var error in lexerResult.errors)
                    Console.WriteLine(error.ToString());
            else
                foreach (var token in lexerResult.tokens)
                    Console.WriteLine(token.ToString());
        }

        file.Close();

        /*while (true)
        {
            Console.Write(">> ");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                break;

            var lexerResult = RunLexer(input);
            if (lexerResult.errors.Any())
                foreach (var error in lexerResult.errors)
                    Console.WriteLine(error.ToString());
            else
                foreach (var token in lexerResult.tokens)
                    Console.WriteLine(token.ToString());
        }*/
    }

    private static (IEnumerable<Token> tokens, IEnumerable<Error> errors) RunLexer(string source, string fileName)
    {
        var lexer = new Lexer(source, fileName);
        var tokens = lexer.GenerateTokens().ToArray();
        var errors = lexer.Errors.ToArray();
        return (tokens, errors);
    }
}