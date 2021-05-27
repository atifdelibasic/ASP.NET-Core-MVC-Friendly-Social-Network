using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.EntityModels;
using FriendlyRS1.Helper.Messages;
using FriendlyRS1.Repository;
using FriendlyRS1.Repository.RepostorySetup;
using FriendlyRS1.SignalRChat.Hubs;
using FriendlyRS1.SignalRChat.Interface;
using FriendlyRS1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using NToastNotify;
using ReflectionIT.Mvc.Paging;

namespace FriendlyRS1.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationDbContext _db;

        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IHubContext<NotificationHubUser> _notificationUserHubContext;
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly IToastNotification _toastNotification;

        public UserProfileController(UserManager<ApplicationUser> userManager
           , IUnitOfWork unitOfWork, ApplicationDbContext db, IHubContext<NotificationHub> notificationHubContext, IHubContext<NotificationHubUser> notificationUserHubContext, IUserConnectionManager userConnectionManager
         , IToastNotification toastNotification)
        {
            _userManger = userManager;
            _db = db;
            this.unitOfWork = unitOfWork;
            _notificationHubContext = notificationHubContext;
            _notificationUserHubContext = notificationUserHubContext;
            _userConnectionManager = userConnectionManager;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index(int id)
        {
            bool me = false;
            ApplicationUser user = await _userManger.GetUserAsync(User);
            if (id == 0 || id == user.Id)
            {
                id = user.Id;
                me = true;
            }

            UserDetailsVM userDetails = unitOfWork.User.GetSingle(x => x.Id == id, x => new UserDetailsVM
            {
                UserId = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                ProfileImage = x.ProfileImage,
                CityName = x.City.Name,
                ConnectionsCount = unitOfWork.Friendship.CountConnections((int)id),
                Me = me ? true : false,
                LoggedUserId = user.Id,
                DateCreated = user.DateCreated,
                About = x.About
            });

            userDetails.HobbyCategories = unitOfWork.hobbyCategory.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            if (TempData["successMsg"] != null)
            {
                _toastNotification.AddSuccessToastMessage(TempData["successMsg"].ToString());
            }

            return View(userDetails);
        }

        [HttpGet]
        public IActionResult EditUserDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDetailsVM userVM = unitOfWork.User.GetSingle<UserDetailsVM>(x => x.Id == id,
                x => new UserDetailsVM
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DateOfBirth = x.DateOfBirth,
                    PhoneNumber = x.PhoneNumber,
                    UserId = x.Id,
                    About = x.About,
                    LoggedUserId = (int)id
                });



            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUserDetails(UserDetailsVM model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = unitOfWork.User.Find(model.UserId);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.DateOfBirth = model.DateOfBirth;
                user.About = model.About;


                unitOfWork.User.Update(user);
                unitOfWork.Complete();
            }

            MsgVM msg = new MsgVM
            {
                Message = Constants.Messages.SuccessEdit
            };

            TempData["successMsg"] = Constants.Messages.SuccessEdit;

            return RedirectToAction("Index", new { id = model.UserId });
        }



        public IActionResult GetUser(int id, int id2)
        {
            UserDetailsVM user = new UserDetailsVM { UserId = id, Me = id == id2 ? true : false };
            return View(user);
        }

        public IActionResult EditCity(int id)
        {
            string[] entities = { "City" };
            ApplicationUser user = unitOfWork.User.Get(x => x.Id == id, entities);

            CityVM model = new CityVM
            {
                UserId = user.Id,
                CityName = user.City?.Name,
                CountryId = user.City?.CountryId,
                PostalCode = user.City?.PostalCode
            };
            // create dropdwon menu 
            model.Countries = unitOfWork.Country.GetList(g => new SelectListItem
            {
                Text = g.Name + " - " + g.AlphaTwoCode,
                Value = g.Id.ToString(),
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCity(CityVM model)
        {
            if (!ModelState.IsValid) return RedirectToAction("EditCity", new { id = model.UserId });

            ApplicationUser user = unitOfWork.User.Get(x => x.Id == model.UserId, null);
            City city;

            City result = unitOfWork.City.Get(c => c.Name == model.CityName || c.PostalCode == model.PostalCode, null);

            if (result == null)
            {
                city = new City
                {
                    Name = model.CityName,
                    CountryId = (int)model.CountryId,
                    PostalCode = model.PostalCode
                };
                unitOfWork.City.Add(city);
            }
            else
            {
                city = result;
            }

            unitOfWork.Complete();

            user.CityId = city.Id;
            await _userManger.UpdateAsync(user);

            //redirect 
            return RedirectToAction("LivingPlaces", new { id = user.Id, successNotif = Constants.Messages.SuccessEdit });
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(IFormFile Image, string tekst)
        {
            if (IsValidImage(Image))
            {
                var fs1 = Image.OpenReadStream();
                var ms1 = new MemoryStream();

                fs1.CopyTo(ms1);
                byte[] p1 = ms1.ToArray();

                ApplicationUser user = await _userManger.FindByNameAsync(User.Identity.Name);
                user.ProfileImage = p1;

                await _userManger.UpdateAsync(user);
                unitOfWork.Complete();

                return RedirectToAction("Index");
            }

            //TempData["ImageError"] = Helper.Messages.Constants.Messages.InvalidFormat;
            _toastNotification.AddErrorToastMessage(Constants.Messages.InvalidFormat);

            return RedirectToAction("Index");
        }
        public bool IsValidImage(IFormFile image)
        {
            if (image != null)
            {
                string[] extensions = new[] { ".jpg", ".png" };
                string extension = Path.GetExtension(image.FileName);
                if (extensions.Contains(extension))
                    return true;
            }
            return false;
        }

        public async Task<IActionResult> UserProfile(int id)
        {
            if (id == 0) RedirectToAction("Index");

            var loggeduser = await _userManger.GetUserAsync(User);

            Friendship f = unitOfWork.Friendship.GetSingle(x => (x.User1Id == id && x.User2Id == loggeduser.Id) || (x.User1Id == loggeduser.Id && x.User2Id == id),
               x => new Friendship
               {
                   ActionUserId = x.ActionUserId,
                   User1Id = x.User1Id,
                   User2Id = x.User2Id
               });

            UserDetailsVM user = unitOfWork.User.GetSingle(x => x.Id == id, x => new UserDetailsVM
            {
                UserId = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CityName = x.City.Name,
                Gender = x.Gender.GenderType,
                CountryName = x.City.Country.Name,
                DateOfBirth = x.DateOfBirth,
                PhoneNumber = x.PhoneNumber,
                ProfileImage = x.ProfileImage,
                //  nece se prikazati edit button
                Me = false
            });

            return View(user);
        }

        public async Task<IActionResult> FriendConnect(int id)
        {
            var loggeduser = await _userManger.GetUserAsync(User);

            AddFriendVM user = unitOfWork.Friendship.GetSingle(x => (x.User1Id == id && x.User2Id == loggeduser.Id) || (x.User1Id == loggeduser.Id && x.User2Id == id),
               x => new AddFriendVM
               {
                   FriendshipId = x.Id,
                   Status = x.Status.Status,
                   ActionUserId = x.ActionUserId,
                   User1Id = x.User1Id,
                   User2Id = x.User2Id,
                   LoggedUserId = loggeduser.Id,
                   UserProfileId = id
               });

            if (user == null)
            {
                user = new AddFriendVM
                {
                    LoggedUserId = loggeduser.Id,
                    UserProfileId = id,
                    FriendshipId = 0
                };
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult AddFriend(AddFriendVM model)
        {
            Friendship f = unitOfWork.Friendship.Get(x => ((x.User1Id == model.LoggedUserId && x.User2Id == model.UserProfileId) ||
                (x.User2Id == model.LoggedUserId && x.User1Id == model.UserProfileId)), null);

            if (f == null)
            {
                f = new Friendship
                {
                    User1Id = model.LoggedUserId,
                    User2Id = model.UserProfileId,
                    ActionUserId = model.LoggedUserId,
                    StatusId = unitOfWork.FriendshipStatus.Get(x => x.Status == 0, null).Id
                };

                BellNotification notification = new BellNotification
                {
                    ActorId = model.LoggedUserId,
                    NotificationTypeId = 1,
                    NotifierId = model.UserProfileId,
                    DateCreated = DateTime.Now
                };
                unitOfWork.BellNotification.Add(notification);


                unitOfWork.Friendship.Add(f);
                unitOfWork.Complete();
            }
            return RedirectToAction("FriendConnect", new { id = model.UserProfileId });
        }

        [HttpPost]
        public IActionResult CancelRequest(AddFriendVM model)
        {
            Friendship f = unitOfWork.Friendship.Get(x => ((x.User1Id == model.LoggedUserId && x.User2Id == model.UserProfileId) ||
                (x.User2Id == model.LoggedUserId && x.User1Id == model.UserProfileId)), null);
            if (f != null)
            {
                BellNotification notification = unitOfWork.BellNotification.Get(x => (x.ActorId == model.LoggedUserId && x.NotifierId == model.UserProfileId) ||
                                                                                       x.ActorId == model.UserProfileId && x.NotifierId == model.LoggedUserId);

                if (notification != null)
                {
                    unitOfWork.BellNotification.Remove(notification);
                }

                unitOfWork.Friendship.Remove(f);
                unitOfWork.Complete();
            }
            return RedirectToAction("FriendConnect", new { id = model.UserProfileId });
        }

        [HttpPost]
        public IActionResult EstablishConnection(AddFriendVM model)
        {
            string[] a = { "Status" };
            // get friendship status
            Friendship f = unitOfWork.Friendship.Get(x => ((x.User1Id == model.LoggedUserId && x.User2Id == model.UserProfileId) ||
                 (x.User2Id == model.LoggedUserId && x.User1Id == model.UserProfileId)), a);
            // change to friends
            if (f != null)
            {
                f.StatusId = unitOfWork.FriendshipStatus.Get(x => x.Status == 1, null).Id;

                BellNotification notification = new BellNotification
                {
                    ActorId = model.LoggedUserId,
                    NotificationTypeId = 2,
                    NotifierId = model.UserProfileId,
                    DateCreated = DateTime.Now
                };
                unitOfWork.BellNotification.Add(notification);

                unitOfWork.Complete();
            }
            //save
            return RedirectToAction("FriendConnect", new { id = model.UserProfileId });
        }

        public async Task<IActionResult> ContactInfo(int? id)
        {
            if (id == null) return View("Index");

            ApplicationUser user = await _userManger.GetUserAsync(User);
            UserDetailsVM userDetails = unitOfWork.User.GetSingle(x => x.Id == id, x => new UserDetailsVM
            {
                UserId = x.Id,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Me = user.Id == id ? true : false
            });

            //razradit ovo malo za error
            //if (userDetails == null)
            //    return RedirectToAction("Index");

            return View(userDetails);
        }

        public async Task<IActionResult> LivingPlaces(int? id, string successNotif = null)
        {
            if (id == null) return View("Index");

            ApplicationUser user = await _userManger.GetUserAsync(User);
            UserDetailsVM userDetails = unitOfWork.User.GetSingle(x => x.Id == id, x => new UserDetailsVM
            {
                UserId = x.Id,
                CityName = x.City.Name,
                CountryName = x.City.Country.Name,
                Me = user.Id == id ? true : false
            });

            //razradit ovo malo za error
            //if (userDetails == null)
            //    return RedirectToAction("Index");
            if (successNotif != null)
            {
                _toastNotification.AddSuccessToastMessage(successNotif);
            }

            return View(userDetails);
        }
        public async Task<IActionResult> PersonalInfo(int? id)
        {
            if (id == null) return View("Index");

            ApplicationUser user = await _userManger.GetUserAsync(User);
            UserDetailsVM userDetails = unitOfWork.User.GetSingle(x => x.Id == id, x => new UserDetailsVM
            {
                UserId = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                Gender = x.Gender.GenderType,
                Me = user.Id == id ? true : false
            });

            //razradit ovo malo za error
            //if (userDetails == null)
            //    return RedirectToAction("Index");

            return View(userDetails);
        }

        public async Task<IActionResult> GetConnections(int? id, string searchString, int firstItem = 0)
        {
            var user = await _userManger.GetUserAsync(User);


            var model = new ConnectionsVM();
            model.Connections = unitOfWork.Friendship.
                GetConnections(x => new ConnectionsVM.Connection
                {
                    User1Id = x.User1Id,
                    User2Id = x.User2Id,
                    ActorId = x.ActionUserId,
                    Id = x.Id,
                    User = x.User1Id != id ? x.User1 : x.User2
                }, (int)id, firstItem, 10, searchString);

            model.LoggedUser = user.Id;


            return View(model);
        }
        public List<SelectListItem> GetHobbies()
        {
            DisplayHobbyVM obj = new DisplayHobbyVM();
            List<Hobby> hobbies = unitOfWork.hobby.GetAll(new[] { "HobbyCategory" }).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach (var hobby in hobbies)
            {
                selectListItems.Add(new SelectListItem(hobby.Title, hobby.Id.ToString()));
            }
            return selectListItems;
        }

        public async Task<IActionResult> DisplayHobbies()
        {
            var loggedUser = await _userManger.GetUserAsync(User);
            DisplayHobbyVM obj = new DisplayHobbyVM();

            obj.Hobbies = GetHobbies();

            int userId = Int32.Parse((User.FindFirst("Id").Value));
            List<ApplicationUserHobby> list = unitOfWork.UserHobby.GetList(x => new ApplicationUserHobby
            {
                ApplicationUser = x.ApplicationUser,
                ApplicationUserId = x.ApplicationUserId,
                Hobby = x.Hobby,
                HobbyId = x.HobbyId,
                Date = x.Date
            }, x => x.ApplicationUserId == userId);

            list.Sort((x, y) => x.Date.CompareTo(y.Date));
            ViewData["Hobbies"] = list;

            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> AddHobbyToUser(DisplayHobbyVM input)
        {
            int userId = Int32.Parse(User.FindFirst("Id").Value);
            input.Hobbies = GetHobbies();

            List<ApplicationUserHobby> lista = unitOfWork.UserHobby.GetSelect(x => x.ApplicationUserId == userId, new[] { "Hobby" }).ToList();
            if (input.HobbyID == 0)
            {

                ViewData["Hobbies"] = lista;
                TempData["ErrorMsg"] = "Please select one";
                //ModelState.Clear();
                return RedirectToAction("DisplayHobbies");
            }

            ApplicationUserHobby hobby = unitOfWork.UserHobby.Get(x => x.ApplicationUserId == userId && x.HobbyId == input.HobbyID);
            if (hobby != null)
            {
                ViewData["Hobbies"] = lista;
                TempData["ErrorMsg"] = "Hobby already choosen";
                //ModelState.Clear();
                return RedirectToAction("DisplayHobbies");
            }

            ApplicationUserHobby obj = new ApplicationUserHobby()
            {
                Date = DateTime.Now,
                HobbyId = input.HobbyID,
                ApplicationUserId = userId
            };

            unitOfWork.UserHobby.Add(obj);
            unitOfWork.Complete();

            List<ApplicationUserHobby> list = unitOfWork.UserHobby.GetSelect(x => x.ApplicationUserId == userId, new[] { "Hobby" }).ToList();

            ViewData["Hobbies"] = list;
            input.HobbyID = 0;
            ModelState.Clear();
            return RedirectToAction("DisplayHobbies");
        }

        [HttpGet]
        public IActionResult DeleteHobbyFromUser(int id)
        {
            int userId = Int32.Parse(User.FindFirst("Id").Value);
            ApplicationUserHobby userHobby = unitOfWork.UserHobby.Get(x => x.ApplicationUserId == userId && x.HobbyId == id, null);

            unitOfWork.UserHobby.Remove(userHobby);
            unitOfWork.Complete();

            return RedirectToAction("DisplayHobbies");
        }

        public IActionResult SearchConnections() => View();
        public async Task<IActionResult> Testiranje(AddFriendVM model)
        {
            try
            {
                var loggedUser = await _userManger.GetUserAsync(User);
                var connections = _userConnectionManager.GetUserConnections(model.UserProfileId.ToString());

                if (connections != null)
                {
                    foreach (var connectionId in connections)
                    {
                        await _notificationUserHubContext.Clients.Client(connectionId).SendAsync("sendToUser", loggedUser.ToString(), model.NotificationMsg, model.LoggedUserId, loggedUser?.ProfileImage);
                    }
                }
            }
            catch { }


            return new EmptyResult();
        }

    }
}