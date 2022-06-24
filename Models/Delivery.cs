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

        [Required(ErrorMessage = "Please enter Customer's Full Name")]
        public string FullName { get; set; }

        
        public int CompanyId{ get; set; }

        public string CompanyName { get; set; }


        [Required(ErrorMessage = "Please enter Details of Delivery")]
        public string Details { get; set; }

       public string CountryFrom { get; set; }

       public string CountryTo { get; set; }


        [Required(ErrorMessage = "Please enter Delivery Distance")]
        public double Distance { get; set; }

        [Required(ErrorMessage = "Please enter Package Weight")]
        public double WeightPackage { get; set; }

        [Required(ErrorMessage = "Please enter Vehicle Weight")]
        public double VehicleWeight { get; set; }

        [Required(ErrorMessage = "Please enter Total Weight of Vehicle")]
        public double TotalWeight { get; set; }

        [Required(ErrorMessage = "Please enter your Vehicle Speed")]

        public double VehicleSpeed { get; set; }

        public int VehicleId { get; set; }

        public string VehicleType { get; set; }

        [Required(ErrorMessage = "Please enter your Carbon Emission")]
        public double CarbonEmi { get; set; }

  

    }

}
