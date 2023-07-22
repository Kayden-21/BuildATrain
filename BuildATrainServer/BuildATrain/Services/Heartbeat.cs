using Lib.AspNetCore.ServerSentEvents;

namespace BuildATrain.Services
{
    public class Heartbeat: IHostedService
    {
        private readonly IServerSentEventsService sseService;

        public Heartbeat(IServerSentEventsService sseService)
        {
            this.sseService = sseService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
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
            catch(TaskCanceledException)
            {

            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
