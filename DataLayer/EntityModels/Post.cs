using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.EntityModels
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public int HobbyId { get; set; }
        [ForeignKey("HobbyId")]
        public Hobby Hobby { get; set; }
        public string Text { get; set; }
      
    }
}
