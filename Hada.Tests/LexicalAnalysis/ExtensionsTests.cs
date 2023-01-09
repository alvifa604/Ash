using Hada.Core.LexicalAnalysis;
using Xunit;

namespace Hada.Tests.LexicalAnalysis;

public class ExtensionsTests
{
    [Theory]
    [MemberData(nameof(GetTokenKinds))]
    public void TokenKind_GetText_ReturnsCorrectText(TokenKind kind, string text)
    {
        var kindText = kind.GetText();
        Assert.Equal(text, kindText);
    }

    public static IEnumerable<object[]> GetTokenKinds()
    {
        var pairs = GetTokenKindsWithText();
        foreach (var pair in pairs)
            yield return new object[] { pair.kind, pair.text };
    }

    private static IEnumerable<(TokenKind kind, string? text)> GetTokenKindsWithText()
    {
        return Enum.GetValues<TokenKind>()
            .Select(k => (kind: k, text: k.GetText()));
    }
}