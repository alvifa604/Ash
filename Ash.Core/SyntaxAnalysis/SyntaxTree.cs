using Ash.Core.Errors;
using Ash.Core.LexicalAnalysis;
using Ash.Core.SyntaxAnalysis.Expressions;

namespace Ash.Core.SyntaxAnalysis;

public sealed class SyntaxTree
{
    public SyntaxTree(Expression? root, ErrorsBag errorsBag, Token endOfFileToken)
    {
        Root = root;
        ErrorsBag = errorsBag;
        EndOfFileToken = endOfFileToken;
    }

    public Expression? Root { get; }
    public ErrorsBag ErrorsBag { get; }
    public Token EndOfFileToken { get; }

    public void Print()
    {
        Print(Root);
    }

    private static void Print(Expression node, string indent = "")
    {
        switch (node)
        {
            case BinaryExpression b:
                Console.WriteLine($"{indent}{b.Kind} ");
                indent += "  ";
                Print(b.Left, indent);
                Console.WriteLine($"{indent}{b.OperatorToken.Kind}: {b.OperatorToken.Text}");
                Print(b.Right, indent);
                break;
            case UnaryExpression u:
                Console.WriteLine($"{indent}{u.Kind} ");
                indent += "  ";
                Console.WriteLine($"{indent}{u.OperatorToken.Kind}: {u.OperatorToken.Text}");
                Print(u.Expression, indent);
                break;
            case NumberExpression n:
                Console.WriteLine($"{indent}{n.Token.Kind}: {n.Token.Text}");
                break;
            case ParenthesizedExpression p:
                Console.WriteLine($"{indent}{p.Kind}");
                indent += "  ";
                Console.WriteLine($"{indent}{p.OpenParenthesisToken.Kind}: {p.OpenParenthesisToken.Text}");
                Print(p.Expression, indent);
                Console.WriteLine($"{indent}{p.CloseParenthesisToken.Kind}: {p.CloseParenthesisToken.Text}");
                break;
        }
    }
}