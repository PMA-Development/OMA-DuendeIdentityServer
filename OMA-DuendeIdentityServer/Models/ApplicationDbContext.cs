using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OMA_DuendeIdentityServer.Entity;

namespace OMA_DuendeIdentityServer.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define roles
            var adminRole = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            var hotlineUserRole = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Hotline-User",
                NormalizedName = "HOTLINE-USER"
            };

            // Seed roles
            builder.Entity<IdentityRole>().HasData(adminRole, hotlineUserRole);

            // Define admin user
            var adminUser = new User
            {
                Id = "c6936336-4a10-4445-b373-60f6a37a58c4",
                UserName = "adminUser",
                NormalizedUserName = "ADMINUSER",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                FullName = "Admin User",
                PhoneNumber = "1234567890",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "P@ssw0rd")
            };

            // Define hotline user
            var hotlineUser = new User
            {
                Id = "cf9844c4-55aa-4eef-bba2-9b97771a8c29",
                UserName = "hotlineUser",
                NormalizedUserName = "HOTLINEUSER",
                Email = "hotlineuser@example.com",
                NormalizedEmail = "HOTLINEUSER@EXAMPLE.COM",
                FullName = "Hotline User",
                PhoneNumber = "0987654321",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "P@ssw0rd")
            };

            // Seed users
            builder.Entity<User>().HasData(adminUser, hotlineUser);

            // Assign roles to users
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRole.Id,
                    UserId = adminUser.Id
                },
                new IdentityUserRole<string> 
                {
                    RoleId = adminRole.Id,
                    UserId = adminUser.Id
                },

                new IdentityUserRole<string>
                {
                    RoleId = hotlineUserRole.Id,
                    UserId = hotlineUser.Id
                }
            );
        }
    }
}
