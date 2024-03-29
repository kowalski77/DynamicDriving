﻿#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using DynamicDriving.Identity.Service.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace DynamicDriving.Identity.Service.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IEmailSender emailSender;
    private readonly IUserEmailStore<ApplicationUser> emailStore;
    private readonly ILogger<RegisterModel> logger;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IUserStore<ApplicationUser> userStore;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender)
    {
        this.userManager = userManager;
        this.userStore = userStore;
        this.emailStore = this.GetEmailStore();
        this.signInManager = signInManager;
        this.logger = logger;
        this.emailSender = emailSender;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public async Task OnGetAsync(string returnUrl = null)
    {
        this.ReturnUrl = returnUrl;
        this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= this.Url.Content("~/");
        this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (this.ModelState.IsValid)
        {
            var user = this.CreateUser();

            await this.userStore.SetUserNameAsync(user, this.Input.Email, CancellationToken.None);
            await this.emailStore.SetEmailAsync(user, this.Input.Email, CancellationToken.None);
            var result = await this.userManager.CreateAsync(user, this.Input.Password);

            if (result.Succeeded)
            {
                this.logger.LogInformation("User created a new account with password.");

                await this.userManager.AddToRoleAsync(user, Roles.User);

                var userId = await this.userManager.GetUserIdAsync(user);
                var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = this.Url.Page(
                    "/Account/ConfirmEmail",
                    null,
                    new { area = "Identity", userId, code, returnUrl },
                    this.Request.Scheme);

                await this.emailSender.SendEmailAsync(this.Input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl });
                }

                await this.signInManager.SignInAsync(user, false);
                return this.LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // If we got this far, something failed, redisplay form
        return this.Page();
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                                                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                "override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!this.userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }

        return (IUserEmailStore<ApplicationUser>)this.userStore;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
