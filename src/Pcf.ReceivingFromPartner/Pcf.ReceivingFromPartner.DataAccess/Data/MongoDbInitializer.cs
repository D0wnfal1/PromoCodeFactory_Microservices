using MongoDB.Driver;
using Pcf.ReceivingFromPartner.Core.Abstractions.Repositories;
using Pcf.ReceivingFromPartner.Core.Domain;
using Pcf.ReceivingFromPartner.DataAccess.Data;

namespace Pcf.ReceivingFromPartner.DataAccess.Data
{
	public class MongoDbInitializer : IDbInitializer
	{
		private readonly IRepository<Preference> _preferencesRepository;
		private readonly IRepository<Partner> _partnersRepository;

		public MongoDbInitializer(IRepository<Preference> preferencesRepository, IRepository<Partner> partnersRepository)
		{
			_preferencesRepository = preferencesRepository;
			_partnersRepository = partnersRepository;
		}

		public void InitializeDb()
		{
			_preferencesRepository.DeleteAll();
			_partnersRepository.DeleteAll();

			_preferencesRepository.AddMany(FakeDataFactory.Preferences);
			_partnersRepository.AddMany(FakeDataFactory.Partners);
		}
	}
}
