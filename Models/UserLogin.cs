using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
   public class UserLogin
   {
      [Required(ErrorMessage = "Please enter User ID")]
      public string UserID { get; set; }

      [Required(ErrorMessage = "Please enter Password")]
      [DataType(DataType.Password)]
      public string Password { get; set; }

      public bool RememberMe { get; set; }
   }
}
