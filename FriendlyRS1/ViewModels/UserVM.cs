using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.ViewModels
{
    public class UserVM
    {
        public class Row
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public byte[] ProfileImage { get; set; }
            public bool IsMe { get; set; }
        }
        public int LoggedUserId { get; set; }
        public bool ShowConnectBtn { get; set; }
        public List<Row> Users { get; set; }
    }
}
