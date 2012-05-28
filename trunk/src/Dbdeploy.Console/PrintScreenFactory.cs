using System;
using System.IO;
using Net.Sf.Dbdeploy.Configuration;

namespace Net.Sf.Dbdeploy
{
    public class PrintScreenFactory
    {
        public TextWriter GetDoPrintStream(
            ParsedArguments p)
        {
            if (p.HasValue(CommandlineSwitchType.DoFile))
                return new StreamWriter(p.GetValue(CommandlineSwitchType.DoFile));

            return Console.Out;
        }

        public TextWriter GetUndoPrintStream(
            ParsedArguments p)
        {
            if (p.HasValue(CommandlineSwitchType.UndoFile))
                return new StreamWriter(p.GetValue(CommandlineSwitchType.UndoFile));

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