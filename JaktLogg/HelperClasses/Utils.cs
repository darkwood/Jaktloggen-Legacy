using System;
using System.IO;
using MonoTouch.UIKit;
using System.Globalization;
using MonoTouch.CoreAnimation;

namespace JaktLogg
{
	public static class Utils
	{
		public static int HUNTYEAR_STARTMONTH = 4;
		
		public static string UppercaseFirst(string s)
	    {
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
			    return string.Empty;
			}
			// Return char and concat substring.
			return char.ToUpper(s[0]) + s.Substring(1).ToLower();
	    }
		
		public static DateTime NSDateToDateTime(MonoTouch.Foundation.NSDate date)
		{
		    return (new DateTime(2001,1,1,0,0,0)).AddSeconds(date.SecondsSinceReferenceDate);
		}
		
		public static MonoTouch.Foundation.NSDate DateTimeToNSDate(DateTime date)
		{
		    return MonoTouch.Foundation.NSDate.FromTimeIntervalSinceReferenceDate((date-(new DateTime(2001,1,1,0,0,0))).TotalSeconds);
		}
		
		public static string GetPath(string filename){
			return Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), filename);
				
		}
		/*public static UIImage GetUIImageFromFile(string imagePath, bool useFallback = true)
		{
			var path = "Images/Icons/pictureframe.png";
			
			if(!string.IsNullOrEmpty(imagePath)){
				string longFilePath = Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), imagePath);
				if(File.Exists(longFilePath))
					path = longFilePath;
				else if(imagePath.StartsWith("Images/"))
					path = imagePath;
			}	
			else if(!useFallback)
				return null;
			
			return new UIImage(path);
		}
		*/
		
	}
}

