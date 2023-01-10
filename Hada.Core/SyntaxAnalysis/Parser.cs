using Hada.Core.Errors;
using Hada.Core.LexicalAnalysis;
using Hada.Core.SyntaxAnalysis.Expressions;

namespace Hada.Core.SyntaxAnalysis;

internal sealed class Parser
{
    public ErrorsBag ErrorsBag { get; } = new();
    private readonly Token[] _tokens;
    private int _position;

    private Token Current => _tokens[_position];

    public Parser(Token[] tokens)
    {
        _tokens = tokens;
        var endOfFileToken = _tokens.Last();
        //MatchToken(TokenKind.EndOfFileToken);
    }

    public Expression Parse()
    {
        return ParseExpression();
    }

    private Expression ParseExpression()
    {
        return ParseBinaryExpression(ParseTerm, TokenKind.PlusToken, TokenKind.MinusToken);
    }

    private Expression ParseTerm()
    {
        return ParseBinaryExpression(ParseFactor, TokenKind.MultiplicationToken, TokenKind.DivisionToken);
    }

    private Expression ParseBinaryExpression(Func<Expression> func, params TokenKind[] kinds)
    {
        var left = func();
        while (kinds.Contains(Current.Kind))
        {
            var operatorToken = NextToken();
            var right = func();
            left = new BinaryExpression(left, operatorToken, right);
        }

        return left;
    }

    private Expression ParseFactor()
    {
        if (Current.Kind is TokenKind.IntegerToken or TokenKind.DoubleToken)
        {
            var numberToken = NextToken();
            return new NumberExpression(numberToken);
        }

        return null;
    }

    private Token NextToken()
    {
        var current = Current;
        _position++;
        return current;
    }

    /*private Token MatchToken(TokenKind kind)
    {
        if (Current.Kind == kind) return NextToken();

        ErrorsBag.ReportUnexpectedToken($"UnexpectedToken {Current.Kind} expected {kind}", Current.);
        return new Token(kind, Current.Position, "", null);
    }*/
}