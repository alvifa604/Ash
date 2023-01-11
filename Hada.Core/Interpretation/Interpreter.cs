using Hada.Core.Errors;
using Hada.Core.LexicalAnalysis;
using Hada.Core.SyntaxAnalysis;
using Hada.Core.SyntaxAnalysis.Expressions;

namespace Hada.Core.Interpretation;

internal sealed class Interpreter
{
    private readonly SyntaxTree _tree;
    public ErrorsBag ErrorsBag { get; } = new();

    public Interpreter(SyntaxTree tree)
    {
        _tree = tree;
    }

    public object? Interpret()
    {
        var result = Visit(_tree.Root!);
        return result;
    }

    private object Visit(Expression node)
    {
        return (node switch
        {
            BinaryExpression binary => VisitBinary(binary),
            UnaryExpression unary => VisitUnary(unary),
            ParenthesizedExpression parenthesized => VisitParenthesized(parenthesized),
            //LiteralExpression literal => VisitLiteral(literal),
            NumberExpression number => number.Token.Value,
            _ => ErrorsBag.ReportInvalidExpression()
        })!;
    }

    private object VisitParenthesized(ParenthesizedExpression parenthesized)
    {
        return Visit(parenthesized.Expression);
    }

    private object? VisitUnary(UnaryExpression unary)
    {
        var @operator = unary.OperatorToken.Kind;
        dynamic expression = Visit(unary.Expression);

        return @operator switch
        {
            TokenKind.PlusToken => expression,
            TokenKind.MinusToken => -expression,
            _ => throw new ArgumentOutOfRangeException(nameof(@operator), @operator, null)
        };
    }

    private object? VisitBinary(BinaryExpression binary)
    {
        dynamic left = Visit(binary.Left);
        dynamic right = Visit(binary.Right);

        dynamic? result;

        switch (binary.OperatorToken.Kind)
        {
            case TokenKind.PlusToken:
                result = left + right;
                break;
            case TokenKind.MinusToken:
                result = left - right;
                break;
            case TokenKind.MultiplicationToken:
                result = left * right;
                break;
            case TokenKind.DivisionToken:
                result = left / right;
                break;
            default:
                ErrorsBag.ReportInvalidBinaryOperator(binary.OperatorToken);
                result = null;
                return result;
        }

        if (result is double res)
            return Math.Round(res, 10);
        return result;
    }
}