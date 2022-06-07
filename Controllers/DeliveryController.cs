using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class DeliveryController : Controller
    {
        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Calculation()
        {
            ViewData["Vehicles"] = GetListVehicles();
            return View();
        }

        [Authorize(Roles = "manager, member, admin")]
        public IActionResult ListDelivery()
        {
            List<Delivery> delivery = DBUtl.GetList<Delivery>(
            @"SELECT * FROM Delivery, Vehicle
              WHERE Delivery.VehicleId = Vehicle.VehicleId");
            return View(delivery);
        }
        [Authorize(Roles = "manager, member, admin")]
        public IActionResult AddDelivery()
        {
            ViewData["Vehicles"] = GetListVehicles();
            return View();
        }
        [Authorize(Roles = "manager, member, admin")]
        [HttpPost]
        public IActionResult AddDelivery(Delivery newDelivery)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Vehicles"] = GetListVehicles();
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("AddDelivery");
            }
            else
            {
                string insert = @"INSERT INTO Delivery(Details, CountryFrom, CountryTo, Distance, WeightPackage, Speed, VehicleId, CarbonEmi)
                                  VALUES('{0}', '{1}', '{2}', {3}, {4}, {5}, {6}, {7})";
                int result =
                DBUtl.ExecSQL(insert, newDelivery.Details, newDelivery.CountryFrom, newDelivery.CountryTo, newDelivery.Distance, newDelivery.WeightPackage, newDelivery.Speed, newDelivery.VehicleId, newDelivery.CarbonEmi); if (result == 1)
                {
                    TempData["Message"] = "Delivery Created";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("ListDelivery");
            }
        }
        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public IActionResult EditDelivery(string id)
        {
            string deliverySql = @"SELECT DeliveryId, Details, CountryFrom, CountryTo,
                                   Distance, WeightPackage, Speed, Vehicle.VehicleId, CarbonEmi
                                   FROM Delivery, Vehicle
                                   WHERE Delivery.VehicleId = Vehicle.VehicleId
                                   AND Delivery.DeliveryId = '{0}'";
            List<Delivery> deliveryList = DBUtl.GetList<Delivery>(deliverySql, id);
            if (deliveryList.Count == 1)
            {
                ViewData["Vehicles"] = GetListVehicles();
                return View(deliveryList[0]);
            }
            else
            {
                TempData["Message"] = "Delivery not found.";
                TempData["MsgType"] = "warning";
                return RedirectToAction("ListDelivery");
            }
        }
        [Authorize(Roles = "manager, admin")]
        [HttpPost]
        public IActionResult EditDelivery(Delivery deli)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Vehicles"] = GetListVehicles();
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("EditDelivery");
            }
            string update = @"UPDATE Delivery
                              SET Details= '{1}', CountryFrom='{2}', CountryTo='{3}', Distance={4}, WeightPackage={5}, Speed={6}, VehicleId={7}, CarbonEmi={8}
                              WHERE DeliveryId={0}";
            int result =
            DBUtl.ExecSQL(update, deli.DeliveryId, deli.Details, deli.CountryFrom,
            deli.CountryTo, deli.Distance, deli.WeightPackage, deli.Speed, deli.VehicleId, deli.CarbonEmi
            );
            if (result == 1)
            {
                TempData["Message"] = "Delivery Updated";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }
            return RedirectToAction("ListDelivery");
        }
        [Authorize(Roles = "manager, admin")]
        public IActionResult DeleteDelivery(int id)
        {
            string select = @"SELECT * FROM Delivery 
                              WHERE DeliveryId={0}";
            DataTable ds = DBUtl.GetTable(select, id);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Delivery record no longer exists.";
                TempData["MsgType"] = "warning";
            }
            else
            {
                string delete = "DELETE FROM Delivery WHERE DeliveryId={0}";
                int res = DBUtl.ExecSQL(delete, id);
                if (res == 1)
                {
                    TempData["Message"] = "Delivery Deleted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("ListDelivery");
        }
        private static SelectList GetListVehicles()
        {
            string vehicleSql = @"SELECT LTRIM(STR(VehicleId)) as Value, VehicleType as Text FROM Vehicle";
            List<SelectListItem> vehicleList = DBUtl.GetList<SelectListItem>(vehicleSql);
            return new SelectList(vehicleList, "Value", "Text");
        }
    }
}

