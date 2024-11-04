using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace OMA_DuendeIdentityServer.Pages.ClientManagement
{
    public class CreateModel : PageModel
    {

        private readonly ConfigurationDbContext _context;

        public CreateModel(ConfigurationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Client NewClient { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Generate a random Clientsecret
            var randomSecret = SecretGenerator.GenerateRandomSecret();
            //var hashedSecret = SecretGenerator.HashSecret(randomSecret);

            // Add the secret to the client's secrets
            NewClient.ClientSecrets = new List<Secret>() { new(randomSecret.Sha256()) };
            NewClient.AllowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles"
            };

            NewClient.AllowedGrantTypes = GrantTypes.Code;
            NewClient.RequireClientSecret = true;
            NewClient.RequirePkce = true;
            NewClient.AllowPlainTextPkce = false;

            _context.Clients.Add(NewClient.ToEntity());
            await _context.SaveChangesAsync();

            //shows the Secret on index when the client gets created
            TempData["GeneratedSecret"] = randomSecret;

            return RedirectToPage("Index");
        }
    }
}
