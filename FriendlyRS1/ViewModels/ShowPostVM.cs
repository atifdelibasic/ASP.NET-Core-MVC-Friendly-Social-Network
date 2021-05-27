using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.ViewModels
{
    public class ShowPostVM
    {
        public int Id { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "4 characters minimum")]
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public byte[] ProfileImage { get; set; }
        public int AuthorId { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public DateTime DateCreated { get; set; }
        public string Date { get; set; }
        public string Hobby { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsMe { get; set; }
    }
}
