using System.Collections.Generic;
using Net.Sf.Dbdeploy.Configuration;

namespace Net.Sf.Dbdeploy
{
    public static class SwitchDictionaryExtensions
    {
        public static bool HasSwitchValue(
            this Dictionary<CommandlineSwitchType, string> switches,
            CommandlineSwitchType commandlineSwitchType)
        {
            return switches.ContainsKey(commandlineSwitchType);
        }

        public static string GetSwitchValue(
             this Dictionary<CommandlineSwitchType, string> switches,
             CommandlineSwitchType commandlineSwitchType)
         {
             return switches[commandlineSwitchType];
         }
    }
}