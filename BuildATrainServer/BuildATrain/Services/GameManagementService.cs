using BuildATrain.Models;

namespace BuildATrain.Services
{
    public class GameManagementService
    {

        #region Properties

        private Dictionary<string, Thread> loadedGames;

        #endregion

        #region Ctor

        public GameManagementService()
        {
            loadedGames = new Dictionary<string, Thread>();
        }

        #endregion

        #region Public

        public void LoadGame(string userID, GameModel gameModel)
        {
            if (!loadedGames.ContainsKey(userID))
            {
                loadedGames.Add(userID, new Thread(() => RunGameLoop(gameModel)));
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
                gameThread.Abort();
                loadedGames.Remove(userID);
            }
            else
            {
                //return game does not exist
            }
        }

        #endregion

        #region Private

        private void RunGameLoop(GameModel gameModel)
        {
            string? loopDuration = "10000";

            if (loopDuration != null)
            {
                while (true)
                {
                    Thread.Sleep(Convert.ToInt32(gameModel));
                }
            }
        }

        #endregion
    }
}
