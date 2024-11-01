using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class NotificationGateway
        : INotificationGateway
    {
        public Task SendNotificationToPartnerAsync(ObjectId partnerId, string message)
        {   
            // Notification Service
            return Task.CompletedTask;
        }
    }
}