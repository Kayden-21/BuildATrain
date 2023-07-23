using Lib.AspNetCore.ServerSentEvents;
using System.Threading;

namespace BuildATrain.Services
{
    public class Heartbeat: BackgroundService
    {
        private readonly IServerSentEventsService sseService;

        public Heartbeat(IServerSentEventsService sseService)
        {
            this.sseService = sseService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var clients = sseService.GetClients();

                if (clients.Any())
                {
                    await sseService.SendEventAsync(new ServerSentEvent
                    {
                        Id = "Keep-Alive",
                        Type = "string",
                        Data = new List<string> { "Ah ah ah ah staying alive, staying alive" }
                    });
                }

                await Task.Delay(1000);
            }
        }
    }
}
