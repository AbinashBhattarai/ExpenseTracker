using ExpenseTracker.Models;
using ExpenseTracker.Service;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mailService = mailService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            LoginViewModel loginVM = new()
            {
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(loginVM);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            loginVM.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.Email);
                if(user != null)
                {
                    if (!user.EmailConfirmed)
                    {
                        ModelState.AddModelError(string.Empty, "Please confirm email first.");
                        return View(loginVM);
                    }

                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View(loginVM);
            }
            return View(loginVM);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(registerVM.Email);
                if (user != null)
                {
                    ModelState.AddModelError("Email", "Email already registered. Proceed to login");
                    return View(registerVM);
                }

                var newUser = new AppUser
                {
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    Email = registerVM.Email,
                    UserName = registerVM.Email
                };

                var result = await _userManager.CreateAsync(newUser, registerVM.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, token = token }, Request.Scheme);

                    MailRequest mailRequest = new()
                    {
                        ToEmail = newUser.Email,
                        Subject = "Confirmation Mail",
                        Body = $"Please click <a href='{confirmationLink}'>this link</a> to confirm your email" 
                    };
                    
                    await _mailService.SendEmailAsync(mailRequest);
                    return View("ConfirmEmail");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity!.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            LoginViewModel loginVM = new LoginViewModel();
            loginVM.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["error"] = "Error loading external login information.";
                return View("Login", loginVM);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            AppUser user = null;

            if (email != null)
            {
                user = await _userManager.FindByEmailAsync(email);

                if(user == null)
                {
                    user = new AppUser
                    {
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                        UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                    var result = await _userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                        MailRequest mailRequest = new()
                        {
                            ToEmail = user.Email,
                            Subject = "Confirmation Mail",
                            Body = $"Please click <a href='{confirmationLink}'>this link</a> to confirm your email"
                        };

                        await _userManager.AddLoginAsync(user, info);
                        await _mailService.SendEmailAsync(mailRequest);
                        return View("ConfirmEmail");
                    }
                    ModelState.AddModelError("", "Invalid Login Attempt");
                    return View("Login", loginVM);
                }
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Please confirm email first.");
                    return View("Login", loginVM);
                }

                var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View("Login", loginVM);
            }
            TempData["error"] = $"Email claim not received from: {info.LoginProvider}";
            return View("Login", loginVM); 
        }


        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["error"] = $"The User ID {userId} is invalid";
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            
            if (result.Succeeded)
            {
                return View("ConfirmEmailSuccess");
            }

            return View("ConfirmEmailError");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                    MailRequest mailRequest = new()
                    {
                        ToEmail = model.Email,
                        Subject = "Reset Password",
                        Body = $"Please click <a href='{passwordResetLink}'>this link</a> to reset your password."
                    };
                    await _mailService.SendEmailAsync(mailRequest);
                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);

            var userHasPassword = await _userManager.HasPasswordAsync(user);

            if (!userHasPassword)
            {
                return RedirectToAction("AddPassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> AddPassword()
        {
            var user = await _userManager.GetUserAsync(User);

            var userHasPassword = await _userManager.HasPasswordAsync(user);

            if (userHasPassword)
            {
                return RedirectToAction("ChangePassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);

                return View("AddPasswordConfirmation");
            }

            return View(model);
        }

    }
}
