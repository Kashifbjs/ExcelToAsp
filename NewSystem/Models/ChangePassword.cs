using System.ComponentModel.DataAnnotations;

namespace NewSystem.Models
{
    public class ChangePassword
    {
        public int UserId{ get; set; }

		public string UserName { get; set; }

		public string UserEmail { get; set; }

		[Display(Name = "Enter Old password")]
        public string OldPassword { get; set; }

        [Display(Name = "Enter New Password")]
        public string NewPassword { get; set; }

        [Display(Name = "Comfirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
