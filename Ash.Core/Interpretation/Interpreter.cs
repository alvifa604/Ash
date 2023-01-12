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
            LiteralExpression number => VisitLiteral(number),
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

    private object? VisitLiteral(LiteralExpression literal)
    {
        return literal.Value;
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

        switch (left)
        {
            case (double or int) when right is (double or int):
                return HandleNumbersOperators(left, right, binary);
            case bool when right is bool:
                return HandleBooleanOperators(left, right, binary);
            default:
                _errorsBag.ReportInvalidBinaryOperator(binary.OperatorToken.Text, left.GetType(), right.GetType(),
                    binary.OperatorToken.Start, binary.OperatorToken.End);
                return left;
        }
    }

    private object? HandleBooleanOperators(object left, object right, BinaryExpression binary)
    {
        switch (binary.OperatorToken.Kind)
        {
            case TokenKind.EqualsToken:
                return Equals(left, right);
            default:
                _errorsBag.ReportInvalidBinaryOperator(binary.OperatorToken.Text, left.GetType(), right.GetType(),
                    binary.OperatorToken.Start, binary.OperatorToken.End);
                return null;
        }
    }

    private object? HandleNumbersOperators(dynamic left, dynamic right, BinaryExpression binary)
    {
        switch (binary.OperatorToken.Kind)
        {
            case TokenKind.PlusToken:
                return left + right;
            case TokenKind.MinusToken:
                return left - right;
            case TokenKind.MultiplicationToken:
                return left * right;
            case TokenKind.ExponentiationToken:
                if (left != 0)
                    return Math.Pow(left, right);
                _errorsBag.ReportRunTimeError("The base cannot be zero", _context, binary.Right.Start,
                    binary.Right.End);
                return null;
            case TokenKind.DivisionToken:
                if (right != 0)
                    return (double)left / right;
                _errorsBag.ReportRunTimeError("Division by zero is not allowed", _context, binary.Right.Start,
                    binary.Right.End);
                return null;
            case TokenKind.EqualsToken:
                return left == right;
            default:
                _errorsBag.ReportInvalidBinaryOperator(binary.OperatorToken.Text, left.GetType(), right.GetType(),
                    binary.OperatorToken.Start, binary.OperatorToken.End);
                return null;
        }
    }
}