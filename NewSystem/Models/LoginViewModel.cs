using System.ComponentModel.DataAnnotations;

namespace NewSystem.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Enter Username")]
        public string UserName { get; set; }

        [Display(Name = "Enter Password")]
        public string UserPassword { get; set; }
    }
}
