using BuildATrain.Database.Repositories;
using BuildATrain.Models.Game;
using Lib.AspNetCore.ServerSentEvents;
using BuildATrain.Database.Models;
using BuildATrain.Models.Event;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BuildATrain.Services
{
    public class GameManagementService : EventsServiceBase
    {

        #region Properties

        private static Dictionary<string, Thread> loadedGames;
        private static Dictionary<Guid, string> clientGuidMapping;

        private readonly IRepository<TrainModel> _trainRepository;
        private static IRepository<Attributes> _attributeRepository;
        
        private readonly IServiceScopeFactory _scopeFactory;

        #endregion

        #region Ctor

        public GameManagementService(IEventsService eventsService, IRepository<TrainModel> trainRepositor, IRepository<Attributes> attributeRepository, IServiceScopeFactory scopeFactory) : base (eventsService)
        {
            loadedGames = new Dictionary<string, Thread>();
            clientGuidMapping = new Dictionary<Guid, string>();
            _trainRepository = trainRepositor;
            _attributeRepository = attributeRepository;
            _scopeFactory = scopeFactory;
        }

        #endregion

        #region Public

        public async Task LoadGame(string userID)
        {
            if (!loadedGames.ContainsKey(userID))
            {
                var trainModels = await _trainRepository.GetPlayerTrainsByEmailAsync(userID);
                GameModel gameModel = new GameModel();

                gameModel.Email = userID;
                gameModel.Trains = trainModels.ToList();

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

        private async Task RunGameLoop(GameModel gameModel)
        {
            string? loopDuration = "5000";

            if (loopDuration != null)
            {
                while (true)
                {
                    try
                    {
                        var scope = _scopeFactory.CreateScope();
                        var scopedRepoService = scope.ServiceProvider.GetService(typeof(IRepository<Attributes>));
                        double income = 0;

                        foreach (var train in gameModel.Trains)
                        {
                            //var locomotiveConfig = await _attributeRepository.GetByIdAsync((int)train.LocomotiveTypeId + 1);
                            var locomotiveConfig = await ((IRepository<Attributes>)scopedRepoService).GetByIdAsync(1);

                            var passengarCarConfig = await ((IRepository<Attributes>)scopedRepoService).GetByIdAsync((5));
                            var cargoCarConfig = await ((IRepository<Attributes>)scopedRepoService).GetByIdAsync((4));
                            var fuelCarConfig = await ((IRepository<Attributes>)scopedRepoService).GetByIdAsync((6));

                            double fuel = 0;
                            double fuelUse = 0;

                            double incomeMinRange = 0;
                            double incomeMaxRange = 0;

                            fuel += locomotiveConfig.FuelAdded;
                            fuelUse += locomotiveConfig.FuelUse;

                            fuel += passengarCarConfig.FuelAdded * train.NumPassengerCars;
                            fuelUse += passengarCarConfig.FuelUse * train.NumPassengerCars;

                            fuel += cargoCarConfig.FuelAdded * train.NumCargoCars;
                            fuelUse += cargoCarConfig.FuelUse * train.NumCargoCars;

                            fuel += fuelCarConfig.FuelAdded * train.NumFuelCars;
                            fuelUse += fuelCarConfig.FuelUse * train.NumFuelCars;

                            incomeMinRange += locomotiveConfig.IncomeMinRange;
                            incomeMinRange += passengarCarConfig.IncomeMinRange * train.NumPassengerCars;
                            incomeMinRange += cargoCarConfig.IncomeMinRange * train.NumCargoCars;
                            incomeMinRange += fuelCarConfig.IncomeMinRange * train.NumFuelCars;

                            incomeMaxRange += locomotiveConfig.IncomeMaxRange;
                            incomeMaxRange += passengarCarConfig.IncomeMaxRange * train.NumPassengerCars;
                            incomeMaxRange += cargoCarConfig.IncomeMaxRange * train.NumCargoCars;
                            incomeMaxRange += fuelCarConfig.IncomeMaxRange * train.NumFuelCars;

                            double distance = fuel / fuelUse;

                            income = new Random().NextDouble() * (incomeMaxRange - incomeMinRange) + incomeMinRange;
                            income *= distance;

                        }

                        var retList = new List<KeyValuePair<string, string>>();
                        retList.Add(new KeyValuePair<string, string>
                        (
                            "wallet",
                            income.ToString()
                        ));

                        //await SendSSEEventAsync(clientGuidMapping.First(c => c.Value == gameModel.Email).Key, new UpdateGameEvent { Response = retList });
                        await SendSSEEventAsync(clientGuidMapping.First(c => c.Value == gameModel.Email).Key, new List<string> { income.ToString() });
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
            if (e.Request.Query.Any(q => q.Key == "email"))
            {
                clientGuidMapping.Add(e.Client.Id, e.Request.Query.First(q => q.Key == "email").Value);
            }
            else
            {
                //handle error
            }
        }

        #endregion
    }
}
