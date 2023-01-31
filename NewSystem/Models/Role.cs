namespace NewSystem.Models
{
    public class Role
    {
        public int RoleId { get; set; } 

        public string RoleName { get; set; }

        public string RoleDescription { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; }
    }
}
