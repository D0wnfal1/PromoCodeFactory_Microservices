using MassTransit;
using Pcf.Administration.Core.Services;

namespace Pcf.Administration.WebHost.Consumers
{
    public class AdministationPromocodesConsumer : IConsumer<NotifyAdminAboutPartnerManagerPromoDto>
    {
        private readonly AdministrationPromocodeService service;

        public AdministationPromocodesConsumer(AdministrationPromocodeService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<NotifyAdminAboutPartnerManagerPromoDto> context)
        {
            await service.UpdateApplliedPromocode(context.Message.PartnerManagerId);
        }
    }   
}
