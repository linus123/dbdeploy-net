namespace Net.Sf.Dbdeploy
{
    public class CommandLineArgumentsParser
    {
        public ParsedArguments ParseArgs(string[] args)
        {
            var parsedArguments = new ParsedArguments();

            parsedArguments.ScriptFilesFolder = args[0];

            if (args.Length >= 2)
                parsedArguments.DoScriptFile = args[1];

            if (args.Length >= 3)
                parsedArguments.UndoScriptFile = args[2];

            return parsedArguments;
        }
    }
}