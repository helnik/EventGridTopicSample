using Azure;
using Azure.Messaging.EventGrid;
using Common;

using Microsoft.Extensions.Configuration;

namespace EventPublisher.PublisherService
{
    public class EventGridPublisherService : IPublishOrders
    {
        private readonly EventGridPublisherClient _client;

        public EventGridPublisherService(IConfiguration configuration)
        {
            var endpoint = new Uri(configuration["TopicEndpoint"]);
            var credentials = new AzureKeyCredential(configuration["TopicKey"]);
            _client = new EventGridPublisherClient(endpoint, credentials);
        }

        /// <summary>
        /// Publishes an order as an event to the Event Grid service.
        /// </summary>
        /// <param name="order">The order to be published.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Publish(Order order)
        {
            var orderEvent = new EventGridEvent(
                        subject: $"Order.{order.Id}.{order.Action}",
                        eventType: $"Order.{order.Action}",
                        dataVersion: "1.0",
                        data: order);

            await _client.SendEventAsync(orderEvent);
        }

        /// <summary>
        /// Publishes a batch of orders as events to the Event Grid service.
        /// </summary>
        /// <param name="orders">The list of orders to be published.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PublishBatch(List<Order> orders)
        {
            var events = orders
                .Select(order =>
                    new EventGridEvent(subject: $"Order.{order.Id}.{order.Action}",
                        eventType: $"Order.{order.Action}",
                        dataVersion: "1.0", data: order))
                .ToList();

            await _client.SendEventsAsync(events);
        }
    }
}

