using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace OMA_DuendeIdentityServer
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            
            var user = await _userManager.GetUserAsync(context.Subject);
            if (user == null)
                return;

            
            var roles = await _userManager.GetRolesAsync(user);

            
            var roleClaims = roles.Select(role => new Claim(JwtClaimTypes.Role, role));

  
            var claims = context.Subject.Claims.ToList();
            claims.AddRange(roleClaims);


            context.IssuedClaims = claims;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
