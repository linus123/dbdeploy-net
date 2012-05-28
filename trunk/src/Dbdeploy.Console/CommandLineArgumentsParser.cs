using Net.Sf.Dbdeploy.Configuration;
using System.Collections.Generic;

namespace Net.Sf.Dbdeploy
{
    public class CommandLineArgumentsParser
    {
        public ParsedArguments ParseArgs(string[] args)
        {
            var switches = GetSwitchDictionary(args);

            var parsedArguments = new ParsedArguments();

            if (switches.HasSwitchValue(CommandlineSwitchType.ScriptFiles))
                parsedArguments.ScriptFilesFolder = switches.GetSwitchValue(CommandlineSwitchType.ScriptFiles);

            if (switches.HasSwitchValue(CommandlineSwitchType.DoFile))
                parsedArguments.DoScriptFile = switches.GetSwitchValue(CommandlineSwitchType.DoFile);

            if (switches.HasSwitchValue(CommandlineSwitchType.UndoFile))
                parsedArguments.UndoScriptFile = switches.GetSwitchValue(CommandlineSwitchType.UndoFile);

            return parsedArguments;
        }

        private Dictionary<CommandlineSwitchType, string> GetSwitchDictionary(
            string[] args)
        {
            var result = new Dictionary<CommandlineSwitchType, string>();

            int indexOfSwitch = 0;

            while (indexOfSwitch < args.Length)
            {
                var switchArg = args[indexOfSwitch];
                var switchType = CommandlineSwitchType.GetTypeForCommandLineSwitch(switchArg);

                var indexOfSwitchValue = indexOfSwitch + 1;

                if (WeHaveMatchingCommandLineSwitch(switchType)
                    && ArgsIsLargeEnoughtToSupportSwitchValue(args, indexOfSwitchValue))

                {
                    var switchValue = args[indexOfSwitchValue];
                    result.Add(switchType, switchValue);
                }

                indexOfSwitch += 2;
            }

            return result;
        }

        private bool ArgsIsLargeEnoughtToSupportSwitchValue(
            string[] args, int i)
        {
            return args.Length > i;
        }

        private bool WeHaveMatchingCommandLineSwitch(
            CommandlineSwitchType c)
        {
            return c != null;
        }
    }
}