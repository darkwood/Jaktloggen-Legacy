using System;
namespace JaktLogg
{
	public class Jeger
	{
		public int ID;
		public string Fornavn = string.Empty;
		public string Etternavn = string.Empty;
		public string Email = string.Empty;
		public string Phone = string.Empty;
		public string ImagePath = string.Empty;
		public bool IsMe;
		public string Navn {
			get{
				return Fornavn + " " + Etternavn;
			}
		}
		public Jeger ()
		{
		}
	}
}

