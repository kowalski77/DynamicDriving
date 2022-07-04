// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using DynamicDriving.Identity.Service.Entities;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DynamicDriving.Identity.Service.Areas.Identity.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IIdentityServerInteractionService interactionService;

    public LogoutModel(SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService interactionService)
    {
        this.signInManager = signInManager;
        this.interactionService = interactionService;
    }

    public async Task<IActionResult> OnGet(string logoutId)
    {
        var context = await this.interactionService.GetLogoutContextAsync(logoutId).ConfigureAwait(false);
        
        return !context.ShowSignoutPrompt ? 
            await this.OnPost(context.PostLogoutRedirectUri).ConfigureAwait(false) : 
            this.Page();
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await this.signInManager.SignOutAsync().ConfigureAwait(false);
        
        if (returnUrl != null)
        {
            return this.Redirect(returnUrl);
        }
        else
        {
            return this.RedirectToPage();
        }
    }
}
