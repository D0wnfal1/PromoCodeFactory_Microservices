using MassTransit;
using Microsoft.Extensions.Logging;
using Pcf.GivingToCustomer.Core.Services;

namespace Pcf.GivingToCustomer.WebHost.Consumers
{
    public class PromocodesConsumer : IConsumer<GivePromoCodeToCustomerDto>
    {
        private readonly GivingPromocodesService service;

        public PromocodesConsumer(GivingPromocodesService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<GivePromoCodeToCustomerDto> context)
        {
            await service.GivePromoCodesToCustomersWithPreferenceAsync(context.Message);
        }
    }

    public class SimpleConsumer : IConsumer<NotifyTransaction>
    {
        private readonly ILogger<SimpleConsumer> _logger;

        public SimpleConsumer(ILogger<SimpleConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<NotifyTransaction> context)
        {
            _logger.LogInformation($"Consume notify about transaction {context.Message.CardNumber} amount {context.Message.Amount}");
        }
    }
}
