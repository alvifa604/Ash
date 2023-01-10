using System.Collections;
using System.Text;
using Hada.Core.LexicalAnalysis;
using Hada.Core.Text;

namespace Hada.Core.Errors;

public sealed class ErrorsBag : IEnumerable<Error>
{
    private readonly SourceText _source;
    private readonly List<Error> _errors = new();
    public int Count => _errors.Count;
    public Error this[int index] => _errors[index];

    public ErrorsBag(SourceText source)
    {
        _source = source;
    }

    public void AddRange(ErrorsBag bag)
    {
        _errors.AddRange(bag._errors);
    }

    // Lexer errors
    public void ReportIllegalCharacterError(char @char, Position start, Position end)
    {
        var error = new IllegalCharacterError(@char, start, end);
        _errors.Add(error);
    }

    public void ReportInvalidNumberError(string details, Position start, Position end)
    {
        var error = new InvalidNumberError(details, start, end);
        _errors.Add(error);
    }

    public void WriteErrors()
    {
        var fileName = $"/Users/alberto/Desktop/C#/Hada/Samples/{_source.FileName}-Errors.txt";
        var file = File.Create(fileName);

        var currLine = 0;
        var errorIndex = 0;
        var line = new StringBuilder();

        while (currLine < _source.LineCount)
        {
            var text = _source[currLine];
            var lineNumber = currLine + 1;

            line.AppendLine($"{lineNumber}: {text}");

            if (errorIndex < Count)
            {
                var error = this[errorIndex];
                while (error.PosStart.Line == lineNumber)
                {
                    line.AppendLine($"{error}");
                    errorIndex++;
                    if (errorIndex >= Count)
                        break;
                    error = this[errorIndex];
                }

                line.AppendLine();
            }

            currLine++;
        }

        using var stream = new StreamWriter(file);
        stream.WriteLine(line);
    }

    public IEnumerator<Error> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}