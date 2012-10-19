
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FeedbackScreen : UIJaktViewController
	{
		public string Url {
			get;
			set;
		}
		public string Header {
			get;
			set;
		}
		public FeedbackScreen (string url, string header) : base("FeedbackScreen", null)
		{
			Url = url;
			Header = header;
		}
		
		public override void ViewDidLoad ()
		{
			buttonDone.Clicked += HandleButtonDoneClicked;
			buttonRefresh.Clicked += HandleButtonRefreshClicked;
			
			webView.LoadStarted += HandleWebViewLoadStarted;
			webView.LoadFinished += HandleWebViewLoadFinished;
			webView.LoadError += HandleWebViewLoadError;
	
			LoadWebContent();
			
			base.ViewDidLoad ();
		}
		
		private void LoadWebContent()
		{
			Title = "Laster side...";
			NavigateToUrl(Url);
		}
		
		void HandleWebViewLoadStarted (object sender, EventArgs e)
		{
			activityIndicator.StartAnimating();
		}
		
		void HandleWebViewLoadFinished (object sender, EventArgs e)
		{
			Title = Header;
			activityIndicator.StopAnimating();
		}
		
		public void HandleWebViewLoadError (object sender, UIWebErrorArgs e)
		{
			MessageBox.Show("Kan ikke åpne side", "Du må ha tilgang til internett for å se siden.");
			activityIndicator.StopAnimating();
		}
		
		protected void NavigateToUrl (string url)
		{
			if (!(url.StartsWith ("http://") || url.StartsWith ("https://")))
				url = "http://" + url;
			
			webView.LoadRequest (new NSUrlRequest (new NSUrl (url)));
		}
		
		void HandleButtonDoneClicked (object sender, EventArgs e)
		{
			this.DismissModalViewControllerAnimated(true);
		}
		
		void HandleButtonRefreshClicked (object sender, EventArgs e)
		{
			LoadWebContent();
		}

	}
}

