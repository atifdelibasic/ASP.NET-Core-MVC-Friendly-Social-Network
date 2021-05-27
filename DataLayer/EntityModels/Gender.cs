using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.EntityModels
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public char GenderType { get; set; }
    }
}
