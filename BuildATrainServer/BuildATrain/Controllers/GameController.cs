using BuildATrain.Common;
using BuildATrain.Database.Repositories;
using BuildATrain.Models.Game;
using BuildATrain.Models.Http.Request;
using BuildATrain.Models.Http.Response;
using BuildATrain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BuildATrain.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
    {
        #region Fields

        private readonly GameManagementService _gameManagementService;
        private readonly IRepository<TrainModel> _trainRepository;

        #endregion

        #region Ctor

        public GameController(
            GameManagementService gameManagementService,
            IRepository<TrainModel> trainRepository)
        {
            this._gameManagementService = gameManagementService;
            _trainRepository = trainRepository;
        }

        #endregion

        #region Post

        [HttpPost]
        [Route("add/train")]
        public async Task<IActionResult> AddTrain(PostAddTrainRequest postAddTrainRequest)
        {
            var locomotiveSize = postAddTrainRequest.LocomotiveType.ToString();
            var locomotiveType = (int)postAddTrainRequest.LocomotiveType;
            var email = postAddTrainRequest.Email;
            var locomotiveName = postAddTrainRequest.LocomotiveName;
            var numFuelCars = 0;
            var numPassengerCars = 1;
            var numCargoCars = 0;

            var isAdded = await _trainRepository.InsertPlayerTrainAsync(locomotiveSize, locomotiveType, locomotiveName, numFuelCars, numPassengerCars, numCargoCars, email);

            if (!isAdded)
            {
                return NotFound();
            }

            var game = await GetNewGame(email);
            var response = new PostAddTrainResponse();
            response.NewGameModel = game;

            return Ok(value: response);
        }

        [HttpPost]
        [Route("add/car")]
        public async Task<IActionResult> AddCar(PostAddCarRequest postAddCarRequest)
        {
            var email = postAddCarRequest.Email;
            var locomotiveName = postAddCarRequest.LocomotiveName;
            var carType = postAddCarRequest.CarType;

            var isAdded = await AddCarAsync(email, locomotiveName, carType);

            if (!isAdded)
            {
                return NotFound();
            }

            var playerTrains = await _trainRepository.GetPlayerTrainsByEmailAsync(email);

            var train = playerTrains.FirstOrDefault(t => t.LocomotiveName == locomotiveName);

            return Ok(value: train);
        }

        #endregion

        #region Get

        [HttpGet]
        [Route("load")]
        public async Task<IActionResult> LoadGame([FromQuery] GetLoadGameRequest getLoadGameRequest)
        {
            await _gameManagementService.LoadGame(getLoadGameRequest.Email);

            return Ok(value: new GetLoadGameResponse());
        }

        #endregion

        #region Put

        #endregion

        #region Delete

        [HttpDelete]
        [Route("remove/train")]
        public async Task<IActionResult> RemoveTrain(DeleteRemoveTrainRequest deleteRemoveTrainRequest)
        {
            var email = deleteRemoveTrainRequest.Email;
            var locomotiveName = deleteRemoveTrainRequest.LocomotiveName;

            var isRemoved = await RemoveTrainAsync(email, locomotiveName);

            if (!isRemoved)
            {
                return NotFound();
            }

            var game = await GetNewGame(email);
            var response = new DeleteRemoveTrainResponse();
            response.NewGameModel = game;

            return Ok(value: response);
        }

        [HttpDelete]
        [Route("remove/car")]
        public async Task<IActionResult> RemoveCar(DeleteRemoveCarRequest deleteRemoveCarRequest)
        {
            var email = deleteRemoveCarRequest.Email;
            var locomotiveName = deleteRemoveCarRequest.LocomotiveName;
            var carType = deleteRemoveCarRequest.CarType;

            var isRemoved = await RemoveCarAsync(email, locomotiveName, carType);

            if (!isRemoved)
            {
                return NotFound();
            }

            var playerTrains = await _trainRepository.GetPlayerTrainsByEmailAsync(email);

            var train = playerTrains.FirstOrDefault(t => t.LocomotiveName == locomotiveName);

            return Ok(value: train);
        }

        private async Task<bool> RemoveTrainAsync(string email, string locomotiveName)
        {
            var playerTrains = await _trainRepository.GetPlayerTrainsByEmailAsync(email);

            var train = playerTrains.FirstOrDefault(t => t.LocomotiveName == locomotiveName);

            if (train == null)
            {
                return false;
            }

            await _trainRepository.DeleteAsync(train);

            return true;
        }

        private async Task<bool> AddCarAsync(string email, string locomotiveName, CarType carType)
        {
            var playerTrains = await _trainRepository.GetPlayerTrainsByEmailAsync(email);

            var train = playerTrains.FirstOrDefault(t => t.LocomotiveName == locomotiveName);

            if (train == null)
            {
                return false;
            }

            await _trainRepository.UpdateCarCountAsync(train.TrainId, carType, 1, email);
            return true;
        }

        private async Task<bool> RemoveCarAsync(string email, string locomotiveName, CarType carType)
        {
            var playerTrains = await _trainRepository.GetPlayerTrainsByEmailAsync(email);

            var train = playerTrains.FirstOrDefault(t => t.LocomotiveName == locomotiveName);

            if (train == null)
            {
                return false;
            }

            await _trainRepository.UpdateCarCountAsync(train.TrainId, carType, -1, email);
            return true;
        }

        private async Task<GameModel> GetNewGame(string email)
        {
            var trainModels = await _trainRepository.GetPlayerTrainsByEmailAsync(email);
            GameModel gameModel = new GameModel();

            gameModel.Email = email;
            gameModel.Trains = trainModels.ToList();

            return gameModel;
        }

        #endregion
    }
}
