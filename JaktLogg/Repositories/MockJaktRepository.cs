using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

namespace JaktLogg
{
	public class MockJaktRepository : IJaktRepository
	{
		public List<Jakt> JaktList = new List<Jakt>();
		public List<Jeger> JegerList = new List<Jeger>();
		public List<Art> ArtList = new List<Art>();
		private List<Logg> LoggList = new List<Logg>();
		//private List<ArtGroup> ArtGroupList = new List<ArtGroup>();
		
		public MockJaktRepository ()
		{
			ArtList.Add(new Art{ ID = 1, Navn = "Rype" });
			ArtList.Add(new Art{ ID = 2, Navn = "Tiur" });
			ArtList.Add(new Art{ ID = 3, Navn = "Røy" });
			ArtList.Add(new Art{ ID = 4, Navn = "Orrhane" });
			
			LoggList.Add(new Logg{ Skudd = 2, Treff = 1, ArtId = 2, JaktId = 1 });
			LoggList.Add(new Logg{ Skudd = 3, Treff = 0, ArtId = 1, JaktId = 1});
			LoggList.Add(new Logg{ Skudd = 1, Treff = 0, ArtId = 2, JaktId = 1});
			LoggList.Add(new Logg{ Skudd = 1, Treff = 0, ArtId = 3, JaktId = 2});
			LoggList.Add(new Logg{ Skudd = 1, Treff = 0, ArtId = 2, JaktId = 3});
			LoggList.Add(new Logg{ Skudd = 1, Treff = 0, ArtId = 1, JaktId = 3});
			
			JegerList.Add(new Jeger{ ID= 1, Fornavn = "Tore", Etternavn = "Mørkved" });
			JegerList.Add(new Jeger{ ID= 2, Fornavn = "Alex", Etternavn = "York" });
			JegerList.Add(new Jeger{ ID= 3, Fornavn = "Donald", Etternavn = "Duck" });
			JegerList.Add(new Jeger{ ID= 4, Fornavn = "Daffy", Etternavn = "Duck" });
			JegerList.Add(new Jeger{ ID= 5, Fornavn = "Mikke", Etternavn = "Mus" });
			JegerList.Add(new Jeger{ ID= 6, Fornavn = "Ole", Etternavn = "Brumm" });
			
			JaktList.Add(new Jakt{ID= 1, Sted = "Lierne"});
			JaktList.Add(new Jakt{ID= 2, Sted = "Høylandet", DatoFra = DateTime.Now.AddDays(-1)});
			JaktList.Add(new Jakt{
				ID= 3, 
				Sted = "Frolfjellet", 
				DatoFra = DateTime.Now.AddDays(-2),
				JegerIds = new List<int>()
			});
		}
		
		#region IJaktRepository implementation
		
		public List<Jakt> GetAllJaktItems()
		{	
			return JaktList;
		}
		public List<Jeger> GetAllJegerItems()
		{
			return JegerList;
		}
		public List<Art> GetAllArtItems()
		{
			return ArtList;
		}
		
		public void SaveJaktList (List<Jakt> jaktlist)
		{
			JaktList = jaktlist;
			Console.WriteLine("----- Saved jaktlist ------");
			foreach(var item in jaktlist){
				Console.WriteLine("Jakt: " + item.Sted);
			}
		   Console.WriteLine("----- --------------- ------");     
			
			

		}

		public void SaveJegerList (List<Jeger> jegerlist)
		{
			JegerList = jegerlist;
			Console.WriteLine("----- Saved jegerlist ------");
			foreach(var item in jegerlist){
				Console.WriteLine("jeger: " + item.Navn);
			}
		   Console.WriteLine("----- --------------- ------");
			
			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string filePath = Path.Combine(path, "jegere.xml");
			Console.WriteLine(filePath);
			XmlSerializer serializer = new XmlSerializer( typeof(List<Jeger>) );
	        TextWriter writer = new StreamWriter(filePath);
	        serializer.Serialize(writer, JegerList);
		}

		public void SaveArtList (List<Art> artlist)
		{
			ArtList = artlist;
			Console.WriteLine("----- Saved artlist ------");
			foreach(var item in artlist){
				Console.WriteLine("art: " + item.Navn);
			}
		   Console.WriteLine("----- --------------- ------");
			
			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string filePath = Path.Combine(path, "arter.xml");
			Console.WriteLine(filePath);
			XmlSerializer serializer = new XmlSerializer( typeof(List<Art>) );
	        TextWriter writer = new StreamWriter(filePath);
	        serializer.Serialize(writer, ArtList);
		}
		
		public List<Logg> GetAllLoggItems ()
		{
			return LoggList;
		}

		public void SaveLoggList (List<Logg> loggList)
		{
			LoggList = loggList;
			
			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string filePath = Path.Combine(path, "logger.xml");
			Console.WriteLine(filePath);
			XmlSerializer serializer = new XmlSerializer( typeof(List<Logg>) );
	        TextWriter writer = new StreamWriter(filePath);
	        serializer.Serialize(writer, LoggList);
		}
		public void SaveSelectedArtIdList(List<int> items){
			throw new NotImplementedException();
		}
		#endregion
	}
}

