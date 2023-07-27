using BuildATrain.Models.Game;
using Lib.AspNetCore.ServerSentEvents;

namespace BuildATrain.Services
{
    public class GameManagementService : EventsServiceBase
    {

        #region Properties

        private Dictionary<string, Thread> loadedGames;
        private Dictionary<Guid, string> clientGuidMapping;

        #endregion

        #region Ctor

        public GameManagementService(IEventsService eventsService) : base (eventsService)
        {
            loadedGames = new Dictionary<string, Thread>();
            clientGuidMapping = new Dictionary<Guid, string>();
        }

        #endregion

        #region Public

        public async Task LoadGame(string userID, GameModel gameModel)
        {
            if (!loadedGames.ContainsKey(userID))
            {
                gameModel.Username = userID;
                loadedGames.Add(userID, new Thread(() => RunGameLoop(gameModel)));
                loadedGames[userID].Start();
            }
            else
            {
                //return game already exists
            }
        }

        public void EndGame(string userID)
        {
            if (loadedGames.TryGetValue(userID, out Thread? gameThread))
            {
                gameThread.Suspend();
                gameThread.Abort();
                loadedGames.Remove(userID);
            }
            else
            {
                //return game does not exist
            }
        }

        public void PauseAllGames()
        {
            foreach (KeyValuePair<string, Thread> loadedGame in loadedGames)
            {
                loadedGame.Value.Suspend();
            }
        }

        public void ResumeAllGames()
        {
            foreach (KeyValuePair<string, Thread> loadedGame in loadedGames)
            {
                loadedGame.Value.Resume();
            }
        }

        #endregion

        #region Private

        private async void RunGameLoop(GameModel gameModel)
        {
            string? loopDuration = "5000";

            if (loopDuration != null)
            {
                while (true)
                {
                    try
                    {
                        await SendSSEEventAsync(clientGuidMapping.First(c => c.Value == gameModel.Username).Key);
                        //await SendSSEEventAsync();
                    }
                    catch (Exception e)
                    {

                    }
                    
                    Thread.Sleep(Convert.ToInt32(loopDuration));
                }
            }
        }

        #endregion

        #region Override

        protected override void HandleClientConnected(object? sender, ServerSentEventsClientConnectedArgs e)
        {
            if (e.Request.Query.Any(q => q.Key == "username"))
            {
                clientGuidMapping.Add(e.Client.Id, e.Request.Query.First(q => q.Key == "username").Value);
            }
            else
            {
                //handle error
            }
        }

        #endregion
    }
}
