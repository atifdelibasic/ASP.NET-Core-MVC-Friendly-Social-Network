using DataLayer.EntityModels;
using FriendlyRS1.Repository;
using FriendlyRS1.Repository.RepostorySetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repositories
{
    public class ApplicationUserHobbyRepository:Repository<ApplicationUserHobby>
    {
        public ApplicationUserHobbyRepository(ApplicationDbContext context):base(context)
        {

        }
    }
}
