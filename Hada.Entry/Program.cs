using Hada.Core;
using Hada.Core.Errors;
using Hada.Core.LexicalAnalysis;
using Hada.Core.SyntaxAnalysis;
using Hada.Core.Text;

namespace Hada.Entry;

public static class Program
{
    public static void Main(string[] args)
    {
        const string path = "/Users/alberto/Desktop/C#/Hada/Samples/Example.hada";
        var text = File.ReadAllText(path);
        if (string.IsNullOrEmpty(text)) return;

        var compiler = new Compiler(text);
        var tree = compiler.Run();
        if (tree is null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Compilation failed!");
            Console.ResetColor();
            return;
        }

        Console.WriteLine(tree.Root);
    }


    /*while (true)
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
            }*/

    private static bool IsValidExtension(string path)
    {
        return Path.GetExtension(path) == ".hada";
    }
}