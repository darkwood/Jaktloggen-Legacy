
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;

namespace JaktLogg
{
	public partial class UploadDataScreen : UIJaktViewController
	{
		private string response;
		public UploadDataScreen () : base("UploadDataScreen", null)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			btUpload.TouchUpInside += HandleBtUploadTouchUpInside;
			
			base.ViewDidLoad ();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			lblOutput.Text = "Sjekker siste backup...";
			var datestring = DataService.GetLastUploadedJakt();
			if(datestring != ""){
				lblOutput.Text = "Siste backup: " + datestring;
			}
			else{
				lblOutput.Text = "En feil skjedde ved lesing av data.";
			}
			
			base.ViewDidAppear (animated);
		}
		void HandleBtUploadTouchUpInside (object sender, EventArgs e)
		{
			lblOutput.Text ="Laster opp data... vennligst vent...";
			var timeStart = DateTime.Now;
			UploadData(() => {
				InvokeOnMainThread(() => {
					//MessageBox.Show("Data lastet opp", "http://jaktloggen.no/Jaktbok/?jaktid=1");
					var timeStop = DateTime.Now;
					lblOutput.Text = string.Format("Data lastet opp! ({0})", Math.Round((timeStop-timeStart).TotalMilliseconds/1000,2));
				});
			});
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

