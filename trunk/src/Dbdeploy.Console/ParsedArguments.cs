namespace Net.Sf.Dbdeploy
{
    public class ParsedArguments
    {
        public static string ScriptFilesFolderDefault = ".";

        public string ScriptFilesFolder { get; set; }
        public string DoScriptFile { get; set; }
        public string UndoScriptFile { get; set; }

        public bool HasDoScriptFile
        {
            get { return !string.IsNullOrEmpty(DoScriptFile); }
        }

        public bool HasUndoScriptFile
        {
            get { return !string.IsNullOrEmpty(UndoScriptFile); }
        }

        public string GetScriptFilesFolderOrDefaultFolder()
        {
            if (string.IsNullOrEmpty(ScriptFilesFolder))
                return ScriptFilesFolderDefault;

            return ScriptFilesFolder;
        }
    }
}