using DataLayer.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.ViewModels
{
    public class CityVM
    {
        public int UserId { get; set; }
        public int? CountryId { get; set; }
        [Required]
        public string CityName { get; set; }
        [Required]
        //[StringLength(5, ErrorMessage = "Error", MinimumLength = 5)]
        public string PostalCode { get; set; }
        public List<SelectListItem> Countries { get; set; }
    }
}
