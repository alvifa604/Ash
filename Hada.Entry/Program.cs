using Hada.Core.LexicalAnalysis;

namespace Hada.Entry;

public static class Program
{
    public static void Main(string[] args)
    {
        var lex = new Lexer("23,48 + 12 - 42 / 2 * 65,321");
        var tokens = new List<Token>();
        Token token;
        do
        {
            token = lex.NextToken();
            if (token.Kind is not TokenKind.WhiteSpaceToken or TokenKind.BadToken)
                tokens.Add(token);
        } while (token.Kind is not TokenKind.EndOfFileToken);

        if (!lex.Errors.Any())
            foreach (var t in tokens)
                Console.WriteLine(t.ToString());

        foreach (var lexError in lex.Errors)
            Console.WriteLine(lexError);
    }
}