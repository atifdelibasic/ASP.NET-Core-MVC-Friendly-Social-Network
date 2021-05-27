using DataLayer.EntityModels;
using FriendlyRS1.Repository.RepostorySetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriendlyRS1.Repository.Repositories
{
    public class UserRepository : Repository<ApplicationUser>
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public List<ApplicationUser> GetUsersByName(string search, int skip, int take)
        {
            
            List<ApplicationUser> users = _db.Users
                .Where(x => (x.FirstName.ToLower()+ " " + x.LastName).Contains(search.ToLower().Trim()))
                .Skip(skip)
                .Take(take)
                .ToList();

            return users;
        }
    }
}
