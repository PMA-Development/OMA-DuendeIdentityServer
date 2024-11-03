using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace OMA_DuendeIdentityServer
{
    
    public class AccountController
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
             IIdentityServerInteractionService interaction,
             IClientStore clientStore,
             IAuthenticationSchemeProvider schemeProvider,
             IEventService events,
             SignInManager<IdentityUser> signInManager)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;

            _signInManager = signInManager;
        }

    }
}
