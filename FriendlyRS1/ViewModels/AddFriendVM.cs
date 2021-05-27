using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.ViewModels
{
    public class AddFriendVM
    {
        public int FriendshipId { get; set; }
        public Friendship friendship { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public byte Status { get; set; }
        public int LoggedUserId { get; set; }
        public int UserProfileId { get; set; }
        public int ActionUserId { get; set; }

        public string NotificationMsg { get; set; }
    }
}
