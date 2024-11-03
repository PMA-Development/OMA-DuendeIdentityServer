using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OMA_DuendeIdentityServer.Models
{
    public class ApplicationDbContext : IdentityDbContext
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
        }
    }
}
