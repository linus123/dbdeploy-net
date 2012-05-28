using System.Collections.Generic;

namespace Net.Sf.Dbdeploy.Configuration
{
    public class ParsedArguments
    {
        public static string ScriptFilesFolderDefault = ".";
        private readonly Dictionary<CommandlineSwitchType, string> _switchDictionary;

        public ParsedArguments()
        {
            _switchDictionary = new Dictionary<CommandlineSwitchType, string>();
        }

        public void SetValue(
            CommandlineSwitchType c,
            string v)
        {
            _switchDictionary.Add(c, v);
        }

        public bool HasValue(
            CommandlineSwitchType c)
        {
            return _switchDictionary.ContainsKey(c);
        }
        
        public string GetValue(
            CommandlineSwitchType c)
        {
            return _switchDictionary[c];
        }

        public string GetScriptFilesFolderOrDefaultFolder()
        {
            if (HasValue(CommandlineSwitchType.ScriptFiles))
                return GetValue(CommandlineSwitchType.ScriptFiles);

            return ScriptFilesFolderDefault;
        }
    }
}