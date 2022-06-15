using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string VehicleType { get; set; }

        public double VehicleSpeed { get; set; }

        public double VehicleWeight { get; set; }
    }
}
