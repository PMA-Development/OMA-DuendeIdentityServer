// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMA_DuendeIdentityServer.Pages.Logout;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;

    [BindProperty] 
    public string? LogoutId { get; set; }

    public Index(IIdentityServerInteractionService interaction, IEventService events)
    {
        _interaction = interaction;
        _events = events;
    }

    public async Task<IActionResult> OnGet(string? logoutId)
    {
        LogoutId = logoutId;

        var showLogoutPrompt = LogoutOptions.ShowLogoutPrompt;

        if (User.Identity?.IsAuthenticated != true)
        {
            // if the user is not authenticated, then just show logged out page
            showLogoutPrompt = false;
        }
        else
        {
            var context = await _interaction.GetLogoutContextAsync(LogoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                showLogoutPrompt = false;
            }
        }
            
        if (showLogoutPrompt == false)
        {
            // if the request for logout was properly authenticated from IdentityServer, then
            // we don't need to show the prompt and can just log the user out directly.
            return await OnPost();
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            LogoutId ??= await _interaction.CreateLogoutContextAsync();

            // Clear cookies
            ClearAuthenticationCookies();

            await HttpContext.SignOutAsync();
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        }

        return RedirectToPage("/Account/Logout/LoggedOut", new { logoutId = LogoutId });
    }
    private void ClearAuthenticationCookies()
    {
        Response.Cookies.Delete(".AspNetCore.Identity.Application");
        Response.Cookies.Delete("idsrv");
    }


}
