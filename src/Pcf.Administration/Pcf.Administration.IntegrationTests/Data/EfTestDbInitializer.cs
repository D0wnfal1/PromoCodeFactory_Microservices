using Pcf.Administration.DataAccess.Data;
using Pcf.Administration.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.Administration.IntegrationTests.Data
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

		public void Initialize()
		{
			_dataContext.Database.EnsureDeleted();
			_dataContext.Database.EnsureCreated();

			_dataContext.AddRange(TestDataFactory.Employees);
			_dataContext.SaveChanges();
		}
	}
}
