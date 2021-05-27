using System;

namespace FriendlyRS1.Services
{
    public class EmailSenderOptions
    {
        public string SenderEmail { get; set; }

        public string SendGridName { get; set; }
        public string SendGridKey { get; set; }
    }
}
