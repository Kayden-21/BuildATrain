using Lib.AspNetCore.ServerSentEvents;

namespace BuildATrain.Models.Event
{
    public class UpdateGameEvent : ServerSentEvent
    {
        public string Id { get; set; } = "";
        public string Type { get; set; } = "";
        public List<string> Data { get; set; } = new List<string> { "data" };

        public List<KeyValuePair<string, string>> Response { get; set; }
    }
}
