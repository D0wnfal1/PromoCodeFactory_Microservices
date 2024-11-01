using System;
using System.Net.Http;
using System.Threading.Tasks;
using MongoDB.Bson;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class AdministrationGateway
        : IAdministrationGateway
    {
        private readonly HttpClient _httpClient;

        public AdministrationGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task NotifyAdminAboutPartnerManagerPromoCode(ObjectId partnerManagerId)
        {
            var response = await _httpClient.PostAsync($"api/employees/{partnerManagerId}/appliedPromocodes", 
                new StringContent(string.Empty));

            response.EnsureSuccessStatusCode();
        }
    }
}