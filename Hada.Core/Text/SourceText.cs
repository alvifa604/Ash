namespace Hada.Core.Text;

public class SourceText
{
    public string Text { get; }
    public string FileName { get; }
    public int LineCount { get; }
    private string[] Lines { get; }

    public string this[int index] => Lines[index];

    public SourceText(string text, string fileName)
    {
        Text = text;
        FileName = fileName;
        Lines = SetLines();
        LineCount = Lines.Length;
    }

    private string[] SetLines()
    {
        return Text.Split('\n');
    }
}