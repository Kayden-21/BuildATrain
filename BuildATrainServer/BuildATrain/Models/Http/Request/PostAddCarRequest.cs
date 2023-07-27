using BuildATrain.Common;

namespace BuildATrain.Models.Http.Request
{
    public class PostAddCarRequest
    {
        public string Username { get; set; }

        public string LocomotiveName { get; set; }
        public CarType CarType { get; set; }
    }
}
