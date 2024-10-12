using Microsoft.EntityFrameworkCore;
using TAZZKARTY.Models;

namespace TAZZKARTY.Services
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Match> Matches { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlServer("Server=.;Database = TAZZKARTY;Integrated Security=true;Encrypt=false");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
      .HasMany(u => u.Matches)
      .WithMany(m => m.Users);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                NationalId = "0112126511004",
                FullName = "Ahmed",
                Password = "123#Aa6",
                Phone = "01152751234",
                Role = Role.Admin ,
                Email= "ahmed34@gmail.com"
            });
        }
    }

}

