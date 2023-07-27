using BuildATrain.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BuildATrain.Models.Game
{
    public class TrainModel
    {
        [Key]
        public int TrainId { get; set; }

        //public LocomotiveType LocomotiveTypeId { get; set; }
        //public string LocomotiveName { get; set; }

        public int NumPassengerCars { get; set; }
        public int NumCargoCars { get; set; }
        public int NumFuelCars { get; set; }
    }
}
