using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMA_DuendeIdentityServer.Pages.ClientManagement
{
    public class EditModel : PageModel
    {
        private readonly ConfigurationDbContext _context;

        public EditModel(ConfigurationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Client Client { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Client = await _context.Clients.FindAsync(id);
            if (Client == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Clients.Update(Client);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}

