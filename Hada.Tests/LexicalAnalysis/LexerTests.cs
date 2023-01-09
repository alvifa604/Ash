using Hada.Core.LexicalAnalysis;
using Xunit;

namespace Hada.Tests.LexicalAnalysis;

public class LexerTests
{
    [Theory]
    [InlineData("32 + 22 / 2 * 4", 8)]
    [InlineData("32,12 - 12 + 221", 6)]
    [InlineData("32,12 - 12 + 221 * (21 + 1) / 2", 14)]
    public static void Lexer_MakesCorrectToken_Amount(string source, int amount)
    {
        var lexer = new Lexer(source);
        var tokens = new List<Token>();

        Token token;
        do
        {
            token = lexer.NextToken();
            if (token.Kind is not TokenKind.WhiteSpaceToken or TokenKind.BadToken)
                tokens.Add(token);
        } while (token.Kind is not TokenKind.EndOfFileToken);

        Assert.Equal(amount, tokens.Count);
    }

    [Theory]
    [MemberData(nameof(SetTokensWithKind))]
    public static void Lexer_MakesCorrectToken_WithRightKindAndText(string source, TokenKind kind)
    {
        var lexer = new Lexer(source);
        var tokens = new List<Token>();

        Token token;
        do
        {
            token = lexer.NextToken();
            if (token.Kind is not TokenKind.WhiteSpaceToken or TokenKind.BadToken)
                tokens.Add(token);
        } while (token.Kind is not TokenKind.EndOfFileToken);

        Assert.Equal(source, tokens[0].Text);
        Assert.Equal(kind, tokens[0].Kind);
    }

    public static IEnumerable<object[]> SetTokensWithKind()
    {
        yield return new object[] { "32", TokenKind.IntegerToken };
        yield return new object[] { "32,12", TokenKind.DoubleToken };
        yield return new object[] { "+", TokenKind.PlusToken };
        yield return new object[] { "-", TokenKind.MinusToken };
        yield return new object[] { "*", TokenKind.MultiplicationToken };
        yield return new object[] { "/", TokenKind.DivisionToken };
        yield return new object[] { "(", TokenKind.OpenParenthesisToken };
        yield return new object[] { ")", TokenKind.CloseParenthesisToken };
    }
}