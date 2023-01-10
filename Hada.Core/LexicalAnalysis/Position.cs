namespace Hada.Core.LexicalAnalysis;

public class Position
{
    public int Line { get; private set; }
    public int Column { get; private set; }
    public int Index { get; private set; }
    public string FileName { get; }
    public string Source { get; }

    public Position(int line, int column, int index, string fileName, string source)
    {
        Line = line;
        Column = column;
        Index = index;
        FileName = fileName;
        Source = source;
    }

    public Position Advance(char current)
    {
        Index++;
        Column++;

        if (current is '\n')
        {
            Line++;
            Column = 0;
        }

        return this;
    }

    public Position Clone()
    {
        return new Position(Line, Column, Index, FileName, Source);
    }
}