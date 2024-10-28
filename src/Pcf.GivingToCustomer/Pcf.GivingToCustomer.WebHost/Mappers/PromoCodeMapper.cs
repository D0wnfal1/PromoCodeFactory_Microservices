using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.WebHost.Models;

namespace Pcf.GivingToCustomer.WebHost.Mappers
{
	public class PromoCodeMapper
	{
		public static PromoCode MapFromModel(GivePromoCodeRequest request, Preference preference, IEnumerable<Customer> customers)
		{
			var promocode = new PromoCode()
			{
				Id = request.PromoCodeId,
				PartnerId = request.PartnerId,
				Code = request.PromoCode,
				ServiceInfo = request.ServiceInfo,
				BeginDate = DateTime.Parse(request.BeginDate),
				EndDate = DateTime.Parse(request.EndDate)
			};
			promocode.Preference = preference;
			promocode.PreferenceId = preference.Id;
			promocode.Customers = new List<PromoCodeCustomer>();

			foreach (var customer in customers)
			{
				promocode.Customers.Add(new PromoCodeCustomer()
				{
					CustomerId = customer.Id,
					Customer = customer,
					PromoCodeId = promocode.Id,
					PromoCode = promocode,
				});
			}
			return promocode;
		}
	}
}
