using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.ViewModels
{
    public class NotificationVM
    {
        public int Id { get; set; }
        public string NotificationMessage { get; set; }
        public string Actor { get; set; }
        public byte[] ProfileImage { get; set; }
        public string Date { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateCreated { get; set; }
        public int? ActorId { get; set; }
    }
}
