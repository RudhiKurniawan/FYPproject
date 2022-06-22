using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Controllers
{

   public class AccountController : Controller
   {
        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("About", "Delivery");
        }

      [AllowAnonymous]
      public IActionResult Login(string returnUrl = null)
      {
         TempData["ReturnUrl"] = returnUrl;
         return View("UserLogin");
      }

      [AllowAnonymous]
      [HttpPost]
      public IActionResult Login(UserLogin user)
      {
         if (!AuthenticateUser(user.UserID, user.Password, out ClaimsPrincipal principal))
         {
            ViewData["Message"] = "Incorrect User ID or Password";
            ViewData["MsgType"] = "warning";
            return View("UserLogin");
         }
         else
         {
                HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   principal);

                if (TempData["returnUrl"] != null)
            {
               string returnUrl = TempData["returnUrl"].ToString();
               if (Url.IsLocalUrl(returnUrl))
                  return Redirect(returnUrl);
            }

            return RedirectToAction("ListDelivery", "Delivery");
         }
      }


      [AllowAnonymous]
      public IActionResult Forbidden()
      {
         return View();
      }

      [Authorize(Roles = "manager, admin")]
      public IActionResult Users()
      {
         List<DeliUser> list = DBUtl.GetList<DeliUser>(
             @"SELECT * FROM DeliUser, Company 
               WHERE DeliUser.CompanyId = Company.CompanyId");

            return View(list);
      }

      [Authorize(Roles = "admin")]
        public IActionResult Companies()
        {
            List<Company> list = DBUtl.GetList<Company>("SELECT * FROM Company ");
            return View(list);
        }


        [Authorize(Roles = "manager, admin")]
      public IActionResult DeleteUser(string id)
      {
         string delete = "DELETE FROM DeliUser WHERE UserId='{0}'";
         int res = DBUtl.ExecSQL(delete, id);
         if (res == 1)
         {
            TempData["Message"] = "User Record Deleted";
            TempData["MsgType"] = "success";
         }
         else
         {
            TempData["Message"] = DBUtl.DB_Message;
            TempData["MsgType"] = "danger";
         }

         return RedirectToAction("Users");
      }

        [Authorize(Roles = "admin")]
        public IActionResult DeleteCompany(string id)
        {
            string delete = "DELETE FROM Company WHERE CompanyId='{0}'";
            int res = DBUtl.ExecSQL(delete, id);
            if (res == 1)
            {
                TempData["Message"] = "Company Record Deleted";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }

            return RedirectToAction("Companies");
        }

        [AllowAnonymous]
      public IActionResult RegisterEmployee()
      {
         ViewData["Companies"] = GetListCompanies();
         return View("UserRegisterEmployee");
      }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterEmployee(DeliUser usr)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Companies"] = GetListCompanies();
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("UserRegisterEmployee");
            }
            else
            {
                string insert =
                   @"INSERT INTO DeliUser(UserId, UserPw, FullName, Email, UserRole, CompanyId) VALUES
                 ('{0}', HASHBYTES('SHA1', '{1}'), '{2}', '{3}', 'member', {4})";
                if (DBUtl.ExecSQL(insert, usr.UserId, usr.UserPw, usr.FullName, usr.Email, usr.CompanyId) == 1)
                {

                    ViewData["Message"] = "User Successfully Registered";
                    ViewData["MsgType"] = "success";
                    return View("UserRegisterEmployee");

                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
                return View("UserRegisterEmployee");
            }
        }

        [AllowAnonymous]
        public IActionResult RegisterCustomer()
        {

            return View("UserRegisterCustomer");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterCustomer(DeliUser usr)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("UserRegisterCustomer");
            }
            else
            {
                string insert =
                   @"INSERT INTO DeliUser(UserId, UserPw, FullName, Email, UserRole, CompanyId) VALUES
                 ('{0}', HASHBYTES('SHA1', '{1}'), '{2}', '{3}', 'customer', NULL)";
                if (DBUtl.ExecSQL(insert, usr.UserId, usr.UserPw, usr.FullName, usr.Email, usr.CompanyId) == 1)
                {

                    ViewData["Message"] = "Customer Successfully Registered";
                    ViewData["MsgType"] = "success";
                    return View("UserRegisterCustomer");

                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
                return View("UserRegisterCustomer");
            }
        }
        [AllowAnonymous]
        public IActionResult RegisterEmployer()
        {
            ViewData["Companies"] = GetListCompanies();
            return View("UserRegisterEmployer");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterEmployer(DeliUser usr)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Companies"] = GetListCompanies();
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("UserRegisterEmployer");
            }
            else
            {
                string insertuser =
               @"INSERT INTO DeliUser(UserId, UserPw, FullName, Email, UserRole, CompanyId) VALUES
                    ('{0}', HASHBYTES('SHA1', '{1}'), '{2}', '{3}', 'manager', {4})";

                if (DBUtl.ExecSQL(insertuser, usr.UserId, usr.UserPw, usr.FullName, usr.Email, usr.CompanyId) == 1)
                {
                    ViewData["Message"] = "User Successfully Registered";
                    ViewData["MsgType"] = "success";
                    return View("UserRegisterEmployer");
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }

                return View("UserRegisterEmployer");
            }
        }
        [AllowAnonymous]
        public IActionResult RegisterCompany()
        {
            ViewData["Companies"] = GetListCompanies();
            return View("UserRegisterCompany");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterCompany(Company com)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Companies"] = GetListCompanies();
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("UserRegisterCompany");
            }
            else
            {
                string insertuser =
               @"INSERT INTO Company(CompanyName) VALUES
                    ('{0}')";

                if (DBUtl.ExecSQL(insertuser, com.CompanyName) == 1)
                {
                    ViewData["Message"] = "Company Successfully Registered";
                    ViewData["MsgType"] = "success";
                    return View("UserRegisterCompany");
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }

                return View("UserRegisterCompany");
            }
        }


        [AllowAnonymous]
      public IActionResult VerifyUserID(string userId)
      {
         string select = $"SELECT * FROM DeliUser WHERE UserId='{userId}'";
         if (DBUtl.GetTable(select).Rows.Count > 0)
         {
            return Json($"{userId} already in use");
         }
         return Json(true);
      }

        [AllowAnonymous]
        public IActionResult VerifyCompany(string companyName)
        {
            string select = $"SELECT * FROM Company WHERE CompanyName='{companyName}'";
            if (DBUtl.GetTable(select).Rows.Count > 0)
            {
                return Json($"{companyName} already in use");
            }
            return Json(true);
        }
        private bool AuthenticateUser(string uid, string pw, out ClaimsPrincipal principal)
      {
         principal = null;

            string sql = @"SELECT * FROM DeliUser 
               WHERE UserId = '{0}' 
                 AND UserPw = HASHBYTES('SHA1', '{1}')";

            string select = String.Format(sql, uid, pw);
            DataTable du = DBUtl.GetTable(select);
            if (du.Rows.Count == 1)
         {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, uid),
                        new Claim(ClaimTypes.Name, du.Rows[0]["FullName"].ToString()),
                        new Claim(ClaimTypes.Role, du.Rows[0]["UserRole"].ToString())
                         },
                         CookieAuthenticationDefaults.AuthenticationScheme));
                  
               
            return true;
         }
         return false;
      }
        private static SelectList GetListCompanies()
        {
            string companySql = @"SELECT LTRIM(STR(CompanyId)) as Value, CompanyName as Text FROM Company";
            List<SelectListItem> companyList = DBUtl.GetList<SelectListItem>(companySql);
            return new SelectList(companyList, "Value", "Text");
        }
    }
}