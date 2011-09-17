using System;
using System.Collections.Generic;

namespace JaktLogg
{
	public class Logg
	{
		public int ID;
		public int Treff;
		public int Skudd;
		public int Sett;
		public DateTime Dato = DateTime.Now;
		public int ArtId;
		public int JegerId;
		public int JaktId;
		public string Latitude = string.Empty;
		public string Longitude = string.Empty;
		public string ImagePath = string.Empty;
		public string Gender = string.Empty;
		public string Notes = string.Empty;
		public string Weather = string.Empty;
		public string Age = string.Empty;
		public int Weight;
		public int Tagger;
		
		public Logg ()
		{
			
		}
		public Logg (int jaktId)
		{
			JaktId = jaktId;
		}
	}
}

