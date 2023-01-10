using Hada.Core.LexicalAnalysis;
using Hada.Core.Text;
using Xunit;

namespace Hada.Tests.LexicalAnalysis;

public class LexerTests
{
    [Theory]
    [InlineData("32 + 22 / 2 * 4", 8)]
    [InlineData("32,12 - 12 + 221", 6)]
    [InlineData("32,12 - 12 + 221 * (21 + 1) / 2", 14)]
    public static void Lexer_MakesCorrectToken_Amount(string text, int amount)
    {
        var source = new SourceText(text, "Test.hada");
        var lexer = new Lexer(source);
        var tokens = lexer.GenerateTokens().ToList();

        Assert.Equal(amount, tokens.Count);
    }

    [Theory]
    [MemberData(nameof(SetTokensWithKind))]
    public static void Lexer_MakesCorrectToken_WithRightKindAndText(string text, TokenKind kind)
    {
        var source = new SourceText(text, "Test.hada");
        var lexer = new Lexer(source);
        var tokens = lexer.GenerateTokens().ToList();

        Assert.Equal(text, tokens[0].Text);
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