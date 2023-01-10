namespace Hada.Core.LexicalAnalysis;

public sealed class Token
{
    public TokenKind Kind { get; }
    public string Text { get; }
    public object? Value { get; }

    public Token(TokenKind kind, string text, object? value = null)
    {
        Kind = kind;
        Text = text;
        Value = value;
    }

    public override string ToString()
    {
        return Value is not null
            ? $"Kind: {Kind}. Text: {Text} ({Value})"
            : $"Kind: {Kind}. Text: {Text}";
    }
}