using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Company
    {
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Please enter your Company Name")]
        [Remote(action: "VerifyCompany", controller: "Account")]
        public string CompanyName { get; set; }


    }


}