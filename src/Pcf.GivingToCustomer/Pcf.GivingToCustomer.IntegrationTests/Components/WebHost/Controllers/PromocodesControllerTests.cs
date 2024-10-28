using Pcf.GivingtoCustomer.IntegrationTests;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.DataAccess.Repositories;
using Pcf.GivingToCustomer.WebHost.Controllers;

namespace Pcf.GivingToCustomer.IntegrationTests.Components.WebHost.Controllers
{
	[Collection(EfDatabaseCollection.DbCollection)]
	public class PromocodesControllerTests
	{
        private EfRepository<PromoCode> _promocodeRepository;
        private EfRepository<Customer> _customerRepository;
        private EfRepository<Preference> _preferenceRepository;
        private PromocodesController _promocodeController;
        public PromocodesControllerTests(EfDatabaseFixture efDatabaseFixture)
        {
            _promocodeRepository = new EfRepository<PromoCode>(efDatabaseFixture.DbContext);
			_customerRepository = new EfRepository<Customer>(efDatabaseFixture.DbContext);
			_preferenceRepository = new EfRepository<Preference>(efDatabaseFixture.DbContext);
            _promocodeController = new PromocodesController(_promocodeRepository, _preferenceRepository, _customerRepository);
        }
    }
}
