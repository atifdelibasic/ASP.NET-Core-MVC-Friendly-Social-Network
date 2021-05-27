using DataLayer.EntityModels;
using FriendlyRS1.Repository.RepostorySetup;
using FriendlyRS1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public SearchController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;

        }
        private const int take = 6;
        public async Task<IActionResult> Index(string q = "")
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            QueryVM obj = new QueryVM
            {
                LoggedUserId = loggedUser.Id,
                q = q
            };

            return View("Index", obj);
        }

        public async Task<IActionResult> GetPeople(int id, string q, int firstItem = 0)
        {

            UserVM model = new UserVM();
            if (!string.IsNullOrEmpty(q))
            {
                List<ApplicationUser> users = _unitOfWork.User.GetUsersByName(q, firstItem, take);

                model = new UserVM
                {
                    Users = users.Select(x => new UserVM.Row
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        ProfileImage = x.ProfileImage,
                        IsMe = x.Id == id || id == 0 ? true : false
                    }).ToList()
                };
            }

            if ((model.Users == null || model.Users.Count == 0) && firstItem > take)
            {
                return new EmptyResult();
            }

            return View(model);
        }

    }
}
