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

        [Authorize(Roles = "manager")]
        public IActionResult Distance()
      {

         PrepareData(1);
         ViewData["Chart"] = "line";
         ViewData["Title"] = "Distance";
         ViewData["ShowLegend"] = "true";
         return View("Charts");
      }
        [Authorize(Roles = "manager")]
        public IActionResult Vehicle()
        { 
         PrepareData(2); 
         ViewData["Chart"] = "pie";
         ViewData["Title"] = "Vehicle";
         ViewData["ShowLegend"] = "true";
         return View("Charts");
      }
        [Authorize(Roles = "manager")]
        public IActionResult CarbonEmission()
      {
         
         PrepareData(3);
         ViewData["Chart"] = "bar";
         ViewData["Title"] = "Carbon Emission";
         ViewData["ShowLegend"] = "true";
         return View("Charts");
      }

      private void PrepareData(int x)
      {
         int[] distance = new int[] { 0, 0, 0, 0, 0 };
         int[] vehicle = new int[] { 0, 0, 0, 0 };
         int[] carbonemi = new int[] { 0, 0, 0 };
         List<Delivery> list = DBUtl.GetList<Delivery>("SELECT * FROM Delivery");
         foreach (Delivery dv in list)
         {
            if (dv.Distance < 50) distance[0]++;
            else if (dv.Distance < 250) distance[1]++;
            else if (dv.Distance < 500) distance[2]++;
            else if (dv.Distance < 1000) distance[3]++;
            else distance[4]++;

            if (dv.VehicleId == 1) vehicle[0]++;
            else if (dv.VehicleId == 2) vehicle[1]++;
            else if (dv.VehicleId == 3) vehicle[2]++;
            else vehicle[3]++;

            if (dv.CarbonEmi < 50) carbonemi[0]++;
            else carbonemi[1]++;
         }

         if (x == 1)
         {
            ViewData["Legend"] = "Delivery by Distance";
            ViewData["Colors"] = new[] { "violet", "green", "blue", "orange", "red" };
            ViewData["Labels"] = new[] { "Very Short", "Short", "Normal", "Long", "Very Long" };
            ViewData["Data"] = distance;
         }
         else if (x == 2)
         {
            ViewData["Legend"] = "Delivery by Vehicle Type";
            ViewData["Colors"] = new[] { "red", "orange", "yellow", "green" };
            ViewData["Labels"] = new[] { "Van", "Truck", "Ship", "Plane" };
            ViewData["Data"] = vehicle;
         }
         else if (x == 3)
         {
            ViewData["Legend"] = "Delivery by Carbon Emission";
            ViewData["Colors"] = new[] { "green", "red" };
            ViewData["Labels"] = new[] { "Good", "Bad" };
            ViewData["Data"] = carbonemi;
         }
         else
         {
            ViewData["Legend"] = "Nothing";
            ViewData["Colors"] = new[] { "gray", "darkgray", "black" };
            ViewData["Labels"] = new[] { "X", "Y", "Z" };
            ViewData["Data"] = new int[] { 1, 2, 3};
         }

      }

   }

}
