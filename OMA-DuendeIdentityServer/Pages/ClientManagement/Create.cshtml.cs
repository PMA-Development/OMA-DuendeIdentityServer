using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public Client Client { get; set; }

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
            var hashedSecret = SecretGenerator.HashSecret(randomSecret);

            // Add the secret to the client's secrets
            Client.ClientSecrets = new List<ClientSecret>
            {
                new ClientSecret
                {
                    Value = hashedSecret,
                    Description = "Auto-generated secret",
                    Expiration = null // can set an expiration date if wanted
                }
            };

            Client.AllowedScopes = new List<ClientScope>
            {
                new ClientScope { Scope = "openid" },
                new ClientScope { Scope = "profile" },
                new ClientScope { Scope = "email" },
                new ClientScope { Scope = "role" }
            };

            Client.RequirePkce = true;
            Client.AllowPlainTextPkce = false;

            _context.Clients.Add(Client);
            await _context.SaveChangesAsync();

            //shows the Secret on index when the client gets created
            TempData["GeneratedSecret"] = randomSecret;

            return RedirectToPage("Index");
        }
    }
}
