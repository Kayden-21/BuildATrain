using BuildATrain.Common;

namespace BuildATrain.Models.Http.Request
{
    public class PostAddTrainRequest
    {
        public string Email { get; set; }

        public LocomotiveType LocomotiveType { get; set; }
        public string LocomotiveName { get; set; }
    }
}
