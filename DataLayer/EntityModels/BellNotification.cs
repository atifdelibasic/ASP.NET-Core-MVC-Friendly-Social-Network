using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.EntityModels
{
    public class BellNotification
    {
        [Key]
        public int Id { get; set; }
        public int? ActorId { get; set; }
        public ApplicationUser Actor { get; set; }
        public int NotifierId { get; set; }
        [ForeignKey("NotifierId")]
        public ApplicationUser Notifier { get; set; }
        public int NotificationTypeId { get; set; }
        [ForeignKey("NotificationTypeId")]
        public NotificationType NotificationType { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }
    }
}
