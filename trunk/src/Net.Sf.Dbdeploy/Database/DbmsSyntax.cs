using System.Text;

namespace Net.Sf.Dbdeploy.Database
{
    public interface IDbmsSyntax
    {
        string GenerateScriptHeader();

        string GenerateTimestamp();

        string GenerateUser();

        string GenerateStatementDelimiter();

        string GenerateCommit();

    	string GenerateBeginTransaction();
    	
		string GenerateCommitTransaction();

        string GenerateChangeOwner();

		string GenerateVersionCheck(string tableName, string currentVersion, string deltaSet);
    }

	public abstract class DbmsSyntax : IDbmsSyntax
	{
	    private string _changeOwner;

	    protected DbmsSyntax(
            string owner)
	    {
	        _changeOwner = owner;
	    }

	    public abstract string GenerateScriptHeader();
		public abstract string GenerateTimestamp();
		public abstract string GenerateUser();
		public abstract string GenerateStatementDelimiter();
		public abstract string GenerateCommit();
		public abstract string GenerateBeginTransaction();
		public abstract string GenerateCommitTransaction();

        public string GenerateChangeOwner()
        {
            return "'" +  _changeOwner + "'";
        }

		public virtual string GenerateVersionCheck(string tableName, string currentVersion, string deltaSet)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("DECLARE @currentDatabaseVersion INTEGER, @errMsg VARCHAR(1000)");
			builder.Append("SELECT @currentDatabaseVersion = MAX(ChangeNumber) FROM ").Append(tableName).AppendLine(" WHERE Project = '" + deltaSet + "'");
			builder.Append("IF (@currentDatabaseVersion <> ").Append(currentVersion).AppendLine(")");
			builder.AppendLine("BEGIN");
			builder.Append("    SET @errMsg = 'Error: current database version on Project <").Append(deltaSet).Append("> is not ").Append(currentVersion).AppendLine(", but ' + CONVERT(VARCHAR, @currentDatabaseVersion)");
			builder.AppendLine("    RAISERROR (@errMsg, 16, 1)");
			builder.AppendLine("END");
			builder.AppendLine(GenerateStatementDelimiter()).AppendLine();
			return builder.ToString();
		}
	}
}