using DataLayer.EntityModels;
using FriendlyRS1.Helper.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.ViewModels
{
    public class UserDetailsVM
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = Constants.Messages.Required)]
        [Display(Name = "First Name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} should contain minimum {2} letters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Constants.Messages.Required)]
        [Display(Name = "Last Name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} should contain minimum {2} letters")]
        [RegularExpression(@"^[a-zA-ZĐđŠšŽžČčĆć]+$", ErrorMessage = Constants.Messages.LettersOnly)]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Display(Name = "Birth date")]
        public DateTime DateOfBirth { get; set; }
        public DateTime DateCreated { get; set; }
        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"(^[0-9]{9,})$", ErrorMessage = "Invalid Mobile Number.")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Gender")]
        public char Gender { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public bool Me { get; set; } = false;
        public int ConnectionsCount { get; set; }
        public int LoggedUserId { get; set; }
        [MaxLength(50)]
        public string About { get; set; } 
        public List<SelectListItem> HobbyCategories { get; set; }
        public List<SelectListItem> Hobbies { get; set; }
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
        public string GetAge()
        {

            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (this.DateOfBirth.Year * 100 + this.DateOfBirth.Month) * 100 + this.DateOfBirth.Day;

            return $"{(a - b) / 10000} years";
        }
        public byte[] ProfileImage { get; set; }
    }
}
