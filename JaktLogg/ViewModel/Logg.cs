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
		public string Notes = string.Empty;
		
		private string _gender = string.Empty;
		public string Gender  { get{ return _gender; } set{ _gender = value; } }
		
		private string _weather = string.Empty;
		public string Weather { get{ return _weather; } set{ _weather = value; } }
		
		private string _age = string.Empty;
		public string Age { get{ return _age; } set{ _age = value; } }
			
		private string _weapontype = string.Empty;
		public string WeaponType { get{ return _weapontype; } set{ _weapontype = value; } }
		
		private string _dog = string.Empty;
		public string Dog { get{ return _dog; } set{ _dog = value; } }
		
		
		public int Weight { get; set; }
		public int ButchWeight { get; set; }
		public int Tags { get; set; }
		
		public Logg ()
		{
			
		}
		public Logg (int jaktId)
		{
			JaktId = jaktId;
		}
	}
}

