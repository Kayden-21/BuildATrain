using BuildATrain.Models.Http.Request;
using Microsoft.AspNetCore.Mvc;

namespace BuildATrain.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameController
    {
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
        public void LoadGame()
        {

        }

        #endregion

        #region Put

        #endregion

        #region Delete

        [HttpDelete]
        [Route("remove/train")]
        public void RemoveTrain()
        {

        }

        [HttpDelete]
        [Route("remove/car")]
        public void RemoveCar()
        {

        }

        #endregion
    }
}
