using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
   public class Delivery
   {

      public int DeliveryId { get; set; }

      public string Details { get; set; }

      public string CountryFrom { get; set; }

      public string CountryTo { get; set; }

      public double Distance { get; set; }

      public double WeightPackage { get; set; }

      public int Speed { get; set; }

      public int VehicleId { get; set; }

      public string VehicleName { get; set; }

      public double CarbonEmi { get; set; }
    }
}
