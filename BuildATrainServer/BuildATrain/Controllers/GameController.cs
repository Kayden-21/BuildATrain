using BuildATrain.Common;
using BuildATrain.Database.Repositories;
using BuildATrain.Models.Game;
using BuildATrain.Models.Http.Request;
using BuildATrain.Services;
using Microsoft.AspNetCore.Mvc;
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
            var username = postAddTrainRequest.Username;
            var locomotiveName = postAddTrainRequest.LocomotiveName;
            var numFuelCars = 0;
            var numPassengerCars = 0;
            var numCargoCars = 0;

            await _trainRepository.InsertPlayerTrainAsync(locomotiveSize, locomotiveName, numFuelCars, numPassengerCars, numCargoCars, username);

            return Ok();
        }

        [HttpPost]
        [Route("add/car")]
        public void AddCar(PostAddCarRequest postAddCarRequest)
        {

        }

        #endregion

        #region Get

        [HttpGet]
        [Route("load")]
        public async Task LoadGame([FromQuery] GetLoadGameRequest getLoadGameRequest)
        {
            await _gameManagementService.LoadGame(getLoadGameRequest.Username, new Models.Game.GameModel());
        }

        #endregion

        #region Put

        #endregion

        #region Delete

        [HttpDelete]
        [Route("remove/train")]
        public async Task<IActionResult> RemoveTrain(DeleteRemoveTrainRequest deleteRemoveTrainRequest)
        {
            var username = deleteRemoveTrainRequest.Username;
            var locomotiveName = deleteRemoveTrainRequest.LocomotiveName;

            var isRemoved = await RemoveTrainAsync(username, locomotiveName);

            if (!isRemoved)
            {
                return NotFound();
        }

            return NoContent();
        }

        [HttpDelete]
        [Route("remove/car")]
        public void RemoveCar(DeleteRemoveCarRequest deleteRemoveCarRequest)
        {

        }

        private async Task<bool> RemoveTrainAsync(string username, string locomotiveName)
        {
            var playerTrains = await _trainRepository.GetPlayerTrainsByUsernameAsync(username);

            var train = playerTrains.FirstOrDefault(t => t.LocomotiveName == locomotiveName);

            if (train == null)
            {
                return false;
            }

            await _trainRepository.DeleteAsync(train);

            return true;
        }

        private async Task<bool> AddCarAsync()
        {
            return true;
        }

        private async Task<bool> RemoveCarAsync()
        {
            return true;
        }
        #endregion
    }
}
