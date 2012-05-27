using System;
using System.Collections.Generic;
using System.Data;
using Net.Sf.Dbdeploy.Exceptions;
using Net.Sf.Dbdeploy.Scripts;
using NUnit.Framework;

namespace Net.Sf.Dbdeploy.Database
{
    [TestFixture]
    public abstract class AbstractDatabaseSchemaVersionManagerTest
    {
        protected DatabaseSchemaVersionManager databaseSchemaVersion;

        [SetUp]
        protected void SetUp()
        {
			databaseSchemaVersion = new DatabaseSchemaVersionManager(new DbmsFactory(Dbms, ConnectionString), DeltaSet, null);
        }

        public virtual void TestCanRetrieveSchemaVersionFromDatabase()
        {
            EnsureTableDoesNotExist();
            CreateTable();
            InsertRowIntoTable(5);

            List<Int64> appliedChangeNumbers = databaseSchemaVersion.GetAppliedChangeNumbers();
            Assert.AreEqual(1, appliedChangeNumbers.Count);
            Assert.Contains(Int64.Parse("5"), appliedChangeNumbers);
        }

        public virtual void TestThrowsWhenDatabaseTableDoesNotExist()
        {
            EnsureTableDoesNotExist();

            try
            {
                databaseSchemaVersion.GetAppliedChangeNumbers();
                Assert.Fail("expected exception");
            }
            catch (SchemaVersionTrackingException ex)
            {
                Assert.AreEqual(ChangelogTableDoesNotExistMessage, ex.Message);
            }
        }

        public virtual void TestShouldReturnEmptySetWhenTableHasNoRows()
        {
            EnsureTableDoesNotExist();
            CreateTable();

            Assert.AreEqual(0, databaseSchemaVersion.GetAppliedChangeNumbers().Count);
        }

        public virtual void TestCanRetrieveDeltaFragmentHeaderSql()
        {
            ChangeScript script = new ChangeScript(3, "description");
            Assert.AreEqual(@"--------------- Fragment begins: #3 ---------------
INSERT INTO changelog (change_number, delta_set, start_dt, applied_by, description) VALUES (3, 'All', getdate(), user_name(), 'description')
GO
",
                databaseSchemaVersion.GenerateDoDeltaFragmentHeader(script));
        }

        public virtual void TestCanRetrieveDeltaFragmentFooterSql()
        {
            ChangeScript script = new ChangeScript(3, "description");
            Assert.AreEqual(
                @"
GO
UPDATE changelog SET complete_dt = getdate() WHERE change_number = 3 AND delta_set = 'All'
GO

--------------- Fragment ends: #3 ---------------",
                databaseSchemaVersion.GenerateDoDeltaFragmentFooter(script));
        }

		public virtual void TestCanRetrieveUndoDeltaFragmentHeaderSql()
		{
			ChangeScript script = new ChangeScript(3, "description");
			Assert.AreEqual(@"--------------- Fragment begins: #3 ---------------", databaseSchemaVersion.GenerateUndoDeltaFragmentHeader(script));
		}

		public virtual void TestCanRetrieveUndoDeltaFragmentFooterSql()
		{
			ChangeScript script = new ChangeScript(3, "description");
			Assert.AreEqual(
				@"DELETE FROM changelog WHERE change_number = 3 AND delta_set = 'All'
GO

--------------- Fragment ends: #3 ---------------",
				databaseSchemaVersion.GenerateUndoDeltaFragmentFooter(script));
		}

		public virtual void TestCanSetChangeLogTableName()
		{
			const string expectedTableName = "FooTable";
			DatabaseSchemaVersionManager databaseSchemaVersionManager = new DatabaseSchemaVersionManager(new DbmsFactory(Dbms, ConnectionString), DeltaSet, null, expectedTableName);
			Assert.AreEqual(expectedTableName, databaseSchemaVersionManager.TableName);
		}

        protected virtual void EnsureTableDoesNotExist()
        {
			ExecuteSql("DROP TABLE " + databaseSchemaVersion.TableName);
        }

        protected void ExecuteSql(String sql)
        {
			using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }

        protected abstract string ConnectionString { get; }
        protected abstract string DeltaSet { get; }
        protected abstract string ChangelogTableDoesNotExistMessage { get; }
		protected abstract string Dbms { get; }
		protected abstract IDbConnection GetConnection();
        protected abstract void CreateTable();
        protected abstract void InsertRowIntoTable(int i);
    }
}