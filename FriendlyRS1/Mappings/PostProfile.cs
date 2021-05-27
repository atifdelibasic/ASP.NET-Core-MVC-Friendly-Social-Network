using AutoMapper;
using DataLayer.EntityModels;
using FriendlyRS1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.Mappings
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, ShowPostVM>();
        }
    }
}
