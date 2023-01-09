namespace Hada.Core.LexicalAnalysis;

public enum TokenKind
{
    IntegerToken,
    DoubleToken,
    PlusToken,
    MinusToken,
    MultiplicationToken,
    DivisionToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    WhiteSpaceToken,
    EndOfFileToken,
    BadToken
}