﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.ViewModels
{
    public class HobbyVM
    {
        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [DisplayName("Hobby category")]
        public int HobbyCategoryId { get; set; }

    }
}
