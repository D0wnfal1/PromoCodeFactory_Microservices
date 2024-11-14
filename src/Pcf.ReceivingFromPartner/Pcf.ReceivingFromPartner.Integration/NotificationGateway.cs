using MongoDB.Bson;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using System;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class NotificationGateway
        : INotificationGateway
    {
		public Task SendNotificationToPartnerAsync(ObjectId partnerId, string message)
		{
			return Task.CompletedTask;
		}
	}
}