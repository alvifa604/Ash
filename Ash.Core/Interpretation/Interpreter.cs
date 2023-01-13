using Ash.Core.Errors;
using Ash.Core.LexicalAnalysis;
using Ash.Core.SyntaxAnalysis;
using Ash.Core.SyntaxAnalysis.Expressions;
using Ash.Core.SyntaxAnalysis.Statements;

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
        var result = VisitStatement(_tree.Root!);
        return new InterpreterResult(result, _errorsBag);
    }

    private object? VisitStatement(Node node)
    {
        return node switch
        {
            DeclarationStatement assignment => VisitDeclaration(assignment, _context),
            IfStatement ifStatement => VisitIfStatement(ifStatement, _context),
            ElseStatement elseStatement => VisitElseStatement(elseStatement, _context),
            ExpressionStatement expression => VisitExpression(expression.Expression, _context),
            _ => VisitExpression(node, _context)
        };
    }

    private object? VisitDeclaration(DeclarationStatement declaration, Context context)
    {
        var variableName = declaration.IdentifierToken.Text;
        var variableExists = context.Symbols[variableName] != null;
        if (variableExists)
        {
            _errorsBag.ReportRunTimeError($"Variable '{variableName}' is already defined.", context,
                declaration.Start, declaration.End);
            return null;
        }

        var value = VisitExpression(declaration.Expression, context);
        context.Symbols[variableName] = value;
        return value;
    }

    private object? VisitIfStatement(IfStatement ifStatement, Context context)
    {
        var condition = VisitExpression(ifStatement.Condition, context);
        if (condition is not bool booleanCondition)
        {
            _errorsBag.ReportRunTimeError("Condition must be a boolean.", context,
                ifStatement.Condition.Start, ifStatement.Condition.End);
            return null;
        }

        if (booleanCondition)
            return VisitStatement(ifStatement.BodyStatement);
        return ifStatement.ElseStatement != null
            ? VisitStatement(ifStatement.ElseStatement)
            : "";
    }

    private object? VisitElseStatement(ElseStatement elseStatement, Context context)
    {
        return VisitStatement(elseStatement.BodyStatement);
    }

    private object? VisitExpression(Node node, Context context)
    {
        return node switch
        {
            BinaryExpression binary => VisitBinary(binary, context),
            UnaryExpression unary => VisitUnary(unary, context),
            ParenthesizedExpression parenthesized => VisitParenthesized(parenthesized, context),
            AssignmentExpression assignment => VisitAssignment(assignment, context),
            LiteralExpression number => VisitLiteral(number, context),
            _ => VisitVariable((VariableExpression)node, context)
        };
    }

    private object? VisitAssignment(AssignmentExpression assignment, Context context)
    {
        var variableName = assignment.IdentifierToken.Text;
        var variableExists = context.Symbols[variableName] != null;
        if (!variableExists)
        {
            _errorsBag.ReportRunTimeError($"Variable '{variableName}' is not defined.", context, assignment.Start,
                assignment.End);
            return null;
        }

        var value = VisitExpression(assignment.Expression, context);

        var newType = value?.GetType();
        var prevType = context.Symbols[variableName]?.GetType();

        // Prevents reassignment of different types
        if (newType != prevType)
        {
            _errorsBag.ReportRunTimeError($"Type {newType?.Name} cannot be assigned to type {prevType?.Name}",
                context, assignment.Start,
                assignment.End);
            return null;
        }

        context.Symbols[variableName] = value;
        return value;
    }

    private object? VisitVariable(VariableExpression variable, Context context)
    {
        var variableName = variable.IdentifierToken.Text;
        var variableExists = context.Symbols[variableName] != null;
        if (variableExists)
            return context.Symbols[variableName];

        _errorsBag.ReportRunTimeError($"Variable '{variableName}' is not defined.", context, variable.Start,
            variable.End);
        return null;
    }

    private object? VisitLiteral(LiteralExpression literal, Context context)
    {
        return literal.Value;
    }

    private object? VisitParenthesized(ParenthesizedExpression parenthesized, Context context)
    {
        return VisitExpression(parenthesized.Expression, context);
    }

    private object? VisitUnary(UnaryExpression unary, Context context)
    {
        var op = unary.OperatorToken;
        var expression = VisitExpression(unary.Expression, context);

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

    private object? VisitBinary(BinaryExpression binary, Context context)
    {
        var left = VisitExpression(binary.Left, context);
        var right = VisitExpression(binary.Right, context);

        if (left is null || right is null)
            return null;

        switch (left)
        {
            case (double or int) when right is (double or int):
                return HandleNumbersOperators(left, right, binary, context);
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

    private object? HandleNumbersOperators(dynamic left, dynamic right, BinaryExpression binary, Context context)
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
                _errorsBag.ReportRunTimeError("The base cannot be zero", context, binary.Right.Start,
                    binary.Right.End);
                return null;
            case TokenKind.DivisionToken:
                if (right != 0)
                    return left / right is double d2 ? Math.Round(d2, 8) : left / right;
                _errorsBag.ReportRunTimeError("Division by zero is not allowed", context, binary.Right.Start,
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