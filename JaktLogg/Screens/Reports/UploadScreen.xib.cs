
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

namespace JaktLogg
{
	public partial class UploadScreen : UIJaktTableViewController
	{
		private UploadScreenTableSource _tableSource;
		private UIBarButtonItem _rightButton;
		public Jakt jakt;
		public UploadScreen (Jakt _jakt) : base("UploadScreen", null)
		{
			jakt = _jakt;
		}
		
		public override void ViewDidLoad ()
		{
			Title = "Lag jaktbok";
			
			_rightButton = new UIBarButtonItem("Ferdig", UIBarButtonItemStyle.Done, HandleRightButtonClicked);			
			NavigationItem.RightBarButtonItem = _rightButton;
			
			_tableSource = new UploadScreenTableSource(this);
			TableView.Source = _tableSource;
			
			base.ViewDidLoad ();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}
		
		public void Refresh(){
			
		}
		
		void HandleRightButtonClicked (object sender, EventArgs EventArgs)
		{
			this.NavigationController.PopViewControllerAnimated(true);
		}
	}
	
	
	public class UploadScreenTableSource : UITableViewSource
	{
		private UploadScreen _controller;
		private List<SectionMapping> sections = new List<SectionMapping>();

		
		public UploadScreenTableSource(UploadScreen controller)
		{
			_controller = controller;
			
			var section1 = new SectionMapping("Tittel", "");
			var section2 = new SectionMapping("Velg mottakere", " ");
			var section3 = new SectionMapping("", " ");
			
			sections.Add(section1);
			sections.Add(section2);
			sections.Add(section3);
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Undertittel på jaktboka",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var fieldScreen = new FieldStringScreen("Undertittel", screen => {
						//jakt.Sted = screen.Value;
						_controller.Refresh();
					});
					fieldScreen.Placeholder = "Undertittel";
					//fieldScreen.Value = jakt.Sted;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/signpost.png"
			});
			
			section2.Rows.Add(new RowItemMapping {
				Label = "Velg jegere",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var fieldScreen = new JegerPickerScreen(new List<int>(), screen => {
						_controller.Refresh();
					});
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/user.png"
			});
			
			section3.Rows.Add(new RowItemMapping {
				Label = "Lag jaktbok",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					
					LoadingView.Show("Laster opp data...");
					var response = UploadJakt();
					Console.WriteLine(response);	
					LoadingView.Hide();
					MessageBox.Show("Jaktbok opprettet :)", "http://jaktloggen.no/Jaktbok/?jaktid="+_controller.jakt.ID);
					/*
					JaktLoggApp.instance.InitializeAllData(() => {
						InvokeOnMainThread(() => {
							
						});
					});*/
							
					
				},
				ImageFile = "Images/Icons/user.png"
			});
		}
		
		private string UploadJakt()
		{
			var url="http://www.jaktloggen.no/customers/tore/jalo/services/uploadjakt.ashx";
			var userid = UIDevice.CurrentDevice.UniqueIdentifier;
			var parameters = "userid="+userid+
							"&jaktxml="+GetJaktXml().Replace("&", "&amp;")+
							"&jegerexml="+GetJegerXml().Replace("&", "&amp;")+
							"&loggerxml="+GetLoggerXml().Replace("&", "&amp;");
			
			//Last opp jaktxml til server
			//Create a WebRequest
			WebRequest req = WebRequest.Create(url);
	
			//Set the content type and method
			
			req.ContentType = "application/x-www-form-urlencoded";
			req.Method = "POST";
			
			//Get the total size of the post parameters and set the content length
			byte [] bytes = System.Text.Encoding.UTF8.GetBytes(parameters);
			req.ContentLength = bytes.Length;
	
			//Write the data to the request stream
			Stream os = req.GetRequestStream ();
			os.Write (bytes, 0, bytes.Length);
			os.Close ();
	
			//Get the response
			WebResponse resp = req.GetResponse();
			if (resp== null) return null;
	
			//Get the response stream and read the response
			StreamReader sr = new StreamReader(resp.GetResponseStream());
			string result = sr.ReadToEnd().Trim();

			sr.Close();
			resp.Close();
			
			return result;
			
			
		}
		
		private string GetJegerXml()
		{	
			XmlSerializer serializer = new XmlSerializer( typeof(List<Jeger>) );
			MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
	        serializer.Serialize(xmlTextWriter, JaktLoggApp.instance.JegerList);
			memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
			return UTF8ByteArrayToString(memoryStream.ToArray());
		}
		
		private string GetJaktXml()
		{	
			XmlSerializer serializer = new XmlSerializer( typeof(List<Jakt>) );
			MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
	        serializer.Serialize(xmlTextWriter, JaktLoggApp.instance.JaktList);
			memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
			return UTF8ByteArrayToString(memoryStream.ToArray());
		}
		
		private string GetLoggerXml()
		{	
			XmlSerializer serializer = new XmlSerializer( typeof(List<Logg>) );
			MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
	        serializer.Serialize(xmlTextWriter, JaktLoggApp.instance.LoggList);
			memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
			return UTF8ByteArrayToString(memoryStream.ToArray());
		}
		
		private String UTF8ByteArrayToString(Byte[] characters) 
		{ 
		
		  UTF8Encoding encoding = new UTF8Encoding();
		  String constructedString = encoding.GetString(characters);
		  return (constructedString);
		 }
		
		public override int NumberOfSections (UITableView tableView)
		{
			return sections.Count;
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return sections.ElementAt(section).Rows.Count();
		}
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return sections.ElementAt(section).Header;
		}
		public override string TitleForFooter (UITableView tableView, int section)
		{
			return sections.ElementAt(section).Footer;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, false);
			sections.ElementAt(indexPath.Section).Rows.ElementAt(indexPath.Row).RowSelected();
		}
		
		
		private string GetCellLabel(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).Label;
		}
		
		private string GetCellValue(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).GetValue();
		}
			
		private string GetImageFile(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).ImageFile;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("UploadScreenCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Value1, "UploadScreenCell");
			
			
			if(indexPath.Section == 2)
			{
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Default, "UploadButtonCell");
				cell.TextLabel.Text = GetCellLabel(indexPath.Section, indexPath.Row);
				cell.TextLabel.TextAlignment = UITextAlignment.Center;
			}
			else{
				cell.TextLabel.Text = GetCellLabel(indexPath.Section, indexPath.Row);
				cell.DetailTextLabel.Text = GetCellValue(indexPath.Section, indexPath.Row);
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			}
			
			return cell;
		}
	}
}

