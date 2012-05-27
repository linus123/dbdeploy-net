using NUnit.Framework;

namespace Net.Sf.Dbdeploy.Console
{
    [TestFixture]
    public class CommandLineArgumentsParserTests
    {
        private const string SourcePath = "c:\\temp";
        private const string DoScriptFile = "c:\\foo\\bah.sql";
        private const string UndoScriptFile = "c:\\foo\\bah2.sql";

        [Test]
        public void ParseArgsShouldSetScriptFilesPathWhenGivenArgs()
        {
            ParsedArguments result = GetResultsForArgs(
                new[] { SourcePath });

            Assert.That(result.ScriptFilesFolder, Is.EqualTo(SourcePath));
        }

        [Test]
        public void ParseArgsShouldDoScriptFileWhenGivenArgs()
        {
            ParsedArguments result = GetResultsForArgs(
                new[] { SourcePath, DoScriptFile });
            
            Assert.That(result.DoScriptFile, Is.EqualTo(DoScriptFile));
        }

        [Test]
        public void ParseArgsShouldUndoScriptFileWhenGivenArgs()
        {
            ParsedArguments result = GetResultsForArgs(
                new[] { SourcePath, DoScriptFile, UndoScriptFile});

            Assert.That(result.UndoScriptFile, Is.EqualTo(UndoScriptFile));
        }

        private ParsedArguments GetResultsForArgs(
            string[] args)
        {
            var parser = new CommandLineArgumentsParser();

            ParsedArguments result = parser.ParseArgs(args);

            return result;
        }

    }
}