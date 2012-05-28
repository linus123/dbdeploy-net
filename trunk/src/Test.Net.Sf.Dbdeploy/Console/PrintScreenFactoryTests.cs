using System.IO;
using NUnit.Framework;
using Net.Sf.Dbdeploy.Configuration;

namespace Net.Sf.Dbdeploy.Console
{
    [TestFixture]
    public class PrintScreenFactoryTests
    {
        [Test]
        public void GetDoPrintStreamShouldReturnConsoleWhenNotGivenFinalScriptFile()
        {
            var printScreenFactory = new PrintScreenFactory();

            var parsedArguments = new ParsedArguments();

            TextWriter results = printScreenFactory.GetDoPrintStream(parsedArguments);

            Assert.That(results, Is.EqualTo(System.Console.Out));

            printScreenFactory.ClosePrintStream(results);
        }

        [Test]
        [Explicit]
        public void GetDoPrintStreamShouldReturnFileWriterWhenGivenFinalScriptFile()
        {
            var printScreenFactory = new PrintScreenFactory();

            var parsedArguments = new ParsedArguments();
            parsedArguments.SetValue(CommandlineSwitchType.DoFile, "c:\\temp\\foo.sql");

            TextWriter results = printScreenFactory.GetDoPrintStream(parsedArguments);

            Assert.That(results, Is.TypeOf<StreamWriter>());

            printScreenFactory.ClosePrintStream(results);
        }

        [Test]
        [Explicit]
        public void GetUndoPrintStreamShouldReturnFileWriterWhenGivenFinalScriptFile()
        {
            var printScreenFactory = new PrintScreenFactory();

            var parsedArguments = new ParsedArguments();
            parsedArguments.SetValue(CommandlineSwitchType.UndoFile, "c:\\temp\\foo2.sql");

            TextWriter results = printScreenFactory.GetUndoPrintStream(parsedArguments);

            Assert.That(results, Is.TypeOf<StreamWriter>());

            printScreenFactory.ClosePrintStream(results);
        }

    }
}