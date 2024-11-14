using System;
using System.Net.Http;
using System.Threading.Tasks;
using MassTransit;
using MongoDB.Bson;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.Administration.Core.Services;
using Pcf.ReceivingFromPartner.Core.Domain;

namespace Pcf.ReceivingFromPartner.Integration
{
	public class AdministrationGateway
			: IAdministrationGateway
	{
		private readonly HttpClient _httpClient;
		private readonly IPublishEndpoint publishEndpoint;

		public AdministrationGateway(HttpClient httpClient, IPublishEndpoint publishEndpoint)
		{
			_httpClient = httpClient;
			this.publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
		}

		public async Task NotifyAdminAboutPartnerManagerPromoCode(ObjectId partnerManagerId)
		{
			//var response = await _httpClient.PostAsync($"api/v1/employees/{partnerManagerId}/appliedPromocodes", 
			//    new StringContent(string.Empty));

			//response.EnsureSuccessStatusCode();
			var dto = new NotifyAdminAboutPartnerManagerPromoDto()
			{
				PartnerManagerId = new Guid(partnerManagerId.ToByteArray())
			};
			await publishEndpoint.Publish(dto);
		}
	}
}