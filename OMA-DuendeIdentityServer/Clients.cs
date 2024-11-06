using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace OMA_DuendeIdentityServer
{
    internal class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "oauthClient",
                    ClientName = "Example client application using client credentials",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())}, // change me!
                    AllowedScopes = new List<string> {"api1.read"}
                },
                new Client
                {
                    ClientId = "testID2",
                    ClientName = "Example Client Application",
                    ClientSecrets = new List<Secret> {new Secret("dVy0aMiQtQso/DErlqKtvlQHYFKUtWW0x5gczU8C6Cs=".Sha256())}, // change me!
    
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string> {"https://localhost:7176/signin-oidc"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "api1.read"
                    },
                    RequirePkce = true,
                    AllowPlainTextPkce = false
                },
                 new Client
                {

                    ClientId = "OMA-Web",
                    ClientName = "Example Client Application",

                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string> {"https://localhost:7123/authentication/login-callback"},
                      PostLogoutRedirectUris = new List<string> { "https://localhost:7123/authentication/logout-callback" }, // Must match exactly
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                    },
                    RequirePkce = true,
                    AllowPlainTextPkce = false
                },
                 new Client
{
                    ClientId = "OMA-Maui",
                    ClientName = "MAUI Client Application",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RedirectUris = { "myapp://" },
                    PostLogoutRedirectUris = { "myapp://" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api.read",
                        "role"
                    },

                    AllowOfflineAccess = true, // Enables the use of refresh tokens
                    RefreshTokenUsage = TokenUsage.OneTimeOnly, // Recommended setting for better security
                    RefreshTokenExpiration = TokenExpiration.Sliding

                }

            };

        }
    }
}
