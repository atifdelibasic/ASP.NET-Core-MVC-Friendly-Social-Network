using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.EntityModels
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AlphaTwoCode { get; set; }
        public string AlphaThreeCode { get; set; }
    }
}
