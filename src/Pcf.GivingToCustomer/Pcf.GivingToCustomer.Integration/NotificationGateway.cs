using Pcf.GivingToCustomer.Core.Abstractions.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Integration
{
	public class NotificationGateway : INotificationGateWay
	{
		public Task SendNotificationToPartnerAsync(Guid id, string message)
		{
			// Code to invoke Send Notification To Partner

			return Task.CompletedTask;
		}
	}
}
