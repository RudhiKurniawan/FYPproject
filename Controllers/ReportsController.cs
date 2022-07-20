using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ReportsController : Controller
    {
        [Authorize(Roles = "manager, admin")]
        public IActionResult Vehicle()
        {
            PrepareData(1);
            ViewData["Chart"] = "pie";
            ViewData["Title"] = "Vehicle";
            ViewData["ShowLegend"] = "true";
            return View("Charts");
        }
        [Authorize(Roles = "manager, admin")]
        public IActionResult CarbonEmission()
        {

            PrepareData(2);
            ViewData["Chart"] = "bar";
            ViewData["Title"] = "Carbon Emission";
            ViewData["ShowLegend"] = "true";
            return View("Charts");
        }

        private void PrepareData(int x)
        {
            int[] vehicle = new int[] { 0, 0, 0, 0, 0, 0, 0 };
            int[] carbonemi = new int[] { 0, 0, 0 };
            List<Delivery> list = DBUtl.GetList<Delivery>("SELECT * FROM Delivery");
            foreach (Delivery dv in list)
            {

                if (dv.VehicleId == 1) vehicle[0]++;
                else if (dv.VehicleId == 2) vehicle[1]++;
                else if (dv.VehicleId == 3) vehicle[2]++;
                else if (dv.VehicleId == 4) vehicle[3]++;
                else if (dv.VehicleId == 5) vehicle[4]++;
                else if (dv.VehicleId == 6) vehicle[5]++;
                else vehicle[6]++;

                if (dv.CarbonEmi < 25) carbonemi[0]++;
                else if (dv.CarbonEmi < 50) carbonemi[1]++;
                else carbonemi[2]++;
            }

    
            if (x == 1)
            {
                ViewData["Legend"] = "Vehicle Type Preference";
                ViewData["Colors"] = new[] { "red", "orange", "yellow", "green", "brown", "black", "grey" };
                ViewData["Labels"] = new[] { "Van", "Truck", "Ship", "Plane", "Motorcycle", "Bicycle", "Car" };
                ViewData["Data"] = vehicle;
            }
            else if (x == 2)
            {
                ViewData["Legend"] = "Carbon Emission Level";
                ViewData["Colors"] = new[] { "black", "black", "black"  };
                ViewData["Labels"] = new[] { "Low", "Moderate", "High"};
                ViewData["Data"] = carbonemi;
            }
            else
            {
                ViewData["Legend"] = "Nothing";
                ViewData["Colors"] = new[] { "gray", "darkgray", "black" };
                ViewData["Labels"] = new[] { "X", "Y", "Z" };
                ViewData["Data"] = new int[] { 1, 2, 3 };
            }

        }

    }

}