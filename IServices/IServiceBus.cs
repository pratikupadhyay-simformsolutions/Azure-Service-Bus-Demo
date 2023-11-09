using Demo_Azure.DTO.Request;

namespace Demo_Azure.IServices
{
    public interface IServiceBus
    {
        Task SendMessageAsync(AzureJsonRequest azureJsonRequest);
    }
}