using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataLayer.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using FriendlyRS1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FriendlyRS1.Repository;
using FriendlyRS1.Repository.RepostorySetup;

namespace FriendlyRS1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HobbyController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UnitOfWork unitOfWork;
        private readonly string[] entites = new string[] { "HobbyCategory" };
        public HobbyController(ApplicationDbContext context)
        {
            this.context = context;
            unitOfWork = new UnitOfWork(context);
        }

        public IActionResult Create()
        {
            ViewData["hobbyCategories"] = GetListItemsHobbyCategory();
            return View();
        }

        [HttpPost]
        public IActionResult Create(HobbyVM input)
        {
            if (ModelState.IsValid)
            {
                if (input.Id != null)
                {
                    Hobby hobby = unitOfWork.hobby.Get(x => x.Id == input.Id, null);

                    hobby.Title = input.Title;
                    hobby.Description = input.Description;
                    hobby.HobbyCategoryId = input.HobbyCategoryId;
                    hobby.ModifiedDate = DateTime.Now;
                }
                else
                {
                    Hobby hobby = new Hobby
                    {
                        Title = input.Title,
                        Description = input.Description,
                        HobbyCategoryId = input.HobbyCategoryId
                    };
                    unitOfWork.hobby.Add(hobby);
                }

                unitOfWork.Complete();
                //TempData["hobbies"] = GetHobbies();
                return RedirectToAction("DisplayAll");
            }

            ViewData["hobbyCategories"] = GetListItemsHobbyCategory();
            return View(input);
        }

        public IActionResult DisplayAll()
        {
            TempData["hobbies"] = GetHobbies();
            return View();
        }

        public IActionResult Get(int id)
        {
            Hobby hobby = unitOfWork.hobby.Get(x => x.Id == id, new string[] { "HobbyCategory" });

            return View(hobby);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Hobby hobby = unitOfWork.hobby.Get(x => x.Id == id, null);
            if (hobby != null)
            {
                context.Hobby.Remove(hobby);
                unitOfWork.Complete();
            }

            TempData["hobbies"] = GetHobbies();
            return View();
        }

        public IActionResult Edit(int id)
        {
            Hobby hobby = unitOfWork.hobby.Get(x => x.Id == id, null);

            HobbyVM hobbyVM = new HobbyVM
            {
                Id = hobby.Id,
                Description = hobby.Description,
                Title = hobby.Title,
                HobbyCategoryId = hobby.HobbyCategoryId
            };

            ViewData["hobbyCategories"] = GetListItemsHobbyCategory();
            return View("Create", hobbyVM);
        }

        public List<Hobby> GetHobbies()
        {
            return unitOfWork.hobby.GetAll(entites).ToList();
        }
        public List<HobbyCategory> GetHobbyCategories()
        {
            return (List<HobbyCategory>)unitOfWork.hobbyCategory.GetAll();
        }
        public List<SelectListItem> GetListItemsHobbyCategory()
        {
            List<HobbyCategory> hobbyCategories = GetHobbyCategories();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in hobbyCategories)
            {
                selectListItems.Add(new SelectListItem(item.Name, item.Id.ToString()));
            }
            return selectListItems;
        }
    }
}
