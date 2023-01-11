namespace Hada.Core.LexicalAnalysis;

public sealed class Token
{
    public TokenKind Kind { get; }
    public string Text { get; }
    public object? Value { get; }
    public Position? Start { get; }
    public Position? End { get; }

    public Token(TokenKind kind, string text, object? value = null, Position? start = null, Position? end = null)
    {
        Kind = kind;
        Text = text;
        Value = value;
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return Value is not null
            ? $"Kind: {Kind}. Text: {Text} ({Value})"
            : $"Kind: {Kind}. Text: {Text}";
    }
}