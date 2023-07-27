using BuildATrain.Common;

namespace BuildATrain.Models.Http.Request
{
    public class DeleteRemoveCarRequest
    {
        public string Email { get; set; }

        public string LocomotiveName { get; set; }

        public CarType CarType { get; set; }
    }
}
