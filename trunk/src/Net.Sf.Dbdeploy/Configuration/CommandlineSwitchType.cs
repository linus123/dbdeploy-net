using System.Linq;
using Headspring;

namespace Net.Sf.Dbdeploy.Configuration
{
    public class CommandlineSwitchType : Enumeration<CommandlineSwitchType>
    {
        public static CommandlineSwitchType DoFile = new CommandlineSwitchType(1, "Do File Path", "dofile");
        public static CommandlineSwitchType UndoFile = new CommandlineSwitchType(2, "Undo File Path", "undofile");
        public static CommandlineSwitchType ScriptFiles = new CommandlineSwitchType(3, "Script Files Folder", "scriptfiles");
        public static CommandlineSwitchType ConnectionString = new CommandlineSwitchType(4, "Database Connection String", "connection");
        public static CommandlineSwitchType DatabaseType = new CommandlineSwitchType(5, "Database Type", "type");
        public static CommandlineSwitchType DeltaSet = new CommandlineSwitchType(6, "Delta Set", "deltaset");
        public static CommandlineSwitchType UseTransaction = new CommandlineSwitchType(7, "Use Transaction", "usetransaction");
        public static CommandlineSwitchType CurrrentDatabaseVersion = new CommandlineSwitchType(8, "Current Database Version", "dbversion");
        public static CommandlineSwitchType TableName = new CommandlineSwitchType(9, "Table Name", "tablename");

        private readonly string _commandLineSwitch;

        public CommandlineSwitchType(
            int value,
            string displayName,
            string commandLineSwitch)
            : base(value, displayName)
        {
            _commandLineSwitch = commandLineSwitch;
        }

        public string CommandLineSwitch
        {
            get { return _commandLineSwitch; }
        }

        public string GetSwitch()
        {
            return "-" + _commandLineSwitch;
        }

        public static CommandlineSwitchType GetTypeForCommandLineSwitch(
            string commandLineSwitch)
        {
            var allTypes = CommandlineSwitchType.GetAll();

            var commandlineSwitchType = allTypes.FirstOrDefault(t => t.GetSwitch() == commandLineSwitch);

            return commandlineSwitchType;
        }
    }
}