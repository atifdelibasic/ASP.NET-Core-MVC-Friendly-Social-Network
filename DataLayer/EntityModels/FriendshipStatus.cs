using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.EntityModels
{
    public class FriendshipStatus
    {
        [Key]
        public int Id { get; set; }
        public byte Status { get; set; }
    }
}
