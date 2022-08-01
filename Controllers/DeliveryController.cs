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

        [Authorize(Roles = "manager, member, admin, customer")]
        public IActionResult ListDelivery()
        {
            List<Delivery> delivery = DBUtl.GetList<Delivery>(
            @"SELECT * FROM Delivery, Vehicle, Company
              WHERE Delivery.VehicleId = Vehicle.VehicleId
              AND Delivery.CompanyId = Company.CompanyId");
            return View(delivery);
        }

        [Authorize(Roles = "manager, admin, member")]
        public IActionResult ListVehicle()
        {
            List<Vehicle> vehicle = DBUtl.GetList<Vehicle>(
             @"SELECT * FROM Vehicle");
            return View(vehicle);
        }

        [Authorize(Roles = "manager, member, admin")]
        public IActionResult AddDelivery()
        {
            ViewData["Vehicles"] = GetListVehicles();
            ViewData["Companies"] = GetListCompanies();
            return View();
        }
        [Authorize(Roles = "manager, member, admin")]
        [HttpPost]
        public IActionResult AddDelivery(Delivery newDelivery)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Vehicles"] = GetListVehicles();
                ViewData["Companies"] = GetListCompanies();
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("AddDelivery");
            }
            else
            {
                string insert =
                    @"INSERT INTO Delivery(FullName, CompanyId, Details, CountryFrom, CountryTo, Distance, 
                                  VehicleId, WeightPackage, VehicleWeight, TotalWeight, VehicleSpeed, CarbonEmi)
                                  VALUES('{0}', {1}, '{2}' ,'{3}', '{4}', {5}, {6}, {7}, {8}, {9}, {10}, {11})";
                int result =
                DBUtl.ExecSQL(insert, newDelivery.FullName, newDelivery.CompanyId, newDelivery.Details, newDelivery.CountryFrom,
                newDelivery.CountryTo, newDelivery.Distance, newDelivery.VehicleId, newDelivery.WeightPackage, newDelivery.VehicleWeight, newDelivery.VehicleSpeed, newDelivery.TotalWeight,
              newDelivery.CarbonEmi);

                if (result == 1)
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
            string deliverySql = @"SELECT DeliveryId, FullName, CompanyId, Details, CountryFrom, CountryTo,
                                   Distance, VehicleId,  WeightPackage, VehicleWeight, TotalWeight, VehicleSpeed, CarbonEmi
                                   FROM Delivery
                                   WHERE Delivery.DeliveryId = '{0}'
                                   ";
            List<Delivery> deliveryList = DBUtl.GetList<Delivery>(deliverySql, id);
            if (deliveryList.Count == 1)
            {
                ViewData["Vehicles"] = GetListVehicles();
                ViewData["Companies"] = GetListCompanies();
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
                ViewData["Companies"] = GetListCompanies();
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("EditDelivery");
            }
            string update = @"UPDATE Delivery
                              SET FullName= '{1}', CompanyId = {2}, Details= '{3}', CountryFrom='{4}',
                              CountryTo='{5}', Distance={6},  VehicleId={7}, WeightPackage={8}, 
                              VehicleWeight ={9}, TotalWeight={10}, VehicleSpeed={11}, CarbonEmi={12}
                              WHERE DeliveryId={0}";
            int result =
            DBUtl.ExecSQL(update, deli.DeliveryId, deli.FullName, deli.CompanyId, deli.Details, deli.CountryFrom,
            deli.CountryTo, deli.Distance, deli.VehicleId, deli.WeightPackage, deli.VehicleWeight, deli.TotalWeight, deli.VehicleSpeed, deli.CarbonEmi
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

        public IActionResult DeleteVehicles(int id)
        {
            string select = @"SELECT * FROM Vehicle
                              WHERE VehicleId={1}";
            DataTable ds = DBUtl.GetTable(select, id);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Vehicle record no longer exists.";
                TempData["MsgType"] = "warning";
            }
            else
            {
                string delete = "DELETE FROM Vehicle WHERE VehicleId={0}";
                int res = DBUtl.ExecSQL(delete, id);
                if (res == 1)
                {
                    TempData["Message"] = "Vehicle Deleted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("ListVehicle");
        }
        private static SelectList GetListVehicles()
        {
            string vehicleSql = @"SELECT LTRIM(STR(VehicleId)) as Value, VehicleType as Text FROM Vehicle";
            List<SelectListItem> vehicleList = DBUtl.GetList<SelectListItem>(vehicleSql);
            return new SelectList(vehicleList, "Value", "Text");
        }
        private static SelectList GetListCompanies()
        {
            string companySql = @"SELECT LTRIM(STR(CompanyId)) as Value, CompanyName as Text FROM Company";
            List<SelectListItem> companyList = DBUtl.GetList<SelectListItem>(companySql);
            return new SelectList(companyList, "Value", "Text");
        }
    }
}
