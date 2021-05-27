using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.EntityModels
{
    public class Hobby
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int? TagCount { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual int HobbyCategoryId { get; set; }
        [DisplayName("Hobby category")]
        public virtual HobbyCategory HobbyCategory { get; set; }
        //public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public List<ApplicationUserHobby> UserHobbies { get; set; }
    }
}
