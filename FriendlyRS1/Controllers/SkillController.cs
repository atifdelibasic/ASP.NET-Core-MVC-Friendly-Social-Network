using DataLayer.EntityModels;
using FriendlyRS1.Repository.RepostorySetup;
using FriendlyRS1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.Controllers
{
    public class SkillController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SkillController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult GetSkills()
        {

            List<SkillVM.Row> skills = _unitOfWork.Skill.GetList<SkillVM.Row>(x => new SkillVM.Row
            {
                id = x.Id,
                name = x.Name,
                description = x.Description
            });

            SkillVM model = new SkillVM
            {
                skills = skills,
                total = skills.Count()
                
            };
            return Ok(model);
        }

        public IActionResult DeleteSkill(int? id)
        {
            if (id != null)
            {
                Skill skill = _unitOfWork.Skill.Find((int)id);
                _unitOfWork.Skill.Remove(skill);
                _unitOfWork.Complete();
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult Update([FromBody] SkillVM.Row skillModel)
        {
            if (skillModel != null)
            {
                Skill skill = _unitOfWork.Skill.Find((int)skillModel.id);
                skill.Description = skillModel.description;
                skill.Name = skillModel.name;
                skill.DateModified = DateTime.Now;

                _unitOfWork.Skill.Update(skill);
                _unitOfWork.Complete();
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult Insert([FromBody] SkillVM.Row skillModel)
        {
            Skill skill = new Skill
            {
                Description = skillModel.description,
                Name = skillModel.name,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            _unitOfWork.Skill.Add(skill);
            _unitOfWork.Complete();

            return Ok();
        }

        public IActionResult Delete(int id)
        {
            Skill skill = _unitOfWork.Skill.Find(id);
            if (skill != null)
            {
                _unitOfWork.Skill.Remove(skill);
                _unitOfWork.Complete();
            }


            return Ok();
        }

        public IActionResult Paginate(string search, int currentPage= 1, int itemsPerPage= 5)
        {

            List<Skill> s = _unitOfWork.Skill.SearchSkills(search, currentPage, itemsPerPage);


            SkillVM model = new SkillVM
            {
                skills = s.Select(x => new SkillVM.Row
                {
                    id = x.Id,
                    description = x.Description,
                    name = x.Name
                }).ToList(),
                total = _unitOfWork.Skill.Count()
            };
            


            return Ok(model);
        }
    }
}
