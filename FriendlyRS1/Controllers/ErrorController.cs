using FriendlyRS1.Helper.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace FriendlyRS1.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IToastNotification _toastNotification;
        public ErrorController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {

            _toastNotification.AddErrorToastMessage(Constants.Messages.Error);

            return View();
        }
    }
}
