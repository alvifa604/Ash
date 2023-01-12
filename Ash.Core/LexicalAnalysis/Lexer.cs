using System.Runtime.CompilerServices;
using Ash.Core.Errors;
using Ash.Core.Text;

[assembly: InternalsVisibleTo("Ash.Tests")]

namespace Ash.Core.LexicalAnalysis;

internal sealed class Lexer
{
    private readonly SourceText _source;
    private readonly Position _position;
    private char Current => Peek();

    //Token information
    private int _start;
    private TokenKind _tokenKind;
    private object? _tokenValue;

    public ErrorsBag ErrorsBag { get; } = new();

    public Lexer(SourceText source)
    {
        _source = source;
        _position = new Position(1, 0, 0, source.FileName, source.Text);
    }

    public Token[] GenerateTokens()
    {
        var tokens = new List<Token>();
        Token token;
        do
        {
            token = NextToken();
            if (token.Kind is not (TokenKind.WhiteSpaceToken or TokenKind.BadToken))
                tokens.Add(token);
        } while (token.Kind is not TokenKind.EndOfFileToken);

        return tokens.ToArray();
    }

    private Token NextToken()
    {
        var posStart = _position.Clone();
        _start = _position.Index;
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
            case '^':
                _tokenKind = TokenKind.ExponentiationToken;
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
            case '=':
                _tokenKind = TokenKind.EqualsToken;
                Advance();
                break;
            default:
                if (char.IsLetter(Current))
                {
                    MakeKeywordOrIdentifierToken();
                    break;
                }

                var illegalChar = Current;
                Advance();
                ErrorsBag.ReportIllegalCharacterError(illegalChar, posStart, _position);
                break;
        }

        var tokenLength = _position.Index - _start;
        var tokenText = _tokenKind.GetText() ?? _source.Text.Substring(_start, tokenLength);

        return new Token(_tokenKind, tokenText, posStart, _position, _tokenValue);
    }

    private void MakeKeywordOrIdentifierToken()
    {
        while (char.IsLetterOrDigit(Current) || Current == '_')
            Advance();

        var text = _source.Text.Substring(_start, _position.Index - _start);
        _tokenKind = text.GetKeywordOrIdentifierKind();
    }

    private void MakeWhiteSpaceToken()
    {
        while (char.IsWhiteSpace(Current))
            Advance();
        _tokenKind = TokenKind.WhiteSpaceToken;
    }

    private void MakeNumberToken()
    {
        var start = _position.Clone();
        var dotCount = 0;

        while (char.IsDigit(Current) || Current == ',')
        {
            if (Current is ',') dotCount++;
            Advance();
        }

        var numberLength = _position.Index - _start;
        var numberText = _source.Text.Substring(_start, numberLength);
        switch (dotCount)
        {
            case > 1:
            {
                ErrorsBag.ReportInvalidNumberError("Decimals can't have more than one coma", start, _position);
                _tokenKind = TokenKind.BadToken;
                return;
            }
            case 0 when int.TryParse(numberText, out var integer):
                _tokenKind = TokenKind.IntegerToken;
                _tokenValue = integer;
                break;
            case 0:
            {
                ErrorsBag.ReportInvalidNumberError($"{numberText} is not a valid Int32", start, _position);
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
                    ErrorsBag.ReportInvalidNumberError($"{numberText} is not a valid double", start, _position);
                    _tokenKind = TokenKind.BadToken;
                }

                break;
            }
        }
    }

    private void Advance()
    {
        _position.Advance(Current);
    }

    private char Peek(int offset = 0)
    {
        var index = _position.Index + offset;
        return index >= _source.Text.Length
            ? '\0'
            : _source.Text[index];
    }
}