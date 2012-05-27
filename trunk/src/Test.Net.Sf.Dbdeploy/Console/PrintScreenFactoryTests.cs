using System.IO;
using NUnit.Framework;

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
            parsedArguments.DoScriptFile = "c:\\temp\\foo.sql";

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
            parsedArguments.UndoScriptFile = "c:\\temp\\foo2.sql";

            TextWriter results = printScreenFactory.GetUndoPrintStream(parsedArguments);

            Assert.That(results, Is.TypeOf<StreamWriter>());

            printScreenFactory.ClosePrintStream(results);
        }

    }
}