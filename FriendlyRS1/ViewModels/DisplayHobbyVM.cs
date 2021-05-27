using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FriendlyRS1.ViewModels
{
    public class DisplayHobbyVM
    {
        public List<SelectListItem> Hobbies{ get; set; }
        public int UserID { get; set; }
        public int HobbyID { get; set; }
        public bool IsMe { get; set; }
    }
}
