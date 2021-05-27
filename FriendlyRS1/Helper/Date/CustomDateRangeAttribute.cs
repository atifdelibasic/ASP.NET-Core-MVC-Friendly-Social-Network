using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.Helper.Date
{
    public class CustomDateRangeAttribute : ValidationAttribute
    {
        public string Minimum { get; set; }
        public string Maximum { get; set; }

        public CustomDateRangeAttribute(int min, int max)
        {
            this.Minimum = DateTime.Now.AddYears(-min).ToString();
            this.Maximum = DateTime.Now.AddYears(-max).ToString();
        }

        public override bool IsValid(object value)
        {
            string date = value.ToString();
            DateTime parsedValue = DateTime.Parse(date);

            return DateTime.Parse(Minimum) < parsedValue && parsedValue < DateTime.Parse(Maximum);
        }
    }
}
