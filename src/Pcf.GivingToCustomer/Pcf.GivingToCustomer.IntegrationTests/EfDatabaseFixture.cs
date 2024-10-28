

using Pcf.GivingtoCustomer.IntegrationTests.Data;

namespace Pcf.GivingtoCustomer.IntegrationTests
{
	public class EfDatabaseFixture : IDisposable
	{
		private readonly EfTestDbInitializer _efTestDbInitializer;

		public EfDatabaseFixture()
		{
			SQLitePCL.Batteries.Init();
			DbContext = new TestDataContext();

			_efTestDbInitializer = new EfTestDbInitializer(DbContext);
			_efTestDbInitializer.InitializeDb();
		}

		public void Dispose()
		{
			_efTestDbInitializer.CleanDb();
		}

		public TestDataContext DbContext { get; private set; }
	}
}
