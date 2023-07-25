using BuildATrain.Common;

namespace BuildATrain.Models.Http.Request
{
    public class PostAddTrainRequest
    {
        public string Username { get; set; }

        public LocomotiveType LocomotiveType { get; set; }
        public string LocomotiveName { get; set; }
    }
}
