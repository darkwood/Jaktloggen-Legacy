using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Text;

namespace JaktLogg
{
	public class FileJaktRepository
	{
		private static string FILE_JAKT = "jakt.xml";
		private static string FILE_SELECTED_ARTIDS = "selectedartids.xml";
		private static string FILE_SELECTED_LOGGTYPEIDS = "selectedloggtypeids.xml";
		private static string FILE_ART = "arter.xml";
		private static string FILE_MY_ART = "myspecies.xml";
		private static string FILE_ARTGROUP = "artgroup.xml";
		private static string FILE_JEGER = "jegere.xml";
		private static string FILE_DOG = "dogs.xml";
		private static string FILE_LOGG = "logger.xml";
		private static string FILE_LOGGTYPER = "loggtyper.xml";
		private static string FILE_LOGGTYPE_GROUP = "loggtypegroup.xml";
		
		private List<Jakt> JaktList = new List<Jakt>();
		private List<Jeger> JegerList = new List<Jeger>();
		private List<Dog> DogList = new List<Dog>();
		private List<Logg> LoggList = new List<Logg>();
		
		private List<Art> ArtList = new List<Art>();
		private List<ArtGroup> ArtGroupList = new List<ArtGroup>();
		private List<int> SelectedArtIdList = new List<int>();
		
		private List<LoggType> LoggTypeList = new List<LoggType>();
		private List<LoggTypeGroup> LoggTypeGroupList = new List<LoggTypeGroup>();
		private List<string> SelectedLoggTypeIdList = new List<string>();
		
		
		private string path;
		public FileJaktRepository(){
			path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
		}
	
		#region GET lists
		
		public List<Jakt> GetAllJaktItems ()
		{
			string filePath = Path.Combine(path, FILE_JAKT);
			XmlSerializer serializer = new XmlSerializer( typeof(List<Jakt>) );
			if(!File.Exists(filePath)){
				JaktList = new List<Jakt>();
				return JaktList;
			}
	        FileStream stream = new FileStream(filePath, FileMode.Open);
			if(stream.Length > 0)
				JaktList = (List<Jakt>) serializer.Deserialize(stream);
			else
				JaktList = new List<Jakt>();
			
			stream.Close();
			return JaktList;
		}

		public List<Jeger> GetAllJegerItems ()
		{
			string filePath = Path.Combine(path, FILE_JEGER);
			if(!File.Exists(filePath)){
				JegerList = new List<Jeger>();
				return JegerList;
			}
			XmlSerializer serializer = new XmlSerializer( typeof(List<Jeger>) );
	        FileStream stream = new FileStream(filePath, FileMode.Open);
			if(stream.Length > 0)
				JegerList = (List<Jeger>) serializer.Deserialize(stream);
			else
				JegerList = new List<Jeger>();
			
			stream.Close();
			return JegerList;
		}
		
		public List<Dog> GetAllDogItems ()
		{
			string filePath = Path.Combine(path, FILE_DOG);
			if(!File.Exists(filePath)){
				DogList = new List<Dog>();
				return DogList;
			}
			XmlSerializer serializer = new XmlSerializer( typeof(List<Dog>) );
	        FileStream stream = new FileStream(filePath, FileMode.Open);
			if(stream.Length > 0)
				DogList = (List<Dog>) serializer.Deserialize(stream);
			else
				DogList = new List<Dog>();
			
			stream.Close();
			return DogList;
		}

		public List<int> GetSelectedArtIdList ()
		{	
			string filePath = Path.Combine(path, FILE_SELECTED_ARTIDS);
			if(!File.Exists(filePath)){
				
				return SelectedArtIdList;
			}
			XmlSerializer serializer = new XmlSerializer( typeof(List<int>) );
			FileStream stream = new FileStream(filePath, FileMode.Open);
			if(stream.Length > 0)
				SelectedArtIdList = (List<int>) serializer.Deserialize(stream);
			else
				SelectedArtIdList = new List<int>();
			
			stream.Close();
			return SelectedArtIdList;
		}
		
		public List<string> GetSelectedLoggTypeIdList ()
		{	
			string filePath = Path.Combine(path, FILE_SELECTED_LOGGTYPEIDS);
			if(!File.Exists(filePath)){
				
				return SelectedLoggTypeIdList;
			}
			XmlSerializer serializer = new XmlSerializer( typeof(List<string>) );
			FileStream stream = new FileStream(filePath, FileMode.Open);
			if(stream.Length > 0)
				SelectedLoggTypeIdList = (List<string>) serializer.Deserialize(stream);
			else
				SelectedLoggTypeIdList = new List<string>();
			
			stream.Close();
			return SelectedLoggTypeIdList;
		}
		
		public List<Art> GetAllArtItems ()
		{	
			string filePath = Path.Combine(path, FILE_ART);
			string myArtPath = Path.Combine(path, FILE_MY_ART);

			//if(!File.Exists(filePath)){
				File.Copy("Data/arter.xml", filePath, true);
			//}
			
			XmlSerializer serializer = new XmlSerializer( typeof(List<Art>) );
			//get all standard species
			var stream = new FileStream(filePath, FileMode.Open);
			if(stream.Length > 0)
				ArtList = (List<Art>) serializer.Deserialize(stream);

			stream.Close();

			//get my species list
			if(File.Exists(filePath))
			{
				stream = new FileStream(filePath, FileMode.Open);
				if(stream.Length > 0){
					var myspecies = (List<Art>) serializer.Deserialize(stream);
					ArtList.AddRange(myspecies);

				}
			}

			stream.Close();
			return ArtList;
		}
		
