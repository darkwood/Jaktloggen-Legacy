using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace JaktLogg
{
	public static class ImageUtility
	{
		public static List<string> GetImagePaths(){
			
			var items = new List<string>();
			foreach(var item in JaktLoggApp.instance.JaktList){
				var file = Utils.GetPath("jakt_" + item.ID + ".jpg");
				if(File.Exists(file))
					items.Add(file);
			}
			foreach(var item in JaktLoggApp.instance.LoggList){
				var file = Utils.GetPath("jaktlogg_" + item.ID + ".jpg");
				if(File.Exists(file))
					items.Add(file);
			}
			foreach(var item in JaktLoggApp.instance.JegerList){
				var file = Utils.GetPath("jeger_" + item.ID + ".jpg");
				if(File.Exists(file))
					items.Add(file);
			}
			foreach(var item in JaktLoggApp.instance.DogList){
				var file = Utils.GetPath("dog_" + item.ID + ".jpg");
				if(File.Exists(file))
					items.Add(file);
			}
			return items;	
		}
		public static int GetNumberOfPictures()
		{
			return GetImagePaths().Count();
		}
		
		public static int GetTotalPictureSize()
		{
			int size = 0;
			var items = GetImagePaths();
			foreach(var item in items){
				size += (int) new FileInfo(item).Length;
			}
			return size;	
		}
	}
}

