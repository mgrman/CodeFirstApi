using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFirstApi.Generator;

public sealed class IndentedStringBuilder
{
    private readonly StringBuilder _sb;

    private int _indentationLevel;

    public IndentedStringBuilder()
    {
        _sb = new StringBuilder();
    }

    public IDisposable Indent()
    {
        return new EmptyIndentation(this);
    }

    public IDisposable IndentWithCurlyBrackets()
    {
        return new CurlyBracketIndentation(this);
    }

    public void AppendLine(string line)
    {
        _sb.Append(' ', _indentationLevel * 4);
        _sb.AppendLine(line);
    }

    public void AppendLines(IEnumerable<string> lines)
    {
        foreach (var line in lines) AppendLine(line);
    }

    public void AppendLine()
    {
        _sb.AppendLine();
    }

    public override string ToString()
    {
        return _sb.ToString();
    }

    private class EmptyIndentation : IDisposable
    {
        private readonly IndentedStringBuilder _sb;

        public EmptyIndentation(IndentedStringBuilder sb)
        {
            _sb = sb;
            _sb._indentationLevel++;
        }

        public void Dispose()
        {
            _sb._indentationLevel--;
        }
    }

    private class CurlyBracketIndentation : IDisposable
    {
        private readonly IndentedStringBuilder _sb;

        public CurlyBracketIndentation(IndentedStringBuilder sb)
        {
            _sb = sb;
            _sb.AppendLine("{");
            _sb._indentationLevel++;
        }

        public void Dispose()
        {
            _sb._indentationLevel--;
            _sb.AppendLine("}");
        }
    }
}