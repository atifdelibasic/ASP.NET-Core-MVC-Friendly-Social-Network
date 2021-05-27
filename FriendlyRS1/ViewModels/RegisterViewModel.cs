using FriendlyRS1.Helper.Date;
using FriendlyRS1.Helper.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = Constants.Messages.Required)]
        [Display(Name = "First Name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} should contain minimum {2} letters")]
        [RegularExpression(@"^[a-zA-ZĐđŠšŽžČčĆć]+$", ErrorMessage = Constants.Messages.LettersOnly)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Constants.Messages.Required)]
        [Display(Name = "Last Name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} should contain minimum {2} letters")]
        [RegularExpression(@"^[a-zA-ZĐđŠšŽžČčĆć]+$", ErrorMessage = Constants.Messages.LettersOnly)]
        public string LastName { get; set; }

        [Required(ErrorMessage = Constants.Messages.BrithDateReq)]
        [Display(Name = "Birth date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [CustomDateRange(100, 6 ,ErrorMessage = Constants.Messages.BirthDateErr)]
        public DateTime DateOdBirth { get; set; }

        [Required(ErrorMessage = Constants.Messages.Required)]
        [EmailAddress(ErrorMessage = Constants.Messages.EmailErr)]
        public string Email { get; set; }


        [Required(ErrorMessage = Constants.Messages.Required)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = Constants.Messages.Required)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = Constants.Messages.PasswordMatch)]
        public string ConfirmPassword { get; set; }

        public class Row
        {
            public int Id { get; set; }
            public char GenderType { get; set; }
        }

        [Required(ErrorMessage = Constants.Messages.Required)]
        public int? GenderId { get; set; }
        public List<Row> Gender { get; set; }
        public string[] Genders { get; set; } = new[] { "Male", "Female", "Other" };

    }
}
