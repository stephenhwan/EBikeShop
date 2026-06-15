using EBikeShop.MVC.Data.Entities.Identity;
using EBikeShop.MVC.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Text;
using System.Text.Encodings.Web;

namespace EBikeShop.MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<BikeIdentityUser> _signInManager;
        private readonly UserManager<BikeIdentityUser> _userManager;
        private readonly IUserStore<BikeIdentityUser> _userStore;
        private readonly IUserEmailStore<BikeIdentityUser> _emailStore;
        private readonly ILogger<RegisterVM> _logger;
        //private readonly IEmailSender _emailSender;


        public AuthenticationController(
            UserManager<BikeIdentityUser> userManager,
            IUserStore<BikeIdentityUser> userStore,
            SignInManager<BikeIdentityUser> signInManager,
            ILogger<RegisterVM> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            var loginVM = new LoginVM
            {
                ReturnUrl = returnUrl
            };
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            //TODO: Implement login logic here (e.g., validate credentials, sign in user, etc.)
            var returnUrl = Url.Content(loginVM.ReturnUrl);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = loginVM.RememberMe });
                    return RedirectToAction("LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = loginVM.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    //return View(loginVM);
                }
            }
            return View(loginVM);
        }


        public IActionResult Register(string returnUrl = "/")
        {
            var loginVM = new RegisterVM
            {
                ReturnUrl = returnUrl
            };
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            //TODO: Implement login logic here (e.g., validate credentials, sign in user, etc.)
            var returnUrl = Url.Content(registerVM.ReturnUrl);
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, registerVM.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, registerVM.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(registerVM.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToAction("RegisterConfirmation", new { email = registerVM.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(registerVM);
        }

        public async Task<IActionResult> RegisterConfirmation(string email, string returnUrl = "/")
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            var registerConfirmationVM = new RegisterConfirmationVM
            {
                Email = email,
                DisplayConfirmAccountLink = true,

            };
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            if (registerConfirmationVM.DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                registerConfirmationVM.EmailConfirmationUrl = Url.Action(
                    "ConfirmEmail", "Authentication",
                    new { userId = userId, code = code, returnUrl = returnUrl });
            }

            return View(registerConfirmationVM);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code, string returnUrl = "/")
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);

            }
            return LocalRedirect(returnUrl);
        }


        private BikeIdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<BikeIdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(BikeIdentityUser)}'. " +
                    $"Ensure that '{nameof(BikeIdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<BikeIdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<BikeIdentityUser>)_userStore;
        }



    }
}
