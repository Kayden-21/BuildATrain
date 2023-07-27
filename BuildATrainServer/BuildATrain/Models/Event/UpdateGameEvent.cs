using Lib.AspNetCore.ServerSentEvents;

namespace BuildATrain.Models.Event
{
    public class UpdateGameEvent : ServerSentEvent
    {
        public List<KeyValuePair<string, string>> Response { get; set; }
    }
}
