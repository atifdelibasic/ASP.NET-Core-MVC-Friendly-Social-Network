using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.EntityModels
{
    public class Address
    {
        [Required]
        public int Id { get; set; }
        public string PostalCode { get; set; }
        public string Address1 { get; set; }
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }
    }
}
