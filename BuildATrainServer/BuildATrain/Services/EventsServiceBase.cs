using Lib.AspNetCore.ServerSentEvents;

namespace BuildATrain.Services
{
    public abstract class EventsServiceBase
    {
        private IEventsService _eventsService;

        protected EventsServiceBase(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        protected async Task SendSSEEventAsync(Guid clientGuid)
        {
            await _eventsService.GetClient(clientGuid).SendEventAsync(new ServerSentEvent
            {
                Id = "",
                Type = "",
                Data = new List<string> { "test" }
            });
        }
    }
}
