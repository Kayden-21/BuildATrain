using Lib.AspNetCore.ServerSentEvents;

namespace BuildATrain.Services
{
    public abstract class EventsServiceBase
    {
        private IEventsService _eventsService;

        protected EventsServiceBase(IEventsService eventsService)
        {
            _eventsService = eventsService;

            _eventsService.ClientConnected += _eventsService_ClientConnected;
        }

        protected async Task SendSSEEventAsync(Guid clientGuid)
        {
            await _eventsService.GetClient(clientGuid).SendEventAsync(new ServerSentEvent
            {
                Id = "",
                Type = "",
                Data = new List<string> { clientGuid.ToString() }
            });
        }

        protected async Task SendSSEEventAsync()
        {
            if (_eventsService.GetClients().Any())
            {
                await _eventsService.SendEventAsync(new ServerSentEvent
                {
                    Id = "",
                    Type = "",
                    Data = new List<string> { "test" }
                });
            }
        }

        private void _eventsService_ClientConnected(object? sender, ServerSentEventsClientConnectedArgs e)
        {
            HandleClientConnected(sender, e);
        }

        protected abstract void HandleClientConnected(object? sender, ServerSentEventsClientConnectedArgs e);
    }
}
