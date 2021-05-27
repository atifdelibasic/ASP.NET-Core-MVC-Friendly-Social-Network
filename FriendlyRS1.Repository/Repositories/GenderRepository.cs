using DataLayer.EntityModels;
using FriendlyRS1.Repository.RepostorySetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendlyRS1.Repository.Repositories
{
    public class GenderRepository: Repository<Gender>
    {
        public GenderRepository(ApplicationDbContext context):base(context)
        {

        }
    }
}
