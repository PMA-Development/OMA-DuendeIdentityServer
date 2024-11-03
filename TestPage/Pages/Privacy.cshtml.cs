using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace TestPage.Pages
{
    [Authorize]
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        public string IdToken { get; private set; }
        public string AccessToken { get; private set; }
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public async void OnGetAsync()
        {

            //ONLY USED FOR DEBUGGING OF TOKEN
            // Retrieve tokens
            //IdToken = await HttpContext.GetTokenAsync("id_token");
            //AccessToken = await HttpContext.GetTokenAsync("access_token");

            //// can decode and inspect these tokens as needed
            //if (IdToken != null)
            //{
            //    // Decode and inspect the ID token
            //    var handler = new JwtSecurityTokenHandler();
            //    var idTokenDecoded = handler.ReadJwtToken(IdToken);
            //    // Use 'idTokenDecoded.Claims' to view the claims
            //}

            //if (AccessToken != null)
            //{
            //    // Decode and inspect the Access token
            //    var handler = new JwtSecurityTokenHandler();
            //    var accessTokenDecoded = handler.ReadJwtToken(AccessToken);
            //    // Use 'accessTokenDecoded.Claims' to view the claims
            //}
        }
    }
}


