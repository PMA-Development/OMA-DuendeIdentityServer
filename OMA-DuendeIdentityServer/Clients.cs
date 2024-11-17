using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace OMA_DuendeIdentityServer
{
    internal class Clients
    {
        public static IEnumerable<Client> Get()
        {
            int seconds = 8 * 3600;
            return new List<Client>
            {
                 new Client
                {

                    ClientId = "OMA-Web",
                    ClientName = "Example Client Application",
                    AccessTokenLifetime = seconds,
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:7123/authentication/login-callback",
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:7123/authentication/logout-callback",
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                    },
                    AllowedCorsOrigins = { "https://localhost:7123" },
                    RequirePkce = true,
                    AllowPlainTextPkce = false
                },
                 new Client
{
                    ClientId = "OMA-Maui",
                    ClientName = "MAUI Client Application",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequireClientSecret = false,
                    RedirectUris = { "myapp://auth" },
                    PostLogoutRedirectUris = { "myapp://auth" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role"
                    },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly, 
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AccessTokenLifetime = seconds


                }

            };

        }
    }
}
