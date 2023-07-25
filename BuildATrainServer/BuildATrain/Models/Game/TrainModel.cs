using BuildATrain.Common;

namespace BuildATrain.Models.Game
{
    public class TrainModel
    {
        public LocomotiveType LocomotiveType { get; set; }
        public string LocomotiveName { get; set; }

        public int PassengerCarCount { get; set; }
        public int CargoCarCount { get; set; }
        public int FuelCarCount { get; set; }
    }
}
