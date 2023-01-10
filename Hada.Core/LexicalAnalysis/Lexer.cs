using Hada.Core.Errors;

namespace Hada.Core.LexicalAnalysis;

public sealed class Lexer
{
    //private int _position;
    private readonly Position _pos;
    private char Current => Peek();

    //Token information
    private int _start;
    private TokenKind _tokenKind;
    private object? _tokenValue;

    private readonly List<Error> _errors = new();
    public IEnumerable<Error> Errors => _errors;


    private readonly string _source;

    public Lexer(string source, string fileName)
    {
        _source = source;
        _pos = new Position(1, 0, 0, fileName, source);
    }

    public IEnumerable<Token> GenerateTokens()
    {
        Token token;
        do
        {
            token = NextToken();
            if (token.Kind is not (TokenKind.WhiteSpaceToken or TokenKind.BadToken))
                yield return token;
        } while (token.Kind is not TokenKind.EndOfFileToken);
    }

    private Token NextToken()
    {
        _start = _pos.Index;
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
            default:
                var posStart = _pos.Clone();
                var illegalChar = Current;
                Advance();
                var illegalCharError = new IllegalCharacterError(illegalChar, posStart, _pos.Clone());
                _errors.Add(illegalCharError);
                break;
        }

        var tokenLength = _pos.Index - _start;
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
        var start = _pos.Clone();
        var dotCount = 0;

        while (char.IsDigit(Current) || Current == ',')
        {
            if (Current is ',') dotCount++;
            Advance();
        }

        var numberLength = _pos.Index - _start;
        var numberText = _source.Substring(_start, numberLength);
        switch (dotCount)
        {
            case > 1:
            {
                var invalidNumberError =
                    new InvalidNumberError($"Decimals can't have more than one coma: {numberText}", start, _pos);
                _errors.Add(invalidNumberError);
                _tokenKind = TokenKind.BadToken;
                return;
            }
            case 0 when int.TryParse(numberText, out var integer):
                _tokenKind = TokenKind.IntegerToken;
                _tokenValue = integer;
                break;
            case 0:
            {
                var invalidNumberError = new InvalidNumberError($"{numberText} is not a valid Int32", start, _pos);
                _errors.Add(invalidNumberError);
                _tokenKind = TokenKind.BadToken;
                break;
            }
            default:
            {
                if (double.TryParse(numberText, out var @double))
                {
                    _tokenKind = TokenKind.DoubleToken;
                    _tokenValue = @double;
                }
                else
                {
                    var invalidNumberError = new InvalidNumberError($"{numberText} is not a valid double", start, _pos);
                    _errors.Add(invalidNumberError);
                    _tokenKind = TokenKind.BadToken;
                }

                break;
            }
        }
    }

    private void Advance()
    {
        _pos.Advance(Current);
    }

    private char Peek(int offset = 0)
    {
        var index = _pos.Index + offset;
        return index >= _source.Length
            ? '\0'
            : _source[index];
    }
}