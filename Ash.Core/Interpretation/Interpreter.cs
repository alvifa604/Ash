using Ash.Core.Errors;
using Ash.Core.LexicalAnalysis;
using Ash.Core.SyntaxAnalysis;
using Ash.Core.SyntaxAnalysis.Expressions;

namespace Ash.Core.Interpretation;

internal sealed class Interpreter
{
    private readonly Context _context;
    private readonly SyntaxTree _tree;
    private readonly ErrorsBag _errorsBag = new();

    public Interpreter(SyntaxTree tree, Context context)
    {
        _context = context;
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
            AssignmentExpression assignment => VisitAssignment(assignment),
            ReAssignmentExpression reAssignment => VisitReAssignment(reAssignment),
            NumberExpression number => VisitNumber(number),
            _ => VisitVariable((VariableExpression)node)
        };
    }

    private object? VisitReAssignment(ReAssignmentExpression reAssignment)
    {
        var variableName = reAssignment.IdentifierToken.Text;
        var variableExists = _context.Variables.ContainsKey(variableName);
        if (!variableExists)
        {
            _errorsBag.ReportRunTimeError($"Variable '{variableName}' is not defined.", _context, reAssignment.Start,
                reAssignment.End);
            return null;
        }

        var value = Visit(reAssignment.Expression);
        _context.Variables[variableName] = value;
        return value;
    }

    private object? VisitAssignment(AssignmentExpression assignment)
    {
        var variableName = assignment.IdentifierToken.Text;
        var variableExists = _context.Variables.ContainsKey(variableName);
        if (variableExists)
        {
            _errorsBag.ReportRunTimeError($"Variable '{variableName}' is already defined.", _context,
                assignment.Start, assignment.End);
            return null;
        }

        var value = Visit(assignment.Expression);
        _context.Variables[variableName] = value;
        return value;
    }

    private object? VisitVariable(VariableExpression variable)
    {
        var variableName = variable.IdentifierToken.Text;
        var variableExists = _context.Variables.ContainsKey(variableName);
        if (variableExists)
            return _context.Variables[variableName];

        _errorsBag.ReportRunTimeError($"Variable '{variableName}' is not defined.", _context, variable.Start,
            variable.End);
        return null;
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
                case TokenKind.ExponentiationToken:
                    if (l == 0)
                    {
                        _errorsBag.ReportRunTimeError("The base cannot be zero", _context, binary.Right.Start,
                            binary.Right.End);
                        return null;
                    }

                    result = Math.Pow(l, r);
                    break;
                case TokenKind.DivisionToken:
                    if (r == 0)
                    {
                        _errorsBag.ReportRunTimeError("Division by zero is not allowed", _context, binary.Right.Start,
                            binary.Right.End);
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