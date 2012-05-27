using System;
using System.IO;

namespace Net.Sf.Dbdeploy
{
    public class PrintScreenFactory
    {
        public TextWriter GetDoPrintStream(
            ParsedArguments parsedArguments)
        {
            if (parsedArguments.HasDoScriptFile)
                return new StreamWriter(parsedArguments.DoScriptFile);

            return Console.Out;
        }

        public TextWriter GetUndoPrintStream(ParsedArguments parsedArguments)
        {
            if (parsedArguments.HasUndoScriptFile)
                return new StreamWriter(parsedArguments.UndoScriptFile);

            return Console.Out;
        }

        public void ClosePrintStream(
            TextWriter printStream)
        {
            if (printStream.GetType().ToString() == typeof(StreamWriter).ToString())
            {
                var streamWriter = (StreamWriter) printStream;
                streamWriter.Close();
            }
        }
    }
}