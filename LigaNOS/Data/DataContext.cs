using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LigaNOS.Data
{
   
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Club> Clubs { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Exclude SelectListGroup from being treated as an entity
            modelBuilder.Ignore<SelectListGroup>();
        }
    }
}
