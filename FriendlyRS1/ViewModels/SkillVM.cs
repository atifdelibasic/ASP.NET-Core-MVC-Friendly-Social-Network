using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.ViewModels
{
    public class SkillVM
    {
        public int total { get; set; } = 0;
        public List<Row> skills { get; set; }
        public class Row
        {
            public int? id { get; set; }
            public string description { get; set; }
            public string name { get; set; }
        }

        
    }
}
