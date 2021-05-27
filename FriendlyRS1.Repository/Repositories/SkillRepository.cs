using DataLayer.EntityModels;
using FriendlyRS1.Repository.RepostorySetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriendlyRS1.Repository.Repositories
{
    public class SkillRepository : Repository<Skill>
    {
        private ApplicationDbContext _context;
        public SkillRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Skill> SearchSkills(string search, int currentPage, int itemsPerPage)
        {
            List<Skill> skills = _context.Skill.Where(x => search == null || x.Name.StartsWith(search) || x.Description.StartsWith(search))
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            return skills;
        }
    }
}
