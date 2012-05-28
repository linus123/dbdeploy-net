using Net.Sf.Dbdeploy.Configuration;

namespace Net.Sf.Dbdeploy
{
    public class CommandLineArgumentsParser
    {
        public ParsedArguments ParseArgs(string[] args)
        {
            var parsedArguments = GetSwitchDictionary(args);

            return parsedArguments;
        }

        private ParsedArguments GetSwitchDictionary(
            string[] args)
        {
            var result = new ParsedArguments();

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
                    result.SetValue(switchType, switchValue);
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