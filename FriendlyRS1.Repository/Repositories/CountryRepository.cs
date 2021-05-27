using FriendlyRS1.Repository;
using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using FriendlyRS1.Repository.RepostorySetup;

namespace FriendlyRS1.Repository.Repositories
{
    public class CountryRepository:Repository<Country>
    {
        public CountryRepository(ApplicationDbContext context):base(context)
        {

        }
    }
}
