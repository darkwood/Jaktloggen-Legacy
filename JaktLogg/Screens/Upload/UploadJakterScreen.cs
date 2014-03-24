
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using System.Collections.Specialized;
using System.IO;


namespace JaktLogg
{
	public partial class UploadJakterScreen : UIJaktViewController
	{
		//loads the UploadJakterScreen.xib file and connects it to this object
		public UploadJakterScreen () : base ("UploadJakterScreen", null)
		{
		}
		public override void ViewDidLoad ()
		{
			Title = "Last opp data";
			buttonUpload.TouchUpInside += HandleButtonUploadTouchUpInside;
			buttonJaktturer.TouchUpInside += HandleButtonJaktturerTouchUpInside;
			progressImage.Layer.MasksToBounds = true;
			progressImage.Layer.CornerRadius = 5.0f;
			progressImage.Layer.ShadowRadius = 2.0f;
			
			base.ViewDidLoad ();
		}

		void HandleButtonJaktturerTouchUpInside (object sender, EventArgs e)
		{
			ShowWebView();
		}

		void HandleButtonUploadTouchUpInside (object sender, EventArgs e)
		{
			UploadDataAsync();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public void ShowWebView ()
		{
			var url = "http://www.jaktloggen.no/turer/?userid=" + UIDevice.CurrentDevice.UniqueIdentifier;
			Console.WriteLine(url);
			var webView = new WebScreen();
			webView.Url = url;
			webView.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
			//this.PresentModalViewController(webView, true);
			this.NavigationController.PushViewController(webView, true);
		}
		
		public void UploadDataAsync(){
			Console.WriteLine("Laster opp data", "Vennligst vent...");
			
			progressLabel.Text = "Laster opp data...";
			progressBar.Progress = 0.01f;
			UploadData(() => {
				InvokeOnMainThread(() => {
					var imagesToUpload = ImageUtility.GetImagePaths();
					UploadNextImage(imagesToUpload, 0);
				});
			});	
			
		}
		
		private void UploadNextImage(List<string> imagesToUpload, int index)
		{
			if(imagesToUpload.Count() <= index)
			{
				progressLabel.Text = "All data lastet opp!";
				progressBar.Progress = 1;
				ShowWebView();
				return;
			}
			var nextImgPath = imagesToUpload.ElementAt(index);
			
			progressImage.Image = new UIImage(nextImgPath);
			UploadImage(nextImgPath, () => {
				InvokeOnMainThread(() => {
					index += 1;
					var progressvalue = ((float)index)/imagesToUpload.Count;
					Console.WriteLine(progressvalue);
					progressBar.Progress = progressvalue;
					
					progressLabel.Text = string.Format("Laster bilde {0} av {1}", index, imagesToUpload.Count);
					UploadNextImage(imagesToUpload, index);
				});
			});	
		}
		
		private void UploadImage(string imageToUpload, Action callback)
		{
			ThreadPool.QueueUserWorkItem(a => {
				
				DataService.UploadImage(imageToUpload);
				
				callback();
			});
		}
		
		private void UploadData(Action callback)
		{
			ThreadPool.QueueUserWorkItem(a => {
				DataService.UploadAllData();
				callback();
			});
		}
	}
}

