using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().HasData(
                new Team { Id=1, Name= "Walter White", Position= "Chief Executive Officer", Image="team-1.jpg" },
                new Team { Id=2, Name= "Sarah Jhonson", Position= "Product Manager", Image="team-2.jpg" },
                new Team { Id=3, Name= "William Anderson", Position= "CTO", Image="team-3.jpg" },
                new Team { Id=4, Name= "Amanda Jepson", Position= "Accountant", Image="team-4.jpg" }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
