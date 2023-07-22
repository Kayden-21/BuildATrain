using BuildATrain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildATrain.Controllers
{
    public class EventsController : Controller
    {
        private IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }


        [ActionName("sse-notifications-sender")]
        [AcceptVerbs("GET")]
        public void Sender()
        {

        }

        [ActionName("sse-notifications-receiver")]
        [AcceptVerbs("GET")]
        public void Receiver()
        {

        }
    }
}
