using Hada.Core.Errors;
using Hada.Core.LexicalAnalysis;
using Hada.Core.SyntaxAnalysis;
using Hada.Core.SyntaxAnalysis.Expressions;

namespace Hada.Core.Interpretation;

internal sealed class Interpreter
{
    private readonly SyntaxTree _tree;
    private readonly ErrorsBag _errorsBag = new();

    public Interpreter(SyntaxTree tree)
    {
        _tree = tree;
    }

    public InterpreterResult Interpret()
    {
        var result = Visit(_tree.Root!);
        return new InterpreterResult(result, _errorsBag);
    }

    private object? Visit(Expression node)
    {
        return node switch
        {
            BinaryExpression binary => VisitBinary(binary),
            UnaryExpression unary => VisitUnary(unary),
            ParenthesizedExpression parenthesized => VisitParenthesized(parenthesized),
            //LiteralExpression literal => VisitLiteral(literal),
            NumberExpression number => VisitNumber(number),
            _ => VisitLiteral(node)
        };
    }

    private object VisitLiteral(Expression node)
    {
        throw new NotImplementedException();
    }

    private object? VisitNumber(NumberExpression number)
    {
        return number.Token.Value;
    }

    private object? VisitParenthesized(ParenthesizedExpression parenthesized)
    {
        return Visit(parenthesized.Expression);
    }

    private object? VisitUnary(UnaryExpression unary)
    {
        var @operator = unary.OperatorToken;
        var expression = Visit(unary.Expression);

        if (expression is null) return null;

        switch (expression)
        {
            case int:
            case double:
                dynamic number = expression;
                return @operator.Kind switch
                {
                    TokenKind.PlusToken => number,
                    TokenKind.MinusToken => -number,
                    _ => throw new NotImplementedException()
                };
        }

        _errorsBag.ReportInvalidUnaryOperator(@operator.Text, expression.GetType(), @operator.Start, @operator.End);
        return expression;
    }

    private object? VisitBinary(BinaryExpression binary)
    {
        var left = Visit(binary.Left);
        var right = Visit(binary.Right);

        if (left is null || right is null)
            return null;

        if (left is (double or int) && right is (double or int))
        {
            dynamic l = left;
            dynamic r = right;
            var result = l;
            switch (binary.OperatorToken.Kind)
            {
                case TokenKind.PlusToken:
                    result = l + r;
                    break;
                case TokenKind.MinusToken:
                    result = l - r;
                    break;
                case TokenKind.MultiplicationToken:
                    result = l * r;
                    break;
                case TokenKind.DivisionToken:
                    if (r == 0)
                    {
                        _errorsBag.ReportDivisionByZero(binary.Right.Start, binary.Right.End);
                        return null;
                    }

                    result = (double)l / r;
                    break;
            }

            if (result is double res)
                return Math.Round(res, 10);
            return result;
        }

        _errorsBag.ReportInvalidBinaryOperator(binary.OperatorToken.Text, left.GetType(), right.GetType(),
            binary.OperatorToken.Start, binary.OperatorToken.End);
        return left;
    }
}