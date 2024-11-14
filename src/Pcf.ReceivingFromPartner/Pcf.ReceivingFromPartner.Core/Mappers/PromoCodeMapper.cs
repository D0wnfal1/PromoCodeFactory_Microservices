using MongoDB.Bson;
using Pcf.ReceivingFromPartner.Core.Domain;
using Pcf.ReceivingFromPartner.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Core.Mappers
{
	public class PromoCodeMapper
	{
		public static PromoCode MapFromModel(ReceivingPromoCodeRequest request, Preference preference, Partner partner)
		{

			var promocode = new PromoCode();

			promocode.PartnerId = partner.Id;
			promocode.Partner = partner;
			promocode.Code = request.PromoCode;
			promocode.ServiceInfo = request.ServiceInfo;

			promocode.BeginDate = DateTime.Now;
			promocode.EndDate = DateTime.Now.AddDays(30);

			promocode.Preference = preference;
			promocode.PreferenceId = preference.Id;

			promocode.PartnerManagerId = request.PartnerManagerId.HasValue ? new ObjectId(request.PartnerManagerId.Value.ToString()) : (ObjectId?)null;

			return promocode;
		}
	}
}
