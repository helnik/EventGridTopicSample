// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Azure.Messaging.EventGrid;
using Common;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SendNotificationFunction
{
    public class SendNotification
    {
        private readonly ILogger<SendNotification> _logger;

        public SendNotification(ILogger<SendNotification> logger)
        {
            _logger = logger;
        }

        [Function(nameof(SendNotification))]
        public void Run([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            _logger.LogInformation("Event type: {type}, Event subject: {subject}", eventGridEvent.EventType, eventGridEvent.Subject);
            
            var receivedOrder = eventGridEvent.Data.ToObjectFromJson<Order>();

            switch (eventGridEvent.EventType)
            {
                case "Order.Created":
                    _logger.LogInformation($"Sending notification mail for order created event with Id: {receivedOrder.Id}");
                    break;
                case "Order.Updated":
                    _logger.LogInformation($"Sending notification mail for order updated event with Id: {receivedOrder.Id}");
                    break;
                case "Order.Deleted":
                    _logger.LogInformation($"Sending notification mail for order deleted event with Id: {receivedOrder.Id}");
                    break;
            }
        }
    }
}
