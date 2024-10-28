
using Microsoft.EntityFrameworkCore;
using Pcf.GivingToCustomer.DataAccess;

namespace Pcf.GivingtoCustomer.IntegrationTests
{
	public class TestDataContext : DataContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Filename=PromocodeFactoryGivingToCustomerDb.sqlite");
		}
	}
}
