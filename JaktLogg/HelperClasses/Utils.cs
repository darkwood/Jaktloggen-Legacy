using System;
using System.IO;
using MonoTouch.UIKit;
using System.Globalization;
using MonoTouch.CoreAnimation;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

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
			if(char.IsUpper(s[0]))
				return s;

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
		
		public static string Translate(string key){
			var lang = JaktLoggApp.instance.CurrentLanguage;
			var xmlfile = string.Format("Language/{0}.xml", lang.ToString());
			if(!File.Exists(xmlfile))
				xmlfile = "Language/Norwegian.xml";
			
			var stream = new FileStream(xmlfile, FileMode.Open, FileAccess.Read);
			var xml = XDocument.Load(stream);
			if(xml == null)
				return "Xml is null";
			
			var val = xml.Descendants(key).FirstOrDefault();
			if(val == null || val.Value == null)
				return "["+key+"]";
			
			return val.Value;
		}
		
		public static CultureInfo GetCurrentCultureInfo(){
			CultureInfo ci = new CultureInfo("nb-NO");
			if(JaktLoggApp.instance.CurrentLanguage == Language.English)
				ci = new CultureInfo("en-US");
			
			return ci;
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

