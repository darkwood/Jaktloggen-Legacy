using System;
using System.Globalization;
namespace JaktLogg
{
	public static class ExtensionMethods
	{
		public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' }, 
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }
		public static string ToLocalDateString(this DateTime dt)
        {
            var ci = Utils.GetCurrentCultureInfo();
			return dt.ToString("d. MMMM", ci);
        }
		
		public static string ToLocalDateAndYearString(this DateTime dt)
        {
            var ci = Utils.GetCurrentCultureInfo();
			return dt.ToString("d. MMMM yyyy", ci);
        }
		
		public static string ToLocalTimeString(this DateTime dt)
        {
            var ci = Utils.GetCurrentCultureInfo();
			return dt.ToString("HH:mm", ci);
        }
	}
}

