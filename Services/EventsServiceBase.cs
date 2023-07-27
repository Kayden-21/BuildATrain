using BuildATrain.Models.Event;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Logging;
using System.Globalization;

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

        protected async Task SendSSEEventAsync(Guid clientGuid, ServerSentEvent @event)
        {
            await _eventsService.GetClient(clientGuid).SendEventAsync(@event);
        }

        protected async Task SendSSEEventAsync(Guid clientGuid, List<string> data, string Id = "", string Type = "")
        {
            await _eventsService.GetClient(clientGuid).SendEventAsync(new ServerSentEvent
            {
                Id = Id,
                Type = Type,
                Data = data
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
