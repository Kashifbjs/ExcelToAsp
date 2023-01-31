using System.ComponentModel.DataAnnotations;

namespace NewSystem.Models.Account
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserEmail { get; set; }

        public string UserPassword { get; set; }

        public string UserPhone { get; set; }

        public int RoleId { get; set; } = 0;

        public bool IsActive { get; set; } = false;

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; } = DateTime.Now;
    }
}
