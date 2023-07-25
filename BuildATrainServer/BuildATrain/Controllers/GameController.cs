using BuildATrain.Common;
using BuildATrain.Models.Http.Request;
using BuildATrain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildATrain.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
    {
        #region Fields

        private readonly GameManagementService _gameManagementService;

        #endregion

        #region Ctor

        public GameController(GameManagementService gameManagementService)
        {
            _gameManagementService = gameManagementService;
        }

        #endregion

        #region Post

        [HttpPost]
        [Route("add/train")]
        public void AddTrain(PostAddTrainRequest postAddTrainRequest)
        {

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
        public void RemoveTrain(DeleteRemoveTrainRequest deleteRemoveTrainRequest)
        {

        }

        [HttpDelete]
        [Route("remove/car")]
        public void RemoveCar(DeleteRemoveCarRequest deleteRemoveCarRequest)
        {

        }

        #endregion
    }
}
