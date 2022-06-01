using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

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
            return View();
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

      [Authorize(Roles = "manager")]
      public IActionResult Users()
      {
         List<DeliUser> list = DBUtl.GetList<DeliUser>("SELECT * FROM DeliUser WHERE UserRole='member' ");
         return View(list);
      }

      [Authorize(Roles = "manager")]
      public IActionResult Delete(string id)
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

      [AllowAnonymous]
      public IActionResult Register()
      {
         return View("UserRegister");
      }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(DeliUser usr)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("UserRegister");
            }
            else
            {
                string insert =
                   @"INSERT INTO DeliUser(UserId, UserPw, FullName, Email, UserRole) VALUES
                 ('{0}', HASHBYTES('SHA1', '{1}'), '{2}', '{3}', 'member')";
                if (DBUtl.ExecSQL(insert, usr.UserId, usr.UserPw, usr.FullName, usr.Email) == 1)
                {
                    ViewData["Message"] = "User Successfully Registered";
                    ViewData["MsgType"] = "success";
                    return View("UserRegister");

                    string template = @"Hi {0},<br/><br/>
                               Welcome to Carbon Footprint Insights!
                               Your userid is <b>{1}</b> and password is <b>{2}</b>.
                               <br/><br/>FYP Team";
                    string title = "Registration Successul - Welcome";
                    string message = String.Format(template, usr.FullName, usr.UserId, usr.UserPw);
                    string result;
                    if (EmailUtl.SendEmail(usr.Email, title, message, out result))
                    {
                        ViewData["Message"] = "User Successfully Registered";
                        ViewData["MsgType"] = "success";
                    }
                    else
                    {
                        ViewData["Message"] = result;
                        ViewData["MsgType"] = "warning";
                    }
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
                return View("UserRegister");
            }
        }

        [AllowAnonymous]
      public IActionResult VerifyUserID(string userId)
      {
         string select = $"SELECT * FROM DeliUser WHERE UserId='{userId}'";
         if (DBUtl.GetTable(select).Rows.Count > 0)
         {
            return Json($"[{userId}] already in use");
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

   }
}