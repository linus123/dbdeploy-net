using NUnit.Framework;

namespace Net.Sf.Dbdeploy.Console
{
    [TestFixture]
    public class ParsedArgumentsTests
    {
        [Test]
        public void GetScriptsFilesFolderShouldReturnScriptFolderWhenScriptFolderPopulated()
        {
            var parsedArguments = new ParsedArguments();
            parsedArguments.ScriptFilesFolder = "c:\\foo";

            var r = parsedArguments.GetScriptFilesFolderOrDefaultFolder();

            Assert.That(r, Is.EqualTo("c:\\foo"));
        }

        [Test]
        public void GetScriptsFilesFolderShouldReturnDefaultWhenScriptFilesFolderIsNotSet()
        {
            var parsedArguments = new ParsedArguments();
            parsedArguments.ScriptFilesFolder = null;

            var r = parsedArguments.GetScriptFilesFolderOrDefaultFolder();

            Assert.That(r, Is.EqualTo(ParsedArguments.ScriptFilesFolderDefault));
        }

        [Test]
        public void HasDoScriptFileShouldReturnTrueWhenDoScriptFilePopulated()
        {
            var parsedArguments = new ParsedArguments();

            parsedArguments.DoScriptFile = "sdf";

            Assert.That(parsedArguments.HasDoScriptFile, Is.True);
        }

        [Test]
        public void HasDoScriptFileShouldReturnFalseWhenDoScriptFileNotPopulated()
        {
            var parsedArguments = new ParsedArguments();

            Assert.That(parsedArguments.HasDoScriptFile, Is.False);
        }

        [Test]
        public void HasUndoScriptFileShouldReturnTrueWhenUndoScriptFilePopulated()
        {
            var parsedArguments = new ParsedArguments();

            parsedArguments.UndoScriptFile = "sdf";

            Assert.That(parsedArguments.HasUndoScriptFile, Is.True);
        }

        [Test]
        public void HasUndoScriptFileShouldReturnFalseWhenUndoScriptFileNotPopulated()
        {
            var parsedArguments = new ParsedArguments();

            Assert.That(parsedArguments.HasUndoScriptFile, Is.False);
        }
    }
}