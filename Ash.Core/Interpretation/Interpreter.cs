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
        var variableExists = _context.Symbols[variableName] != null;
        if (!variableExists)
        {
            _errorsBag.ReportRunTimeError($"Variable '{variableName}' is not defined.", _context, reAssignment.Start,
                reAssignment.End);
            return null;
        }

        var value = Visit(reAssignment.Expression);

        var newType = value?.GetType();
        var prevType = _context.Symbols[variableName]?.GetType();

        // Prevents reassignment of different types
        if (newType != prevType)
        {
            _errorsBag.ReportRunTimeError($"Type {newType?.Name} cannot be assigned to type {prevType?.Name}",
                _context, reAssignment.Start,
                reAssignment.End);
            return null;
        }

        _context.Symbols[variableName] = value;
        return value;
    }

    private object? VisitAssignment(AssignmentExpression assignment)
    {
        var variableName = assignment.IdentifierToken.Text;
        var variableExists = _context.Symbols[variableName] != null;
        if (variableExists)
        {
            _errorsBag.ReportRunTimeError($"Variable '{variableName}' is already defined.", _context,
                assignment.Start, assignment.End);
            return null;
        }

        var value = Visit(assignment.Expression);
        _context.Symbols[variableName] = value;
        return value;
    }

    private object? VisitVariable(VariableExpression variable)
    {
        var variableName = variable.IdentifierToken.Text;
        var variableExists = _context.Symbols[variableName] != null;
        if (variableExists)
            return _context.Symbols[variableName];

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
        var op = unary.OperatorToken;
        var expression = Visit(unary.Expression);

        if (expression is null) return null;

        switch (expression)
        {
            case int:
            case double:
                return HandleUnaryNumberOperators(op, expression, unary.Start, unary.End);
            case bool b:
                return HandleBooleanUnaryOperator(op, b, unary.Start, unary.End);
            default:
                _errorsBag.ReportInvalidUnaryOperator(op.Text, expression.GetType(), op.Start,
                    op.End);
                return null;
        }
    }

    private object? HandleBooleanUnaryOperator(Token op, bool value, Position start, Position end)
    {
        switch (op.Kind)
        {
            case TokenKind.LogicalNotToken:
                return !value;
            default:
                _errorsBag.ReportInvalidUnaryOperator(op.Text, value.GetType(),
                    start, end);
                return null;
        }
    }

    private object? HandleUnaryNumberOperators(Token op, dynamic number, Position start, Position end)
    {
        switch (op.Kind)
        {
            case TokenKind.PlusToken:
                return number;
            case TokenKind.MinusToken:
                return -number;
            default:
                _errorsBag.ReportInvalidUnaryOperator(op.Text, number.GetType(),
                    start, end);
                return null;
        }
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
                return HandleBooleanBinaryOperators(left, right, binary);
            default:
                _errorsBag.ReportInvalidBinaryOperator(binary.OperatorToken.Text, left.GetType(), right.GetType(),
                    binary.OperatorToken.Start, binary.OperatorToken.End);
                return null;
        }
    }

    private object? HandleBooleanBinaryOperators(object left, object right, BinaryExpression binary)
    {
        switch (binary.OperatorToken.Kind)
        {
            case TokenKind.EqualsToken:
                return Equals(left, right);
            case TokenKind.NotEqualsToken:
                return !Equals(left, right);
            case TokenKind.LogicalAndToken:
                return (bool)left && (bool)right;
            case TokenKind.LogicalOrToken:
                return (bool)left || (bool)right;
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
                return left + right is double d0 ? Math.Round(d0, 8) : left * right;
            case TokenKind.ExponentiationToken:
                if (left != 0)
                    return Math.Pow(left, right) is double d1 ? Math.Round(d1, 8) : Math.Pow(left, right);
                _errorsBag.ReportRunTimeError("The base cannot be zero", _context, binary.Right.Start,
                    binary.Right.End);
                return null;
            case TokenKind.DivisionToken:
                if (right != 0)
                    return left / right is double d2 ? Math.Round(d2, 8) : left / right;
                _errorsBag.ReportRunTimeError("Division by zero is not allowed", _context, binary.Right.Start,
                    binary.Right.End);
                return null;
            case TokenKind.EqualsToken:
                return left == right;
            case TokenKind.NotEqualsToken:
                return left != right;
            case TokenKind.GreaterThanToken:
                return left > right;
            case TokenKind.GreaterThanOrEqualToken:
                return left >= right;
            case TokenKind.LessThanToken:
                return left < right;
            case TokenKind.LessThanOrEqualToken:
                return left <= right;
            default:
                _errorsBag.ReportInvalidBinaryOperator(binary.OperatorToken.Text, left.GetType(), right.GetType(),
                    binary.OperatorToken.Start, binary.OperatorToken.End);
                return null;
        }
    }
}