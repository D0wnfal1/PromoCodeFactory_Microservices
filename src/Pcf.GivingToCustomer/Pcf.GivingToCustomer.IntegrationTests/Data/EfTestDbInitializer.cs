
using Pcf.GivingToCustomer.DataAccess;
using Pcf.GivingToCustomer.DataAccess.Data;

namespace Pcf.GivingtoCustomer.IntegrationTests.Data
{
	public class EfTestDbInitializer : IDbInitializer
	{
		private readonly DataContext _dataContext;

		public EfTestDbInitializer(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public void CleanDb()
		{
			_dataContext.Database.EnsureDeleted();
		}

		public void InitializeDb()
		{
			_dataContext.Database.EnsureDeleted();
			_dataContext.Database.EnsureCreated();

			_dataContext.AddRange(TestDataFactory.Customers);
			_dataContext.AddRange(TestDataFactory.Preferences);
			_dataContext.SaveChanges();
		}
	}
}
