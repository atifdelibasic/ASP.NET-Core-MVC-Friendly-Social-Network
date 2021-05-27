using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FriendlyRS1.Models;
using Microsoft.AspNetCore.Identity;
using DataLayer.EntityModels;
using NToastNotify;
using FriendlyRS1.Helper.Messages;

namespace FriendlyRS1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IToastNotification _toastNotification;

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            IToastNotification toastNotification)
        {
            _logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {

            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Feed");

            _toastNotification.AddInfoToastMessage(Constants.Messages.Welcome, new NToastNotify.NotyOptions() { 
                Timeout = 2000
            });
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
