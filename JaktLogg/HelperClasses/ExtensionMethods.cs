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
		public static string ToNorwegianDateString(this DateTime dt)
        {
            CultureInfo ci = new CultureInfo("nb-NO");
			return dt.ToString("d. MMMM", ci);
        }
		
		public static string ToNorwegianDateAndYearString(this DateTime dt)
        {
            CultureInfo ci = new CultureInfo("nb-NO");
			return dt.ToString("d. MMMM yyyy", ci);
        }
		
		public static string ToNorwegianTimeString(this DateTime dt)
        {
            CultureInfo ci = new CultureInfo("nb-NO");
			return dt.ToString("HH:mm", ci);
        }
	}
}

