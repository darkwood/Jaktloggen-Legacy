
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FeedbackScreen : UIJaktViewController
	{

		public FeedbackScreen () : base("FeedbackScreen", null)
		{
			
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
			var url = "http://www.jaktloggen.no/tilbakemelding";
			Console.WriteLine(url);
			Title = "Laster side...";
			NavigateToUrl(url);
		}
		
		void HandleWebViewLoadStarted (object sender, EventArgs e)
		{
			activityIndicator.StartAnimating();
		}
		
		void HandleWebViewLoadFinished (object sender, EventArgs e)
		{
			Title = "Gi tilbakemelding";
			activityIndicator.StopAnimating();
		}
		
		public void HandleWebViewLoadError (object sender, UIWebErrorArgs e)
		{
			MessageBox.Show("Kan ikke åpne side", "Du må ha tilgang til internett for å bruke kontaktskjemaet.");
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

