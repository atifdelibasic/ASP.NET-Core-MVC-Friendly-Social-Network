using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.Helper
{
    public static class DateTimeCalculator
    {
        public static string CalcTime(DateTime dateCreated)
        {
            TimeSpan time = DateTime.Now.Subtract(dateCreated);
            string dateTime = null;
            if (isBigger(time.Days, 8)) dateTime = dateCreated.ToShortDateString();
            else if (isBigger(time.Days)) dateTime = time.Days.ToString() + " day";
            else if (isBigger(time.Hours)) dateTime = time.Hours.ToString() + " hour";
            else if (isBigger(time.Minutes)) dateTime = time.Minutes.ToString() + " minute";
            else if (isBigger(time.Seconds)) dateTime = time.Seconds.ToString() + " second";
            else dateTime = time.Milliseconds.ToString() + " ms";

            if (!dateTime.StartsWith("1 ") && !dateTime.Contains("/")) { dateTime += "s"; }
            if (!dateTime.Contains("/")) dateTime += " ago";

            return dateTime;
        }
        public static bool isBigger(int num1, int num2 = 0)
        {
            return num1 > num2;
        }
    }
}
