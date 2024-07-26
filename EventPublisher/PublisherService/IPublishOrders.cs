using Common;

namespace EventPublisher.PublisherService
{
    public interface IPublishOrders
    {
        public Task Publish(Order order);
        public Task PublishBatch(List<Order> orders);
    }
}
