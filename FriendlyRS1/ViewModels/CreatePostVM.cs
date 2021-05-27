using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.ViewModels
{
    public class CreatePostVM
    {
        public int Id { get; set; }
        [Required]
        public string PostText { get; set; }
        [Required(ErrorMessage = "Please select hobby.")]
        public int? HobbyId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int AuthorId { get; set; }
        public string HobbyTitle { get; set; }
    }
}
