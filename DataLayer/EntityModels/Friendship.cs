using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.EntityModels
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }
        public int User1Id { get; set; }
        [ForeignKey("User1Id")]
        public ApplicationUser User1 { get; set; }
        public int User2Id { get; set; }
        [ForeignKey("User2Id")]
        public ApplicationUser User2 { get; set; }
        public int ActionUserId { get; set; }
        [ForeignKey("ActionUserId")]
        public ApplicationUser ActionUser { get; set; }
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public FriendshipStatus Status { get; set; }
    }
}
