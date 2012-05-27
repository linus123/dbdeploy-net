using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Net.Sf.Dbdeploy.Exceptions;
using NUnit.Framework;

namespace Net.Sf.Dbdeploy.Database
{
    public class MsSqlDatabaseSchemaVersionManagerTest : AbstractDatabaseSchemaVersionManagerTest
    {
        private static readonly string CONNECTION_STRING = ConfigurationManager.AppSettings["ConnString"];
        private const string DELTA_SET = "All";
		private const string CHANGELOG_TABLE_DOES_NOT_EXIST_MESSAGE = "Could not retrieve change log from database because: Invalid object name 'changelog'.";
		private const string DBMS = "mssql";

        protected override string ConnectionString
        {
            get { return CONNECTION_STRING; }
        }

        protected override string DeltaSet
        {
            get { return DELTA_SET; }
        }

        protected override string ChangelogTableDoesNotExistMessage
        {
            get { return CHANGELOG_TABLE_DOES_NOT_EXIST_MESSAGE; }
        }

    	protected override string Dbms
    	{
			get { return DBMS; }
    	}

    	protected override void EnsureTableDoesNotExist()
        {
            ExecuteSql(string.Format(
				"IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[{0}]') AND type in (N'U')) DROP TABLE [{0}]", databaseSchemaVersion.TableName));
        }

    	protected override IDbConnection GetConnection()
    	{
			return new SqlConnection(CONNECTION_STRING);
    	}

    	protected override void CreateTable()
        {
            ExecuteSql(
				"CREATE TABLE " + databaseSchemaVersion.TableName + "( " +
                "change_number BIGINT NOT NULL, " +
                "delta_set VARCHAR(10) NOT NULL, " +
                "start_dt DATETIME NOT NULL, " +
                "complete_dt DATETIME NULL, " +
                "applied_by VARCHAR(100) NOT NULL, " +
                "description VARCHAR(500) NOT NULL )");
            ExecuteSql(
				"ALTER TABLE " + databaseSchemaVersion.TableName +
                " ADD CONSTRAINT Pkchangelog  PRIMARY KEY (change_number, delta_set)");
        }

        protected override void InsertRowIntoTable(int i)
        {
			ExecuteSql("INSERT INTO " + databaseSchemaVersion.TableName
                       + " (change_number, delta_set, start_dt, complete_dt, applied_by, description) VALUES ( "
                       + i + ", '" + DELTA_SET
                       + "', getdate(), getdate(), user_name(), 'Unit test')");
        }

    	[Test]
		[ExpectedException(typeof(DbDeployException))]
    	public void ShouldThrowExceptionIfIncompletedScriptIsFound()
    	{
    		EnsureTableDoesNotExist();
    		CreateTable();
			ExecuteSql("INSERT INTO " + databaseSchemaVersion.TableName
					   + " (change_number, delta_set, start_dt, complete_dt, applied_by, description) VALUES ( "
					   + 1 + ", '" + DELTA_SET
					   + "', getdate(), NULL, user_name(), 'Unit test')");

    		databaseSchemaVersion.GetAppliedChangeNumbers();
    	}

    	[Test]
    	public void ShouldNotThrowExceptionIfAllPreviousScriptsAreCompleted()
    	{
			EnsureTableDoesNotExist();
			CreateTable();
    		InsertRowIntoTable(3);
			List<Int64> changeNumbers = databaseSchemaVersion.GetAppliedChangeNumbers();

			Assert.AreEqual(1, changeNumbers.Count);
			Assert.AreEqual(3, changeNumbers[0]);
		}

		[Test]
		public void TestCanGenerateVersionCheck()
		{
			databaseSchemaVersion = new DatabaseSchemaVersionManager(new DbmsFactory(DBMS, CONNECTION_STRING), "Main", 5);
			Assert.AreEqual(@"DECLARE @currentDatabaseVersion INTEGER, @errMsg VARCHAR(1000)
SELECT @currentDatabaseVersion = MAX(change_number) FROM changelog WHERE delta_set = 'Main'
IF (@currentDatabaseVersion <> 5)
BEGIN
    SET @errMsg = 'Error: current database version on delta_set <Main> is not 5, but ' + CONVERT(VARCHAR, @currentDatabaseVersion)
    RAISERROR (@errMsg, 16, 1)
END

GO

", databaseSchemaVersion.GenerateVersionCheck());
		}

    	[Test]
        public override void TestCanRetrieveDeltaFragmentFooterSql()
        {
            base.TestCanRetrieveDeltaFragmentFooterSql();
        }

        [Test]
        public override void TestCanRetrieveSchemaVersionFromDatabase()
        {
            base.TestCanRetrieveSchemaVersionFromDatabase();
        }

        [Test]
        public override void TestThrowsWhenDatabaseTableDoesNotExist()
        {
            base.TestThrowsWhenDatabaseTableDoesNotExist();
        }

        [Test]
        public override void TestShouldReturnEmptySetWhenTableHasNoRows()
        {
            base.TestShouldReturnEmptySetWhenTableHasNoRows();
        }

        [Test]
        public override void TestCanRetrieveDeltaFragmentHeaderSql()
        {
            base.TestCanRetrieveDeltaFragmentHeaderSql();
        }

		[Test]
		public override void TestCanSetChangeLogTableName()
		{
			base.TestCanSetChangeLogTableName();
		}
    }
}