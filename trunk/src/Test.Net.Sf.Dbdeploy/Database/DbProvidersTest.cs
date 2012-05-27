using NUnit.Framework;

namespace Net.Sf.Dbdeploy.Database
{
    [TestFixture]
    public class DbProvidersTest
    {
        private Providers providers;

        [SetUp]
        public void Setup()
        {
            providers = new DbProviderFile().LoadProviders();
        }
     
        [Test]
        public void TestCanLoadMsSqlProvider()
        {
            Assert.IsNotNull(providers.GetProvider("mssql"));
        }
        [Test]
        public void TestCanLoadOracleProvider()
        {
            Assert.IsNotNull(providers.GetProvider("ora"));
        }

        [Test]
        public void TestCanLoadMySQLProvider()
        {
            Assert.IsNotNull(providers.GetProvider("mysql"));
        }
    }
}