using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LigaNOS.Data
{
   
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}
