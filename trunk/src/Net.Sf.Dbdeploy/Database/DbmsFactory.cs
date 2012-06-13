using System;
using System.Data;
using System.Reflection;

namespace Net.Sf.Dbdeploy.Database
{
    public class DbmsFactory
    {
        private readonly string dbms;
        private readonly string connectionString;
        private readonly Providers providers;

        public DbmsFactory(string dbms, string connectionString)
        {
            this.dbms = dbms;
            this.connectionString = connectionString;
            providers = new DbProviderFile().LoadProviders();
        }

        public IDbmsSyntax CreateDbmsSyntax(
            string changeOwner)
        {
            switch (dbms)
            {
                case "ora":
                    return new OracleDbmsSyntax(changeOwner);
                case "mssql":
                    return new MsSqlDbmsSyntax(changeOwner);
                case "mysql":
                    return new MySqlDbmsSyntax(changeOwner);
                default:
                    throw new ArgumentException("Supported dbms: ora, mssql, mysql");
            }
        }

        public IDbConnection CreateConnection()
        {
            DatabaseProvider provider = providers.GetProvider(dbms);
            if (provider == null)
				throw new ArgumentException("Supported dbms: ora, mssql, mysql.");

            Assembly assembly = Assembly.Load(provider.AssemblyName);
            Type type = assembly.GetType(provider.ConnectionClass);
            return (IDbConnection) Activator.CreateInstance(type, connectionString);
        }
    }
}