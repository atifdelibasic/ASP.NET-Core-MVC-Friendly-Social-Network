using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FriendlyRS1.SignalRChat.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FriendlyRS1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        public AdminController(IHubContext<NotificationHub> notificationHubContext)
        {
            _notificationHubContext = notificationHubContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        //public async Task<IActionResult> Index(Article model)
        //{
        //    await _notificationHubContext.Clients.All.SendAsync("sendToUser", model.articleHeading, model.articleContent);
        //    return View();
        //}
        public IActionResult Page() => View();
    }
}
