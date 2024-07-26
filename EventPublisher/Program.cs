using Common;
using EventPublisher.PublisherService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventPublisher
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", false)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IPublishOrders, EventGridPublisherService>()
                .BuildServiceProvider();

            var publisher = serviceProvider.GetService<IPublishOrders>();
            await publisher.Publish(new Order
            {
                Id = 1234,
                ProductName = "TheMostInnovativeProduct",
                Quantity = 1
            });
            Console.WriteLine("Order published successfully!");
            await publisher.PublishBatch(new List<Order>
            {
                new Order
                {
                    Id = 7,
                    ProductName = "TheMostInnovativeProduct",
                    Quantity = 1
                },
                new Order
                {
                    Id = 42,
                    ProductName = "TheSecondMostInnovativeProduct",
                    Quantity = 1
                }
            });
            Console.WriteLine("Orders published successfully!");
        }
    }
}
