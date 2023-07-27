using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Options;

namespace BuildATrain.Services
{
    public class EventsService : ServerSentEventsService, IEventsService
    {
        public EventsService(IOptions<ServerSentEventsServiceOptions<EventsService>> options)
            : base(options.ToBaseServerSentEventsServiceOptions())
        {

        }
    }
}
