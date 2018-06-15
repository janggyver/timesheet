using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheet.HelperMethods
{
    public class TimeHelper
    {
        public static DateTime FindStartDayOfWeek(DateTime dt)
        {
            DateTime today = dt;
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                today = today.AddDays(-7);
            }
            int dayOfToday = (int)today.DayOfWeek;

            return today.AddDays((-1 * dayOfToday) + 1).Date;
        }

        public static DateTime StartDateOfMonth(DateTime dt)
        {
            int yearOfToday = DateTime.Today.Year;
            int monthOfToday = DateTime.Today.Month;
            var startDateOfThisMonth = new DateTime(yearOfToday, monthOfToday, 1);
            return startDateOfThisMonth;
        }

        public static DateTime LastDateOfMonth(DateTime dt)
        {
            var LastDateOfThisMonth = StartDateOfMonth(DateTime.Today).AddMonths(1).AddDays(-1);
            return LastDateOfThisMonth;
        }
    }
}
