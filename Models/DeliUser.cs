using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class DeliUser

    {
        [Required(ErrorMessage = "Please enter User ID")]
        [Remote(action: "VerifyUserID", controller: "Account")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be 5 characters or more")]
        public string UserPw { get; set; }

        [Compare("UserPw", ErrorMessage = "Passwords do not match")]
        public string UserPw2 { get; set; }

        [Required(ErrorMessage = "Please enter Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        public string UserRole { get; set; }

        public string CompanyName { get; set; }
        public int CompanyId { get; set; }




    }
}