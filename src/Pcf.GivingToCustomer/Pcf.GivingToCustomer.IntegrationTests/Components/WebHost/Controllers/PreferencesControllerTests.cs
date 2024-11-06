using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Pcf.GivingtoCustomer.IntegrationTests;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.DataAccess.Repositories;
using Pcf.GivingToCustomer.WebHost.Controllers;
using Pcf.GivingToCustomer.WebHost.Models;
using Pcf.GivingtoCustomer.IntegrationTests.Data;

namespace Pcf.GivingToCustomer.IntegrationTests.Components.WebHost.Controllers
{
	[Collection(EfDatabaseCollection.DbCollection)]
	public class PreferencesControllerTests
	{
		private EfRepository<Preference> _preferenceRepository;
		private PreferencesController _preferencesController;
		private EfTestDbInitializer _dbInitializer;

		public PreferencesControllerTests(EfDatabaseFixture efDatabaseFixture)
		{
			_preferenceRepository = new EfRepository<Preference>(efDatabaseFixture.DbContext);
			_preferencesController = new PreferencesController(_preferenceRepository);
			_dbInitializer = new EfTestDbInitializer(efDatabaseFixture.DbContext);
			_dbInitializer.InitializeDb();
		}

		[Fact]
		public async Task GetPreferenceByIdAsync_ExistedPreference_ExistedId()
		{
			// Arrange
			var expectedPreferenceId = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c");

			// Act
			var result = await _preferencesController.GetPreferencesByIdAsync(expectedPreferenceId);

			// Assert
			var actionResult = result.Result as OkObjectResult;
			actionResult.Should().NotBeNull();

			var preferenceResponse = actionResult.Value as PreferenceResponse;
			preferenceResponse.Should().NotBeNull();
			preferenceResponse.Id.Should().Be(expectedPreferenceId);
		}
	}
}
