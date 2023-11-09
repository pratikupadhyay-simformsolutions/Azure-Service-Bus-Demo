using Demo_Azure.DTO.Request;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;

namespace Demo_Azure.Services
{
    public class PratikServiceBus : IServiceBus
    {
        private readonly IConfiguration _configuration;
        public PratikServiceBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMessageAsync(AzureJsonRequest azureJsonRequest)
        {
            IQueueClient client = new QueueClient(_configuration["Azure:ConnectionString"], _configuration["Azure:QueueName"]);
            //Serialize car details object
            var messageBody = JsonSerializer.Serialize(azureJsonRequest);
            //Set content type and Guid
            var message = new Message(Encoding.UTF8.GetBytes(messageBody))
            {
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "application/json"
            };
            await client.SendAsync(message);
        }
    }

    public interface IServiceBus
    {
        Task SendMessageAsync(AzureJsonRequest azureJsonRequest);
    }
}