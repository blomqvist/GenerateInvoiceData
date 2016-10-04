using System;

namespace Common
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateString(this DateTime dateTime)
        {
            return dateTime.ToString("d");
        }
    }
}
