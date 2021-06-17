using Jwt2._0Authentication.Data.Entities;
using Jwt2._0Authentication.Data.Entities.Other;
using Jwt2._0Authentication.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Jwt2._0Authentication.Data
{
    public class AppDbContext: DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public void SaveChanges()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(x =>
            {
                x.HasData(
                    new User
                    {
                        Id = 1,
                        Name = "Admin",
                        Email = "admin@gmail.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("12345@Vv"),
                        Role = RoleTypes.Admin
                    },
                    new User
                    {
                        Id = 2,
                        Name = "Dominik",
                        Email = "test1@gmail.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("12345@Vv"),
                        Role = RoleTypes.Manager
                    },
                    new User
                    {
                        Id = 3,
                        Name = "Vladislav",
                        Email = "test2@gmail.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("12345@Vv"),
                        Role = RoleTypes.User
                    });
            });
        }
    }
}