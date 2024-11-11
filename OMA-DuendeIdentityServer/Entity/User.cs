using Microsoft.AspNetCore.Identity;

namespace OMA_DuendeIdentityServer.Entity
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
