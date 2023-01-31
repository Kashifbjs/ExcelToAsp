using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NewSystem.Models;
using NewSystem.Models.Account;

namespace NewSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Vendor> Vendors { get; set; }
    }
}
