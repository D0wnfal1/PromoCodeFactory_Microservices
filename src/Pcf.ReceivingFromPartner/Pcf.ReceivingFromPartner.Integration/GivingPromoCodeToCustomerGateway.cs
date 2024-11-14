using System.Net.Http;
using System.Threading.Tasks;
using MassTransit;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Domain;
using Pcf.ReceivingFromPartner.Integration.Dto;


namespace Pcf.ReceivingFromPartner.Integration
{
    public class GivingPromoCodeToCustomerGateway
        : IGivingPromoCodeToCustomerGateway
    {
        private readonly HttpClient _httpClient;
        private readonly IPublishEndpoint publishEndpoint;
        public GivingPromoCodeToCustomerGateway(HttpClient httpClient, IPublishEndpoint publishEndpoint)
        {
            _httpClient = httpClient;
            this.publishEndpoint = publishEndpoint;
        }
        
        public async Task GivePromoCodeToCustomer(PromoCode promoCode)
        {
            var dto = new GivePromoCodeToCustomerDto()
            {
                PartnerId = promoCode.Partner.Id,
                BeginDate = promoCode.BeginDate.ToShortDateString(),
                EndDate = promoCode.EndDate.ToShortDateString(),
                PreferenceId = promoCode.PreferenceId,
                PromoCode = promoCode.Code,
                ServiceInfo = promoCode.ServiceInfo,
                PartnerManagerId = promoCode.PartnerManagerId
            };
            await publishEndpoint.Publish(dto);
            //var response = await _httpClient.PostAsJsonAsync("api/v1/promocodes", dto);

            //response.EnsureSuccessStatusCode();
        }
    }
}