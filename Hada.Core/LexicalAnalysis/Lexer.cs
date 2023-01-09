namespace Hada.Core.LexicalAnalysis;

public sealed class Lexer
{
    private int _position;
    private char Current => Peek();

    //Token information
    private int _start;
    private TokenKind _tokenKind;
    private object? _tokenValue;

    public List<string> Errors { get; } = new();

    private readonly string _source;

    public Lexer(string source)
    {
        _source = source;
    }

    public Token NextToken()
    {
        _start = _position;
        _tokenKind = TokenKind.BadToken;
        _tokenValue = null;

        switch (Current)
        {
            case '\0':
                _tokenKind = TokenKind.EndOfFileToken;
                break;
            case ' ':
            case '\t':
            case '\n':
            case '\r':
                MakeWhiteSpaceToken();
                break;
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
            case '0':
                MakeNumberToken();
                break;
            case '+':
                _tokenKind = TokenKind.PlusToken;
                Advance();
                break;
            case '-':
                _tokenKind = TokenKind.MinusToken;
                Advance();
                break;
            case '*':
                _tokenKind = TokenKind.MultiplicationToken;
                Advance();
                break;
            case '/':
                _tokenKind = TokenKind.DivisionToken;
                Advance();
                break;
            case '(':
                _tokenKind = TokenKind.OpenParenthesisToken;
                Advance();
                break;
            case ')':
                _tokenKind = TokenKind.CloseParenthesisToken;
                Advance();
                break;
        }

        var tokenLength = _position - _start;
        var tokenText = _tokenKind.GetText() ?? _source.Substring(_start, tokenLength);

        return new Token(_tokenKind, tokenText, _tokenValue);
    }

    private void MakeWhiteSpaceToken()
    {
        while (char.IsWhiteSpace(Current))
            Advance();
        _tokenKind = TokenKind.WhiteSpaceToken;
    }

    private void MakeNumberToken()
    {
        var dotCount = 0;

        while (char.IsDigit(Current) || Current == ',')
        {
            if (Current is ',') dotCount++;
            Advance();
        }

        if (dotCount > 1)
        {
            Errors.Add("Decimals can't have more than one dot");
            _tokenKind = TokenKind.BadToken;
        }

        var numberLength = _position - _start;
        var numberText = _source.Substring(_start, numberLength);
        if (dotCount == 1)
        {
            if (double.TryParse(numberText, out var @double))
            {
                _tokenKind = TokenKind.DoubleToken;
                _tokenValue = @double;
            }
            else
            {
                Errors.Add("Invalid double");
                _tokenKind = TokenKind.BadToken;
            }
        }
        else
        {
            if (int.TryParse(numberText, out var integer))
            {
                _tokenKind = TokenKind.IntegerToken;
                _tokenValue = integer;
            }
            else
            {
                Errors.Add("Invalid Int32");
                _tokenKind = TokenKind.BadToken;
            }
        }
    }

    private void Advance(int offset = 1)
    {
        _position += offset;
    }

    private char Peek(int offset = 0)
    {
        var index = _position + offset;
        return index >= _source.Length
            ? '\0'
            : _source[index];
    }
}