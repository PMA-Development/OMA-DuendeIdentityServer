using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;

namespace OMA_DuendeIdentityServer.Pages.ClientManagement
{
    public class IndexModel : PageModel
    {
        private readonly ConfigurationDbContext _context;

        public IndexModel(ConfigurationDbContext context)
        {
            _context = context;
        }

        public List<Client> Clients { get; set; }

        public async Task OnGetAsync()
        {
            Clients = await _context.Clients.ToListAsync();
        }
    }
}
