using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLayer.EntityModels;
using FriendlyRS1.Helper.Messages;
using FriendlyRS1.Repository.RepostorySetup;
using FriendlyRS1.Services;
using FriendlyRS1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FriendlyRS1.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        public IEmailSender _emailSender { get; set; }

        private readonly IUnitOfWork unitOfWork;
        private readonly string[] entites = new string[] { "Gender" };

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger, IEmailSender emailSender,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.unitOfWork = unitOfWork;
        }

        public async Task SendConfirmationToken(ApplicationUser user, string action, bool tokenType = false)
        {

            var token = tokenType ? await _userManager.GenerateEmailConfirmationTokenAsync(user) :
                await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action(
                  action, "Account",
                  new { user = tokenType ? user.Id.ToString() : user.Email, token = token },
                  protocol: Request.Scheme);

            string msg = tokenType ? Constants.Messages.EmailConfirmation  : Constants.Messages.PasswordReset;

            await _emailSender.SendEmailAsync(user.Email, "Social Platform Friendly",
                callbackUrl, msg);
        }

        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Feed");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                // if user's Email not confirmed send token 
                var user = await _userManager.FindByEmailAsync(model.Username);

                if (user != null && !user.EmailConfirmed && (await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    await SendConfirmationToken(user, "ConfirmEmail", true);

                    return View("EmailVerification");
                }

                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberPassword, true);

                if (result.IsLockedOut)
                {
                    TempData["message"] = LockoutExpirationMessage(user);
                    return View("AccountLocked");
                }

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Feed");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }
        public string LockoutExpirationMessage(ApplicationUser user)
        {
            int expirationMinutes = user.LockoutEnd.Value.Minute - DateTime.Now.Minute;
            if (expirationMinutes > 1)
                return "Lockout expires in " + expirationMinutes + " minutes";
            else if (expirationMinutes == 1)
                return "Lockout expires in " + expirationMinutes + " minute";
            else
                return "Lockout expires in less then minute";
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> LogOut()
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public List<RegisterViewModel.Row> GetAllGenders()
        {
            return unitOfWork.Gender.GetList(
               x => new RegisterViewModel.Row
               {
                   Id = x.Id,
                   GenderType = x.GenderType
               });
        }

        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            RegisterViewModel registerVM = new RegisterViewModel
            {
                Gender = GetAllGenders()
            };

            return View(registerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOdBirth,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    ActiveAccount = true,
                    GenderId = (int)model.GenderId,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    await SendConfirmationToken(user, "ConfirmEmail", true);

                    await _userManager.AddClaimAsync(user, new Claim("FirstName", user.FirstName));
                    await _userManager.AddClaimAsync(user, new Claim("Id", user.Id.ToString()));

                    return View("EmailVerification");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("name", error.Description);
                }
            }

            model.Gender = GetAllGenders();

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string user, string token)
        {
            if (user == null || token == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var _user = await _userManager.FindByIdAsync(user);

            if (_user == null)
            {
                return View("Index");
            }

            if (_user.EmailConfirmed)
            {
                TempData["confirmation_msg"] = "Your email is already confirmed";
            }
            else
            {
                var result = await _userManager.ConfirmEmailAsync(_user, token);

                if (result.Succeeded)
                {
                    TempData["confirmation_msg"] = "Email confirmed successfully!";
                }
                else
                {
                    return View("TokenExpired");
                }
            }

            return View("EmailConfirmMessage");
        }

        public IActionResult SendEmailAgain()
        {

            return View("ResendEmail");
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmail(ResendEmailVM resendModel)
        {
            if (!ModelState.IsValid || resendModel.Email == null)
                return View(resendModel);

            var user = await _userManager.FindByEmailAsync(resendModel.Email);

            if (user != null && !user.EmailConfirmed)
            {
                await SendConfirmationToken(user, "ConfirmEmail", true);
            }

            return View("EmailVerification");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await SendConfirmationToken(user, "ResetPassword", false);

                }
                return View("PasswordResetMsg");
            }
            return View(model);
        }

        public IActionResult ResetPassword(string user, string token)
        {
            if (user == null || token == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            ResetPasswordVM reset = new ResetPasswordVM { Email = user };

            return View(reset);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM reset)
        {
            if (!ModelState.IsValid)
            {
                // model not valid try again
                return View(reset);
            }

            var user = await _userManager.FindByEmailAsync(reset.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, reset.Token, reset.Password);
                if (result.Succeeded)
                {
                    return View("ResetPasswordConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(reset);
            }

            return View("PasswordError");
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            ViewData["returnUrl"] = returnUrl;
            return View();
        }

    }
}
