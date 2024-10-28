using System;
using System.Threading.Tasks;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class NotificationGateway
        : INotificationGateway
    {
        public Task SendNotificationToPartnerAsync(Guid partnerId, string message)
        {   
            // Notification Service
            return Task.CompletedTask;
        }
    }
}