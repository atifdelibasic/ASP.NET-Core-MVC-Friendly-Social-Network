using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateModified { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public bool ActiveAccount { get; set; }
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public virtual Gender Gender { get; set; }
        public int? CityId { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }
        public byte[] ProfileImage { get; set; }
        //public ICollection<Hobby> Hobbies { get; set; }
        public string About { get; set; }
        public List<ApplicationUserHobby> UserHobbies { get; set; }
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
