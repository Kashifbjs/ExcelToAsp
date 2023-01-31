using System.ComponentModel.DataAnnotations;

namespace NewSystem.Models
{
    public class SignupViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter username!")]
        [Display(Name = "Enter User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter email!")]
        [Display(Name = "Enter Email")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Please enter password!")]
        [Display(Name = "Enter Password")]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Please enter confirm password!")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter mobile number!")]
        [Display(Name = "Enter Mobile No")]
        public String UserPhone { get; set; }

        [Required(ErrorMessage = "Please enter shop name!")]
        [Display(Name = "Enter Shop Name")]
        public string UserShopName { get; set; }

        public int RoleId { get; set; } = 0;

        public bool IsActive { get; set; }
    }
}
