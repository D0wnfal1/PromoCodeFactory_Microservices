using Microsoft.EntityFrameworkCore;
using Pcf.Administration.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.Administration.IntegrationTests
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
