using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataLayer.EntityModels;
using FriendlyRS1.Helper;
using FriendlyRS1.Helper.Messages;
using FriendlyRS1.Repository.RepostorySetup;
using FriendlyRS1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using NToastNotify;

namespace FriendlyRS1.Controllers
{
    [Authorize]
    public class FeedController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        public FeedController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper, IToastNotification toastNotification)
        {
            _userManager = userManager;
            this.unitOfWork = unitOfWork;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            List<SelectListItem> hobbyCategories = unitOfWork.hobbyCategory.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            UserDetailsVM userVM = new UserDetailsVM
            {
                UserId = loggedUser.Id,
                FirstName = loggedUser.FirstName,
                LastName = loggedUser.LastName,
                ProfileImage = loggedUser.ProfileImage,
                HobbyCategories = hobbyCategories
            };

            return View(userVM);
        }
        // uzme sve usere iz baze i predlozi ih u feed
        // samo za test :)
        public async Task<IActionResult> FriendSuggestion()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            var a = unitOfWork.User.GetAll();

            var users = _userManager.Users.Where(u => u.Id != applicationUser.Id).Select(x => new ApplicationUser
            {
                Id = x.Id,
                City = x.City,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ProfileImage = x.ProfileImage
            }).ToList();


            return View(users);
        }

        public async Task<IActionResult> Page() => View();

        public IActionResult GetData(int id)
        {
            var db = unitOfWork.hobby.GetAll().Where(x => x.HobbyCategoryId == id);
            return Json(new { data = db });
        }

        [HttpPost]
        public IActionResult AddPost(CreatePostVM model)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return Json(new { success = false, issue = model, errors = ModelState.Values.Where(i => i.Errors.Count > 0), msg = message });
            }

            Post post;
            if (model.Id == 0)
            {
                post = new Post
                {
                    Text = model.PostText,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    AuthorId = model.AuthorId,
                    HobbyId = (int)model.HobbyId
                };
                _toastNotification.AddSuccessToastMessage(Constants.Messages.PostAdd);
                unitOfWork.Post.Add(post);
            }
            else
            {
                post = unitOfWork.Post.Get(x => x.Id == model.Id);
                post.Text = model.PostText;
                post.HobbyId = (int)model.HobbyId;
                post.ModifiedDate = DateTime.Now;

                _toastNotification.AddSuccessToastMessage(Constants.Messages.SuccessEdit);
                unitOfWork.Post.Update(post);
            }

            // insert new post to db
            unitOfWork.Complete();

            if (model.Id == 0)
            {
                List<ShowPostVM> list = unitOfWork.Post.GetList<ShowPostVM>(y => new ShowPostVM
                {
                    AuthorName = y.Author.ToString(),
                    ProfileImage = y.Author.ProfileImage,
                    DateCreated = y.CreatedDate,
                    Hobby = y.Hobby.Title,
                    Text = y.Text,
                    Id = y.Id,
                    IsMe = true
                }, x => x.Id == post.Id);

                return View("LoadConnectionsPosts", list);
            }



            model.HobbyTitle = unitOfWork.hobby.Get(x => x.Id == model.HobbyId).Title;

            return Json(model);
        }

        private const int TAKE_NOTIF = 10;
        public async Task<IActionResult> GetNotifications(string sortOrder, string searchString, int firstItem = 0)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            List<BellNotification> bn = unitOfWork.BellNotification.GetNotifications(applicationUser.Id, TAKE_NOTIF, firstItem);

            unitOfWork.BellNotification.MarkAsRead();

            //ViewData["read_notif_count"] = readNotifications;

            if (bn.Count() > 0)
            {
                List<NotificationVM> n = bn.Select(x => new NotificationVM
                {
                    Id = x.Id,
                    NotificationMessage = x.NotificationType.NotificationMessage,
                    Actor = x.Actor?.ToString(),
                    ProfileImage = x.Actor?.ProfileImage,
                    Date = DateTimeCalculator.CalcTime(x.DateCreated),
                    IsRead = x.IsRead,
                    DateCreated = x.DateCreated,
                    ActorId = x.Actor?.Id
                }).ToList();
                return View(n);

            }

            return new EmptyResult();
        }


        public async Task<IActionResult> MarkAsReadTest()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            List<BellNotification> n = unitOfWork.BellNotification.GetList(x => new BellNotification
            {
                Id = x.Id,
                Actor = x.Actor,
                NotificationType = x.NotificationType,
                DateCreated = x.DateCreated,
                IsRead = x.IsRead,
                ActorId = x.ActorId,
                NotificationTypeId = x.NotificationTypeId,
                NotifierId = x.NotifierId
            }, x => x.NotifierId == applicationUser.Id).ToList();


            foreach (BellNotification item in n)
            {
                item.IsRead = false;
                unitOfWork.BellNotification.Update(item);
            }

            unitOfWork.Complete();

            return RedirectToAction("Index");
        }

        private const int BATCH_SIZE = 5;
        [HttpPost]
        public async Task<IActionResult> _TestData(string sortOrder, string searchString, int firstItem = 0)
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            List<Post> posts = unitOfWork.Post.ConectionsPosts(loggedUser.Id, BATCH_SIZE, firstItem);

            List<ShowPostVM> testData = posts.OrderByDescending(x => x.CreatedDate).Select(x => new ShowPostVM
            {
                Id = x.Id,
                Text = x.Text,
                AuthorName = x.Author.ToString(),
                ProfileImage = x.Author.ProfileImage,
                AuthorId = x.Author.Id,
                Date = DateTimeCalculator.CalcTime(x.CreatedDate),
                Hobby = x.Hobby.Title,
                DateTime = x.CreatedDate,
                DateCreated = x.CreatedDate,
                IsMe = loggedUser.Id != x.AuthorId ? false : true
            }).ToList();

            return View("LoadConnectionsPosts", testData);
        }

        public async Task<IActionResult> CountNotifications()
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            int count = unitOfWork.BellNotification.CountUnreadNotifications(loggedUser.Id);
            return Json(count);
        }

        public async Task<IActionResult> GetNearbyPosts(int id, double lat1, double long1, int firstItem = 0)
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            List<Hobby> hobbies = unitOfWork.UserHobby.GetList<Hobby>(x => new Hobby
            {
                Id = x.HobbyId
            }, x => x.ApplicationUserId == loggedUser.Id);

            List<Post> posts = unitOfWork.Post.NearbyPost(loggedUser.Id, lat1, long1, hobbies, firstItem, BATCH_SIZE);

            if (posts != null)
            {
                List<ShowPostVM> p = new List<ShowPostVM>();
                foreach (Post post in posts)
                {
                    ApplicationUser u = unitOfWork.User.Get(x => x.Id == post.AuthorId);
                    Hobby h = unitOfWork.hobby.Get(x => x.Id == post.HobbyId);
                    p.Add(new ShowPostVM
                    {
                        AuthorName = u.ToString(),
                        ProfileImage = u.ProfileImage,
                        Date = DateTimeCalculator.CalcTime(post.CreatedDate),
                        Text = post.Text,
                        AuthorId = post.AuthorId,
                        Hobby = h.Title,
                        DateCreated = post.CreatedDate
                    });
                }
                return View("LoadConnectionsPosts", p);
            }

            return NotFound();
        }

        public async Task<IActionResult> GetMyPosts(int id, int firstItem = 0)
        {
            List<Post> posts = unitOfWork.Post.GetMyPosts(id, firstItem, 5);
            var loggedUser = await _userManager.GetUserAsync(User);

            List<ShowPostVM> p = posts.Select(x => new ShowPostVM
            {
                Id = x.Id,
                AuthorName = x.Author.ToString(),
                ProfileImage = x.Author.ProfileImage,
                Date = DateTimeCalculator.CalcTime(x.CreatedDate),
                Text = x.Text,
                AuthorId = x.AuthorId,
                Hobby = x.Hobby.Title,
                DateCreated = x.CreatedDate,
                IsMe = loggedUser.Id == x.AuthorId ? true : false
            }).ToList();


            return View("LoadConnectionsPosts", p);
        }

        [HttpPost]
        public IActionResult SearchPeople(string search, int skip = 0, int take = 5)
        {
            if (string.IsNullOrEmpty(search))
                return new EmptyResult();

            List<ApplicationUser> users = unitOfWork.User.GetUsersByName(search, skip, take);

            UserVM model = new UserVM
            {
                Users = users.Select(x => new UserVM.Row
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ProfileImage = x.ProfileImage
                }).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult GetPost(int? id)
        {

            Post post = unitOfWork.Post.Get(x => x.Id == id, new string[] { "Hobby" });

            return Json(post);
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            Post p = unitOfWork.Post.Get(x => x.Id == id);
            if (p != null)
            {
                unitOfWork.Post.Remove(p);
                unitOfWork.Complete();
                _toastNotification.AddErrorToastMessage(Constants.Messages.PostDelete);
                return Json(p);
            }
            return new EmptyResult();
        }

    }
}
