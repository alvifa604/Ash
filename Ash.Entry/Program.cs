using Ash.Core;

namespace Ash.Entry;

public static class Program
{
    public static void Main(string[] args)
    {
        //const string path = "/Users/alberto/Desktop/C#/Hada/Samples/Example.ash";
        //var text = File.ReadAllText(path);
        //if (string.IsNullOrEmpty(text)) return;

        var compiler = new Compiler();
        //var result = compiler.Run(text);
        //Console.WriteLine(result?.Result);
        while (true)
        {
            Console.Write(">> ");
            var text = Console.ReadLine();
            if (string.IsNullOrEmpty(text)) return;

            var interpreterResult = compiler.Run(text);
            if (interpreterResult is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Compilation failed!");
                Console.ResetColor();
                continue;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(interpreterResult.Result);
            Console.ResetColor();
        }
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
        return Path.GetExtension(path) == ".ash";
    }
}