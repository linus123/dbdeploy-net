using NUnit.Framework;
using Net.Sf.Dbdeploy.Configuration;

namespace Net.Sf.Dbdeploy.Console
{
    [TestFixture]
    public class ParsedArgumentsTests
    {
        [Test]
        public void GetScriptsFilesFolderShouldReturnScriptFolderWhenScriptFolderPopulated()
        {
            var parsedArguments = new ParsedArguments();
            parsedArguments.SetValue(CommandlineSwitchType.ScriptFiles, "c:\\foo");

            var r = parsedArguments.GetScriptFilesFolderOrDefaultFolder();

            Assert.That(r, Is.EqualTo("c:\\foo"));
        }

        [Test]
        public void GetScriptsFilesFolderShouldReturnDefaultWhenScriptFilesFolderIsNotSet()
        {
            var parsedArguments = new ParsedArguments();

            var r = parsedArguments.GetScriptFilesFolderOrDefaultFolder();

            Assert.That(r, Is.EqualTo(ParsedArguments.ScriptFilesFolderDefault));
        }

        [Test]
        public void HasDoScriptFileShouldReturnTrueWhenDoScriptFilePopulated()
        {
            var parsedArguments = new ParsedArguments();

            parsedArguments.SetValue(CommandlineSwitchType.DoFile, "sdf");

            Assert.That(parsedArguments.HasValue(CommandlineSwitchType.DoFile), Is.True);
        }

        [Test]
        public void HasDoScriptFileShouldReturnFalseWhenDoScriptFileNotPopulated()
        {
            var parsedArguments = new ParsedArguments();

            Assert.That(parsedArguments.HasValue(CommandlineSwitchType.DoFile), Is.False);
        }

        [Test]
        public void HasUndoScriptFileShouldReturnTrueWhenUndoScriptFilePopulated()
        {
            var parsedArguments = new ParsedArguments();

            parsedArguments.SetValue(CommandlineSwitchType.UndoFile, "sdf");

            Assert.That(parsedArguments.HasValue(CommandlineSwitchType.UndoFile), Is.True);
        }

        [Test]
        public void HasUndoScriptFileShouldReturnFalseWhenUndoScriptFileNotPopulated()
        {
            var parsedArguments = new ParsedArguments();

            Assert.That(parsedArguments.HasValue(CommandlineSwitchType.UndoFile), Is.False);
        }
    }
}