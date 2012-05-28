using NUnit.Framework;
using Net.Sf.Dbdeploy.Configuration;

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
            var args = new[]
                           {
                               CommandlineSwitchType.ScriptFiles.GetSwitch()
                               , SourcePath
                           };

            ParsedArguments result = GetResultsForArgs(
                args);

            Assert.That(result.ScriptFilesFolder, Is.EqualTo(SourcePath));
        }

        [Test]
        public void ParseArgsShouldDoScriptFileWhenGivenArgs()
        {
            var args = new[]
                           {
                               CommandlineSwitchType.ScriptFiles.GetSwitch()
                               , SourcePath
                                , CommandlineSwitchType.DoFile.GetSwitch()
                                , DoScriptFile
                           };

            ParsedArguments result = GetResultsForArgs(
                args);
            
            Assert.That(result.DoScriptFile, Is.EqualTo(DoScriptFile));
        }

        [Test]
        public void ParseArgsShouldUndoScriptFileWhenGivenArgs()
        {
            ParsedArguments result = GetResultsForArgs(
                new[]
                    {
                        CommandlineSwitchType.ScriptFiles.GetSwitch()
                        , SourcePath
                        , CommandlineSwitchType.DoFile.GetSwitch()
                        , DoScriptFile
                        , CommandlineSwitchType.UndoFile.GetSwitch()
                        , UndoScriptFile
                    });

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