using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Pcf.Administration.Core.Domain.Administration;
using Pcf.Administration.DataAccess.Repositories;
using Pcf.Administration.WebHost.Controllers;

namespace Pcf.Administration.IntegrationTests.Components.WebHost.Controllers
{
	[Collection(EfDatabaseCollection.DbCollection)]
	public class EmployeesControllerTests
	{
		private EfRepository<Employee> _employeeRepository;
		private EmployeesController _employeesController;
        public EmployeesControllerTests(EfDatabaseFixture efDatabaseFixture, IDistributedCache cache)
        {
			_employeeRepository = new EfRepository<Employee>(efDatabaseFixture.DbContext);
			_employeesController = new EmployeesController(_employeeRepository, cache);
        }

		[Fact]
		public async Task GetEmployeeByIdAsync_ExistedEmployee_ExistedId()
		{
			//Arrange
			var expectedEmployeeId = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f");

			//Act
			var result = await _employeesController.GetEmployeeByIdAsync(expectedEmployeeId);

			//Assert
			result.Value.Id.Should().Be(expectedEmployeeId);
		}
    }
}
