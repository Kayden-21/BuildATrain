using BuildATrain.Common;
using BuildATrain.Models.Http.Request;
using Microsoft.AspNetCore.Mvc;

namespace BuildATrain.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
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
        public void LoadGame(GetLoadGameRequest getLoadGameRequest)
        {

        }

        //[HttpGet]
        //[Route("connect")]
        //public async Task Connect()
        //{
        //    Response.Headers.Add(Headers.CONTENT_TYPE, HeaderValues.ContentType.TEXT_OR_EVENT_STREAM);
        //    Response.Headers.Add(Headers.CACHE_CONTROL, HeaderValues.CacheControl.NO_CACHE);
        //    Response.Headers.Add(Headers.CONNECTION, HeaderValues.Connection.KEEP_ALIVE);

        //    while (true)
        //    {
        //        await Response.WriteAsync("Ah ah ah ah staying alive, staying alive");
        //        await Response.Body.FlushAsync();
        //        await Task.Delay(1000);
        //    }
        //}

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
