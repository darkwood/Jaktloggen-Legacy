using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace JaktLogg
{
	public class Jakt
	{
		public int ID;
		public string Sted = string.Empty;
		public DateTime DatoFra = DateTime.Now;
		public DateTime DatoTil = DateTime.Now;
		public List<int> JegerIds = new List<int>();
		public List<int> DogIds = new List<int>();
		public string Latitude;
		public string Longitude;
		public string ImagePath = string.Empty;
		public string Notes = string.Empty;
		
		public Jakt ()
		{
		}
	}
}

