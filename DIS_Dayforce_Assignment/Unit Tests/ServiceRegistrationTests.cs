using Xunit;
using DIS_Dayforce_Assignment.Services;

namespace DIS_Dayforce_Assignment.Tests
{
    // Unit test class
    public class ServiceRegistrationTests
    {
        [Fact]
        public void Should_Resolve_IPayInfoService()
        {
            // Arrange: Set up the service collection and register services
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IPayInfoService, PayInfoService>(); // Registering your service
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Act: Resolve the service
            var service = serviceProvider.GetService<IPayInfoService>();

            // Assert: Ensure the service is not null, meaning it was registered correctly
            Assert.NotNull(service);
            Assert.IsType<PayInfoService>(service);
        }
    }
}