		public List<ArtGroup> GetAllArtGroupItems ()
		{
			/*string filePath = Path.Combine(path, FILE_ARTGROUP);
			if(!File.Exists(filePath))
			{
				File.Copy("Data/artgroup.xml", filePath, true);
			}*/
				
			XmlSerializer serializer = new XmlSerializer( typeof(List<ArtGroup>) );
			FileStream stream = new FileStream("Data/artgroup.xml", FileMode.Open);
			if(stream.Length > 0)
				ArtGroupList = (List<ArtGroup>) serializer.Deserialize(stream);
			else
				ArtGroupList = new List<ArtGroup>();
			
			stream.Close();
			return ArtGroupList;
		}
		
		public List<LoggTypeGroup> GetAllLoggtypeGroupItems ()
		{
			string filePath = Path.Combine(path, FILE_LOGGTYPE_GROUP);
			//if(!File.Exists(filePath))
			//{
				File.Copy("Data/loggtypegroup.xml", filePath, true);
			//}
				
			XmlSerializer serializer = new XmlSerializer( typeof(List<LoggTypeGroup>) );
			FileStream stream = new FileStream(filePath, FileMode.Open);
			if(stream.Length > 0)
				LoggTypeGroupList = (List<LoggTypeGroup>) serializer.Deserialize(stream);
			else
				LoggTypeGroupList = new List<LoggTypeGroup>();
			
			stream.Close();
			return LoggTypeGroupList;
		}
		
		public List<Logg> GetAllLoggItems ()
		{
			string filePath = Path.Combine(path, FILE_LOGG);
			if(!File.Exists(filePath)){
				LoggList = new List<Logg>();
				return LoggList;
			}
			XmlSerializer serializer = new XmlSerializer( typeof(List<Logg>) );
	        FileStream stream = new FileStream(filePath, FileMode.Open);
			if(stream.Length > 0)
				LoggList = (List<Logg>) serializer.Deserialize(stream);
			else
				LoggList = new List<Logg>();
			
			stream.Close();
			
			return LoggList;
		}
		
		public List<LoggType> GetAllLoggTypeItems ()
		{	
			string filePath = Path.Combine(path, FILE_LOGGTYPER);
			//if(!File.Exists(filePath))
			//{
				File.Copy("Data/loggtyper.xml", filePath, true);
			//}
			
			XmlSerializer serializer = new XmlSerializer( typeof(List<LoggType>) );
			FileStream stream = new FileStream(filePath, FileMode.Open);
			if(stream.Length > 0)
				LoggTypeList = (List<LoggType>) serializer.Deserialize(stream);
			else
				LoggTypeList = new List<LoggType>();
			
			stream.Close();
			return LoggTypeList;
		}
		#endregion
		
		
		#region Save lists
		
		public void SaveJaktList (List<Jakt> item)
		{
			JaktList = item;
			string filePath = Path.Combine(path, FILE_JAKT);
			XmlSerializer serializer = new XmlSerializer( typeof(List<Jakt>) );
	        TextWriter writer = new StreamWriter(filePath);
	        serializer.Serialize(writer, JaktList);
			writer.Close();
			Console.WriteLine("Jakt items saved to file.");
		}

		public void SaveJegerList (List<Jeger> item)
		{
			JegerList = item;
			string filePath = Path.Combine(path, FILE_JEGER);
			XmlSerializer serializer = new XmlSerializer( typeof(List<Jeger>) );
	        TextWriter writer = new StreamWriter(filePath);
	        serializer.Serialize(writer, JegerList);
			writer.Close();
			Console.WriteLine("Jeger items saved to file.");
		}
		
		public void SaveDogList (List<Dog> item)
		{
			DogList = item;
			string filePath = Path.Combine(path, FILE_DOG);
			XmlSerializer serializer = new XmlSerializer( typeof(List<Dog>) );
	        TextWriter writer = new StreamWriter(filePath);
	        serializer.Serialize(writer, DogList);
			writer.Close();
			Console.WriteLine("Dog items saved to file.");
		}

		public void SaveArtList (List<Art> item)
		{
			ArtList = item;
			string filePath = Path.Combine(path, FILE_ART);
			XmlSerializer serializer = new XmlSerializer( typeof(List<Art>) );
	        TextWriter writer = new StreamWriter(filePath);
	        serializer.Serialize(writer, ArtList);
			writer.Close();
			Console.WriteLine("Art items saved to file.");
		}

		public void SaveLoggList (List<Logg> item)
		{
			LoggList = item;
			string filePath = Path.Combine(path, FILE_LOGG);
			XmlSerializer serializer = new XmlSerializer( typeof(List<Logg>) );
	        TextWriter writer = new StreamWriter(filePath);
	        serializer.Serialize(writer, LoggList);
			writer.Close();
			Console.WriteLine("Logg items saved to file.");
		}
		
		public void SaveSelectedArtIdList (List<int> item)
		{
			SelectedArtIdList = item;
			string filePath = Path.Combine(path, FILE_SELECTED_ARTIDS);
			XmlSerializer serializer = new XmlSerializer( typeof(List<int>) );
	        TextWriter writer = new StreamWriter(filePath);
	        serializer.Serialize(writer, SelectedArtIdList);
			writer.Close();
			Console.WriteLine("Art SelectedArtIdList saved to file.");
		}
		
		public void SaveSelectedLoggTypeIdList (List<string> item)
		{
			SelectedLoggTypeIdList = item;
			string filePath = Path.Combine(path, FILE_SELECTED_LOGGTYPEIDS);
			XmlSerializer serializer = new XmlSerializer( typeof(List<string>) );
	        TextWriter writer = new StreamWriter(filePath);
	        serializer.Serialize(writer, SelectedLoggTypeIdList);
			writer.Close();
			Console.WriteLine("SelectedLoggTypeIdList saved to file.");
		}
		#endregion
		
		
	}
}

