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
        var result = VisitStatement(_tree.Root!, _context);
        return new InterpreterResult(result, _errorsBag);
    }

    private object? VisitStatement(Node node, Context context)
    {
        return node switch
        {
            DeclarationStatement assignment => VisitDeclaration(assignment, context),
            IfStatement ifStatement => VisitIfStatement(ifStatement, context),
            ElseStatement elseStatement => VisitElseStatement(elseStatement, context),
            ForStatement forStatement => VisitForStatement(forStatement, context),
            WhileStatement whileStatement => VisitWhileStatement(whileStatement, context),
            BreakStatement breakStatement => breakStatement,
            ExpressionStatement expression => VisitExpression(expression.Expression, context),
            BlockStatement block => VisitStatement(block.Body, context),
            FunctionDeclarationStatement functionDeclaration => VisitFunctionDeclaration(functionDeclaration, context),
            _ => VisitExpression(node, context)
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
        var localContext = new Context("If", context);
        var condition = VisitExpression(ifStatement.Condition, context);
        if (condition is not bool booleanCondition)
        {
            _errorsBag.ReportRunTimeError("Condition must be a boolean.", context,
                ifStatement.Condition.Start, ifStatement.Condition.End);
            return null;
        }

        if (booleanCondition)
            return VisitStatement(ifStatement.Body, localContext);
        return ifStatement.ElseStatement != null
            ? VisitStatement(ifStatement.ElseStatement, localContext)
            : "";
    }

    private object? VisitElseStatement(ElseStatement elseStatement, Context context)
    {
        return VisitStatement(elseStatement.Body, context);
    }

    private object? VisitForStatement(ForStatement forStatement, Context context)
    {
        var lowerBoundVariable = forStatement.LowerBoundDeclaration.IdentifierToken.Text;
        var lowerBound = VisitDeclaration(forStatement.LowerBoundDeclaration, context);
        if (lowerBound is not int lower)
        {
            _errorsBag.ReportRunTimeError("Lower bound must be an integer", context,
                forStatement.LowerBoundDeclaration.Start, forStatement.LowerBoundDeclaration.End);
            return null;
        }

        var upperBound = VisitExpression(forStatement.UpperBound, context);
        if (upperBound is not int upper)
        {
            _errorsBag.ReportRunTimeError("Upper bound must be an integer", context,
                forStatement.UpperBound.Start, forStatement.UpperBound.End);
            return null;
        }

        if (forStatement.Step is null)
        {
            if (lower > upper)
                for (var i = lower; i > upper; i--)
                {
                    context.Symbols[lowerBoundVariable] = i;
                    var result = VisitStatement(forStatement.Body, context);
                    if (result is null) return null;
                    if (result is BreakStatement) break;
                }
            else
                for (var i = lower; i < upper; i++)
                {
                    context.Symbols[lowerBoundVariable] = i;
                    var result = VisitStatement(forStatement.Body, context);
                    if (result is null) return null;
                    if (result is BreakStatement) break;
                }
        }
        else
        {
            var step = VisitExpression(forStatement.Step, context);
            if (step is not int stepValue)
            {
                _errorsBag.ReportRunTimeError("Step must be an integer", context,
                    forStatement.Step.Start, forStatement.Step.End);
                return null;
            }

            if (stepValue < 1)
            {
                _errorsBag.ReportRunTimeError("Step cannot be less than 1", context,
                    forStatement.Step.Start, forStatement.Step.End);
                return null;
            }

            if (lower > upper)
                for (var i = lower; i > upper; i -= stepValue)
                {
                    context.Symbols[lowerBoundVariable] = i;
                    var result = VisitStatement(forStatement.Body, context);
                    Console.WriteLine(result);
                    if (result is null) return null;
                    if (result is BreakStatement) break;
                }
            else
                for (var i = lower; i < upper; i += stepValue)
                {
                    context.Symbols[lowerBoundVariable] = i;
                    var result = VisitStatement(forStatement.Body, context);
                    Console.WriteLine(result);
                    if (result is null) return null;
                    if (result is BreakStatement) break;
                }
        }

        context.Symbols.Remove(lowerBoundVariable);
        return "";
    }


    private object? VisitWhileStatement(WhileStatement whileStatement, Context context)
    {
        //var localContext = new Context("while", context);
        var condition = VisitExpression(whileStatement.Condition, context);
        if (condition is not bool)
        {
            _errorsBag.ReportRunTimeError("Condition must be a boolean.", context,
                whileStatement.Condition.Start, whileStatement.Condition.End);
            return null;
        }

        while ((bool)condition!)
        {
            var res = VisitStatement(whileStatement.Body, context);
            if (res is null) return null;
            if (res is BreakStatement) break;
            condition = VisitExpression(whileStatement.Condition, context);
        }

        return "";
    }

    private object? VisitFunctionDeclaration(FunctionDeclarationStatement functionDeclaration, Context context)
    {
        var functionName = functionDeclaration.IdentifierToken.Text;
        var functionExists = context.Symbols[functionName] != null;
        if (functionExists)
        {
            _errorsBag.ReportRunTimeError($"Function '{functionName}' is already defined", context,
                functionDeclaration.Start, functionDeclaration.End);
            return null;
        }

        context.Symbols[functionName] = functionDeclaration;
        return "";
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
            FunctionCallExpression function => VisitFunctionCall(function, context),
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
        var existingVar = context.Symbols[variableName];
        if (existingVar != null)
            return existingVar;

        _errorsBag.ReportRunTimeError($"Variable '{variableName}' is not defined.", context, variable.Start,
            variable.End);
        return null;
    }

    private object? VisitLiteral(LiteralExpression literal, Context context)
    {
        return literal.Value;
    }

    private object? VisitFunctionCall(FunctionCallExpression function, Context context)
    {
        var localContext = new Context("function", context);
        var functionName = function.IdentifierToken.Text;
        var functionExists = localContext.Symbols[functionName] != null;
        if (!functionExists)
        {
            _errorsBag.ReportRunTimeError($"Function '{functionName}' is not defined.", localContext, function.Start,
                function.End);
            return null;
        }

        var functionDeclaration = (FunctionDeclarationStatement)localContext.Symbols[functionName]!;
        var parameters = functionDeclaration.ParametersList;
        var arguments = function.Arguments;
        if (parameters.Count != arguments.Count)
        {
            _errorsBag.ReportRunTimeError(
                $"'{functionName}' expects {parameters.Count} arguments, but {arguments.Count} were provided", localContext,
                function.Start,
                function.End);
            return null;
        }

        for (var i = 0; i < parameters.Count; i++)
        {
            var parameter = parameters[i];
            var argumentExpression = arguments[i];
            var argumentValue = VisitExpression(argumentExpression.Argument, localContext);

            if (argumentValue is null) return null;
            var argumentType = argumentValue switch
            {
                int => "integer",
                double => "double",
                bool => "boolean",
                _ => "unknown"
            };

            switch (parameter.Type.Kind)
            {
                case TokenKind.IntegerKeyword when argumentValue is int avi:
                    localContext.Symbols[parameter.IdentifierToken.Text] = avi;
                    break;
                case TokenKind.DoubleKeyword when argumentValue is double avd:
                    localContext.Symbols[parameter.IdentifierToken.Text] = avd;
                    break;
                case TokenKind.BooleanKeyword when argumentValue is bool avb:
                    localContext.Symbols[parameter.IdentifierToken.Text] = avb;
                    break;
                default:
                    _errorsBag.ReportRunTimeError(
                        $"Argument {i + 1} of '{functionName}' is of type {argumentType}, but {parameter.Type.Kind.GetText()} was expected",
                        localContext, function.Start,
                        function.End);
                    return null;
            }
        }

        var result = VisitStatement(functionDeclaration.Body, localContext);

        foreach (var parameter in functionDeclaration.ParametersList)
            localContext.Symbols.Remove(parameter.IdentifierToken.Text);

        return result;
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