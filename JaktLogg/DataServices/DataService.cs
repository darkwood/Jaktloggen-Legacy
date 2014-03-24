using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.Collections.Specialized;

namespace JaktLogg
{
	public static class DataService
	{
		public static string UserId {
			get{
				return UIDevice.CurrentDevice.UniqueIdentifier;
			}
		}
		public static string UploadImage(string filePath)
		{
			if(!File.Exists(filePath))
				return "Fil ikke funnet.";
			
			var url = "http://www.jaktloggen.no/customers/tore/jalo/services/uploadimage.ashx";
			using(WebClient client = new WebClient()) 
			{
				var nameValueCollection = new NameValueCollection();
				nameValueCollection.Add("userid",UserId);
				
				client.QueryString = nameValueCollection;
				try
				{
				    byte[] responseArray = client.UploadFile(url, filePath);
					return Encoding.Default.GetString(responseArray);
						
				}
				catch(WebException ex)
				{
					Console.WriteLine(ex.Message);
				}
					
			}
			return "Error";
		}
		
		public static string GetLastUploadedJakt()
		{ 
			var url = "http://www.jaktloggen.no/customers/tore/jalo/services/getjaktdata.ashx";
			var parameters = string.Format("userid={0}&type=date", UIDevice.CurrentDevice.UniqueIdentifier);
			
			var req = (HttpWebRequest) WebRequest.Create(url + "?" + parameters);
			req.Referer = "JaktloggenApp";
			req.UserAgent = "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_0 like Mac OS X; en-us) AppleWebKit/532.9 (KHTML, like Gecko) Version/4.0.5 Mobile/8A293 Safari/6531.22.7";
		
			var response = (HttpWebResponse)req.GetResponse();
			using(var reader = new StreamReader(response.GetResponseStream())){
				return reader.ReadToEnd();
			}
		}
		
		public static string UploadAllData()
		{
			var url="http://www.jaktloggen.no/customers/tore/jalo/services/uploadjakt.ashx";
			var userid = UIDevice.CurrentDevice.UniqueIdentifier;
			var parameters = "userid="+userid+
							"&jaktxml="+GetJaktXml().Replace("&", "&amp;")+
							"&jegerexml="+GetJegerXml().Replace("&", "&amp;")+
							"&loggerxml="+GetLoggerXml().Replace("&", "&amp;");
			
			return SendAndReceiveData(url, parameters, "POST");
		}
		
		private static string SendAndReceiveData(string url, string parameters, string method)
		{
			var req = WebRequest.Create(url);
			
			req.ContentType = "application/x-www-form-urlencoded";
			req.Method = method;
			
			var bytes = System.Text.Encoding.UTF8.GetBytes(parameters);
			req.ContentLength = bytes.Length;
			
			var os = req.GetRequestStream ();
			os.Write (bytes, 0, bytes.Length);
			os.Close ();
			
			var resp = req.GetResponse();
			if (resp== null) return null;
			
			var sr = new StreamReader(resp.GetResponseStream());
			var result = sr.ReadToEnd().Trim();

			sr.Close();
			resp.Close();
			
			return result;
		}
		private static string GetJegerXml()
		{	
			XmlSerializer serializer = new XmlSerializer( typeof(List<Jeger>) );
			MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
	        serializer.Serialize(xmlTextWriter, JaktLoggApp.instance.JegerList);
			memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
			return UTF8ByteArrayToString(memoryStream.ToArray());
		}
		
		private static string GetJaktXml()
		{	
			XmlSerializer serializer = new XmlSerializer( typeof(List<Jakt>) );
			MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
	        serializer.Serialize(xmlTextWriter, JaktLoggApp.instance.JaktList);
			memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
			return UTF8ByteArrayToString(memoryStream.ToArray());
		}
		
		private static string GetLoggerXml()
		{	
			XmlSerializer serializer = new XmlSerializer( typeof(List<Logg>) );
			MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
	        serializer.Serialize(xmlTextWriter, JaktLoggApp.instance.LoggList);
			memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
			return UTF8ByteArrayToString(memoryStream.ToArray());
		}
		
		private static string UTF8ByteArrayToString(Byte[] characters) 
		{ 
		
		  UTF8Encoding encoding = new UTF8Encoding();
		  String constructedString = encoding.GetString(characters);
		  return (constructedString);
		 }
	}
}

