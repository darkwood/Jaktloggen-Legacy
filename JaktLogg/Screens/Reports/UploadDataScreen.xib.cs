
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using System.Collections.Specialized;

namespace JaktLogg
{
	public partial class UploadDataScreen : UIJaktTableViewController
	{
		private string response;
		public UploadDataScreen () : base("UploadDataScreen", null)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			TableView.Source = new UploadDataTableSource(this);
			TableView.ReloadData();
			base.ViewDidLoad ();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}
		public void HandleBtUploadTouchUpInside (object sender, EventArgs e)
		{
			Console.WriteLine("Laster opp data, vennligst vent...");
			var timeStart = DateTime.Now;
			UploadData(() => {
				InvokeOnMainThread(() => {
					//MessageBox.Show("Data lastet opp", "http://jaktloggen.no/Jaktbok/?jaktid=1");
					var timeStop = DateTime.Now;
					Console.WriteLine(string.Format("Data lastet opp! ({0})", Math.Round((timeStop-timeStart).TotalMilliseconds/1000,2)));
					Console.WriteLine("Laster opp bilder:");
					var results = UploadJaktImages();
					foreach(string key in results.Keys)
					{
						var values = results.GetValues(key);
		                foreach (string value in values)
		                {
		                    Console.WriteLine(key + " : " + value == "1" ? "Lastet opp!" : "Feil: " + value);
		                }
						
					}
				});
			});
		}
		
		private NameValueCollection UploadJaktImages()
		{
			var uploadStatusCollection = new NameValueCollection();
			foreach(var item in JaktLoggApp.instance.JaktList){
				var status = DataService.UploadImage(Utils.GetPath("jakt_" + item.ID + ".jpg"));
				uploadStatusCollection.Add(item.Sted, status);
			}
			foreach(var item in JaktLoggApp.instance.LoggList){
				var status = DataService.UploadImage(Utils.GetPath("jaktlogg_" + item.ID + ".jpg"));
				uploadStatusCollection.Add("loggføring, "+item.Dato.ToShortDateString(), status);
			}
			foreach(var item in JaktLoggApp.instance.JegerList){
				var status = DataService.UploadImage(Utils.GetPath("jeger_" + item.ID + ".jpg"));
				uploadStatusCollection.Add(item.Fornavn, status);
			}
			return uploadStatusCollection;
		}
		
		private void UploadData(Action callback)
		{
			ThreadPool.QueueUserWorkItem(a => {
				
				response = DataService.UploadAllData();
				
				callback();
			});
		}
		
	}
}

