
namespace Pcf.GivingToCustomer.Core.Abstractions.Gateways
{
	public interface INotificationGateWay
	{
		Task SendNotificationToPartnerAsync(Guid id, string message);
	}
}
