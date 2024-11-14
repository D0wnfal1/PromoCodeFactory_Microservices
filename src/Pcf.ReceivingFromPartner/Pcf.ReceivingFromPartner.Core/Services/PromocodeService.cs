

using MongoDB.Bson;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Abstractions.Repositories;
using Pcf.ReceivingFromPartner.Core.Domain;
using Pcf.ReceivingFromPartner.Core.Exceptions;
using Pcf.ReceivingFromPartner.Core.Mappers;
using Pcf.ReceivingFromPartner.Core.Models;

namespace Pcf.ReceivingFromPartner.Core.Services
{
    public class PromocodeService
    {
        private readonly IRepository<Partner> _partnersRepository;
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly INotificationGateway _notificationGateway;
        private readonly IGivingPromoCodeToCustomerGateway _givingPromoCodeToCustomerGateway;
        private readonly IAdministrationGateway _administrationGateway;

        public PromocodeService(IRepository<Partner> partnersRepository,
            IRepository<Preference> preferencesRepository,
            INotificationGateway notificationGateway,
            IGivingPromoCodeToCustomerGateway givingPromoCodeToCustomerGateway,
            IAdministrationGateway administrationGateway)
        {
            _partnersRepository = partnersRepository;
            _preferencesRepository = preferencesRepository;
            _notificationGateway = notificationGateway;
            _givingPromoCodeToCustomerGateway = givingPromoCodeToCustomerGateway;
            _administrationGateway = administrationGateway;
        }

        public async Task<PartnerPromocode> ReceivePromoCodeFromPartnerWithPreferenceAsync(Guid id,
            ReceivingPromoCodeRequest request)
        {
			var partner = await _partnersRepository.GetByIdAsync(new ObjectId(id.ToString()));

            if (partner == null)
            {
                throw new BadRequestException("Partner not found");
            }

            var activeLimit = partner.PartnerLimits.FirstOrDefault(x
                => !x.CancelDate.HasValue && x.EndDate > DateTime.Now);

            if (activeLimit == null)
            {
                throw new BadRequestException("There is no limit available for providing promotional codes");
            }

            if (partner.NumberIssuedPromoCodes + 1 > activeLimit.Limit)
            {
                throw new BadRequestException("The limit for issuing promotional codes has been exceeded");
            }

            if (partner.PromoCodes.Any(x => x.Code == request.PromoCode))
            {
                throw new BadRequestException("This promotional code has already been issued previously");
            }

			var preference = await _preferencesRepository.GetByIdAsync(new ObjectId(request.PreferenceId.ToString()));

            if (preference == null)
            {
                throw new BadRequestException("Preference not found");
            }

            PromoCode promoCode = PromoCodeMapper.MapFromModel(request, preference, partner);
            promoCode.EndDate = promoCode.EndDate.ToUniversalTime();
            promoCode.BeginDate = promoCode.BeginDate.ToUniversalTime();
            partner.PromoCodes.Add(promoCode);
            partner.NumberIssuedPromoCodes++;

            await _partnersRepository.UpdateAsync(partner);

            await _givingPromoCodeToCustomerGateway.GivePromoCodeToCustomer(promoCode);

            if (request.PartnerManagerId.HasValue)
            {
				if (request.PartnerManagerId.HasValue)
				{
					await _administrationGateway.NotifyAdminAboutPartnerManagerPromoCode(new ObjectId(request.PartnerManagerId.Value.ToString()));
				}
				if (request.PartnerManagerId.HasValue)
				{
					var partnerManagerObjectId = new ObjectId(request.PartnerManagerId.Value.ToString());
					await _administrationGateway.NotifyAdminAboutPartnerManagerPromoCode(partnerManagerObjectId);
				}
                await _administrationGateway.NotifyAdminAboutPartnerManagerPromoCode(new ObjectId(request.PartnerManagerId.Value.ToString()));
            }

            return new PartnerPromocode { Partner = partner, Promocode = promoCode };
        }
    }
}
