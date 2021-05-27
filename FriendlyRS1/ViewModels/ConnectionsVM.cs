using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.ViewModels
{
    public class ConnectionsVM
    {
        public class Connection
        {
            public int Id { get; set; }
            public int User1Id { get; set; }
            public int User2Id { get; set; }
            public int ActorId { get; set; }
            public string Name { get; set; }
            public ApplicationUser User { get; set; }
        }
        public List<ConnectionsVM.Connection> Connections { get; set; }
        public int LoggedUser { get; set; }
        public string  Search { get; set; }
    }
}
