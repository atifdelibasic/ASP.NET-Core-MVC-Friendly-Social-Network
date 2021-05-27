using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.EntityModels
{
    public class NotificationType
    {
        [Key]
        public int Id { get; set; }
        public string NotificationDescription { get; set; }
        public string NotificationMessage { get; set; }
    }
}
