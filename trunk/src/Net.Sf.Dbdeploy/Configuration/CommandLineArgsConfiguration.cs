namespace Net.Sf.Dbdeploy.Configuration
{
    public class CommandLineArgsConfiguration : IConfiguration
    {
        private readonly ParsedArguments _parsedArguments;

        public CommandLineArgsConfiguration(
            ParsedArguments parsedArguments)
        {
            _parsedArguments = parsedArguments;
        }

        public string DbConnectionString
        {
            get { return GetArgValue(CommandlineSwitchType.ConnectionString); }
        }

        public string DbType
        {
            get { return GetArgValue(CommandlineSwitchType.DatabaseType); }
        }

        public string DbDeltaSet
        {
            get { return GetArgValue(CommandlineSwitchType.DeltaSet); }
        }

        public bool UseTransaction
        {
            get
            {
                if (_parsedArguments.HasValue(CommandlineSwitchType.UseTransaction))
                    return bool.Parse(_parsedArguments.GetValue(CommandlineSwitchType.UseTransaction));

                return false;
            }
        }

        public int? CurrentDbVersion
        {
            get
            {
                if (_parsedArguments.HasValue(CommandlineSwitchType.CurrrentDatabaseVersion))
                    return int.Parse(_parsedArguments.GetValue(CommandlineSwitchType.CurrrentDatabaseVersion));

                return null;
            }
        }

        public string TableName
        {
            get { return GetArgValue(CommandlineSwitchType.TableName); }
        }

        private string GetArgValue(CommandlineSwitchType t)
        {
            if (_parsedArguments.HasValue(t))
                return _parsedArguments.GetValue(t);

            return string.Empty;
        }
    }
}