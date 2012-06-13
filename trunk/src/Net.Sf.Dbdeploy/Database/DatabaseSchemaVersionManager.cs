using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Net.Sf.Dbdeploy.Exceptions;
using Net.Sf.Dbdeploy.Scripts;

namespace Net.Sf.Dbdeploy.Database
{
    public class DatabaseSchemaVersionManager
    {
        public static readonly string DEFAULT_TABLE_NAME = "changelog";
        public static readonly string DEFAULT_CHANGE_OWNER = "none specified";

        private readonly DbmsFactory _factory;
        private readonly string _deltaSet;
        private readonly Int64? _currentVersion;
    	private readonly string _tableName;
        private string _changeOwner;

        public DatabaseSchemaVersionManager(DbmsFactory factory, string deltaSet, int? currentVersion)
            : this(factory, deltaSet, currentVersion, DEFAULT_TABLE_NAME, DEFAULT_CHANGE_OWNER)
        {        	
        }

        public DatabaseSchemaVersionManager(DbmsFactory factory, string deltaSet, Int64? currentVersion, string tableName, string changeOwner)
        {
            _factory = factory;
            _deltaSet = deltaSet;
            _currentVersion = currentVersion;
        	_tableName = tableName;
            _changeOwner = changeOwner;
        }

        private IDbmsSyntax DbmsSyntax
        {
            get { return _factory.CreateDbmsSyntax(_changeOwner); }
        }

        private IDbConnection Connection
        {
            get { return _factory.CreateConnection(); }
        }
    	public string TableName
    	{
    		get { return _tableName; }
    	}

    	public List<Int64> GetAppliedChangeNumbers()
        {
            if (_currentVersion == null)
            {
                return GetCurrentVersionFromDb();
            }
    		
			List<Int64> changeNumbers = new List<Int64>();
    		for (Int64 i = 1; i <= _currentVersion.Value; i++)
    		{
    			changeNumbers.Add(i);
    		}
    		return changeNumbers;
        }

        private List<Int64> GetCurrentVersionFromDb()
        {
            List<Int64> changeNumbers = new List<Int64>();
            try
            {
                using (IDbConnection connection = Connection)
                {
                    connection.Open();

					StringBuilder commandBuilder = new StringBuilder();
                    commandBuilder.AppendFormat("SELECT ChangeNumber, CompletedDate FROM {0}", TableName);
                    commandBuilder.AppendFormat(" WHERE Project = '{0}' ORDER BY ChangeNumber", _deltaSet);

                    IDbCommand command = connection.CreateCommand();
					command.CommandText = commandBuilder.ToString();

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                        	Int64 changeNumber = reader.GetInt64(0);

							if (reader.IsDBNull(1))
							{
								string errorMessage = string.Format("Incompleted delta script {0} found from last execution.", changeNumber);
								throw new DbDeployException(errorMessage);
							}

							changeNumbers.Add(changeNumber);
                        }
                    }

                    return changeNumbers;
                }
            }
            catch (DbException e)
            {
                throw new SchemaVersionTrackingException("Could not retrieve change log from database because: "
                                                         + e.Message, e);
            }            
        }

        public string GenerateDoDeltaFragmentHeader(ChangeScript changeScript)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("--------------- Fragment begins: " + changeScript + " ---------------");

			builder.AppendLine("INSERT INTO " + TableName +
                           " (ChangeNumber, Project, StartDate, AppliedBy, FileName)" +
                           " VALUES (" + changeScript.GetId() + ", '" + _deltaSet + "', " +
                           DbmsSyntax.GenerateTimestamp() +
                           ", " + DbmsSyntax.GenerateChangeOwner() + ", '" + changeScript.GetDescription() + "')" +
                           DbmsSyntax.GenerateStatementDelimiter());
            builder.Append(DbmsSyntax.GenerateCommit());
            return builder.ToString();
        }

        public string GenerateDoDeltaFragmentFooter(ChangeScript changeScript)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(DbmsSyntax.GenerateStatementDelimiter());
			builder.AppendLine("UPDATE " + TableName + " SET CompletedDate = "
                           + DbmsSyntax.GenerateTimestamp()
                           + " WHERE ChangeNumber = " + changeScript.GetId()
                           + " AND Project = '" + _deltaSet + "'"
                           + DbmsSyntax.GenerateStatementDelimiter());
            builder.AppendLine(DbmsSyntax.GenerateCommit());
            builder.Append("--------------- Fragment ends: " + changeScript + " ---------------");
            return builder.ToString();
        }

		public string GenerateUndoDeltaFragmentHeader(ChangeScript changeScript)
		{
			return "--------------- Fragment begins: " + changeScript + " ---------------";
		}

        public string GenerateUndoDeltaFragmentFooter(ChangeScript changeScript)
        {
            StringBuilder builder = new StringBuilder();

			builder.AppendLine("DELETE FROM " + TableName
                           + " WHERE ChangeNumber = " + changeScript.GetId()
                           + " AND Project = '" + _deltaSet + "'"
                           + DbmsSyntax.GenerateStatementDelimiter());
            builder.AppendLine(DbmsSyntax.GenerateCommit());
            builder.Append("--------------- Fragment ends: " + changeScript + " ---------------");
            return builder.ToString();
        }

        public string GenerateVersionCheck()
        {
            string versionCheckSql = string.Empty;
            if (_currentVersion.HasValue)
				versionCheckSql = DbmsSyntax.GenerateVersionCheck(TableName, _currentVersion.Value.ToString(), _deltaSet);

			return versionCheckSql;
        }
    }
}
