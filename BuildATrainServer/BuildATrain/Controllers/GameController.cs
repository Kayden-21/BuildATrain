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
        public async Task<PostAddTrainResponse> AddTrain(PostAddTrainRequest postAddTrainRequest)
        {
            var locomotiveSize = postAddTrainRequest.LocomotiveType.ToString();
            var username = postAddTrainRequest.Email;
            var locomotiveName = postAddTrainRequest.LocomotiveName;
            var numFuelCars = 0;
            var numPassengerCars = 1;
            var numCargoCars = 0;

            await _trainRepository.InsertPlayerTrainAsync(locomotiveSize, locomotiveName, numFuelCars, numPassengerCars, numCargoCars, username);

            return new PostAddTrainResponse();
        }

        [HttpPost]
        [Route("add/car")]
        public async Task<PostAddCarResponse> AddCar(PostAddCarRequest postAddCarRequest)
        {
            var username = postAddCarRequest.Email;
            var locomotiveName = postAddCarRequest.LocomotiveName;
            var carType = postAddCarRequest.CarType;

            await AddCarAsync(username, locomotiveName, carType);

            return new PostAddCarResponse();
        }

        #endregion

        #region Get

        [HttpGet]
        [Route("load")]
        public async Task<GetLoadGameResponse> LoadGame([FromQuery] GetLoadGameRequest getLoadGameRequest)
        {
            await _gameManagementService.LoadGame(getLoadGameRequest.Email);

            return new GetLoadGameResponse();
        }

        #endregion

        #region Put

        #endregion

        #region Delete

        [HttpDelete]
        [Route("remove/train")]
        public async Task<DeleteRemoveTrainResponse> RemoveTrain(DeleteRemoveTrainRequest deleteRemoveTrainRequest)
        {
            var email = deleteRemoveTrainRequest.Email;
            var locomotiveName = deleteRemoveTrainRequest.LocomotiveName;

            var isRemoved = await RemoveTrainAsync(email, locomotiveName);

            if (!isRemoved)
            {
                return new DeleteRemoveTrainResponse();
            }

            return new DeleteRemoveTrainResponse();
        }

        [HttpDelete]
        [Route("remove/car")]
        public async Task<DeleteRemoveCarResponse> RemoveCar(DeleteRemoveCarRequest deleteRemoveCarRequest)
        {
            var email = deleteRemoveCarRequest.Email;
            var locomotiveName = deleteRemoveCarRequest.LocomotiveName;
            var carType = deleteRemoveCarRequest.CarType;

            await RemoveCarAsync(email, locomotiveName, carType);

            return new DeleteRemoveCarResponse();
        }

        private async Task<bool> RemoveTrainAsync(string email, string locomotiveName)
        {
            var playerTrains = await _trainRepository.GetPlayerTrainsByEmailAsync(email);

            //var train = playerTrains.FirstOrDefault(t => t.LocomotiveName == locomotiveName);

            //if (train == null)
            //{
            //    return false;
            //}

            //await _trainRepository.DeleteAsync(train);

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

            await _trainRepository.UpdateCarCountAsync(train.TrainId, carType, 1);
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

            await _trainRepository.UpdateCarCountAsync(train.TrainId, carType, -1);
            return true;
        }

        #endregion
    }
}
