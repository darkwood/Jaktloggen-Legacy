
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class ArtWebViewController: UIJaktViewController
	{
		private Art _art;
		public ArtWebViewController (Art art) : base("ArtWebViewController", null)
		{
			_art = art;
		}
		
		public override void ViewDidLoad ()
		{
			webView.LoadStarted += HandleWebViewLoadStarted;
			webView.LoadFinished += HandleWebViewLoadFinished;
			webView.LoadError += HandleWebViewLoadError;
			
			var rightbutton = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, RefreshClicked);
			NavigationItem.RightBarButtonItem = rightbutton;
			
			LoadWebContent();
			
			base.ViewDidLoad ();
		}
		
		void RefreshClicked (object sender, EventArgs e)
		{
			LoadWebContent();
		}
		
		private void LoadWebContent()
		{
			var artnavn = Uri.EscapeUriString(_art.Navn);
			//Check for override wikiname
			if(_art.Wikinavn.Length > 0)
				artnavn = Uri.EscapeUriString(_art.Wikinavn);
			
			var url = "http://no.m.wikipedia.org/wiki?search="+artnavn;
			Console.WriteLine(url);
			Title = "Laster side...";//_art.Navn;
			NavigateToUrl(url);
		}
		
		void HandleWebViewLoadStarted (object sender, EventArgs e)
		{
			//LoadingView.Show("Henter info om "+_art.Navn+ " fra Wikipedia. Vennligst vent.");
			activityIndicator.StartAnimating();
		}
		
		void HandleWebViewLoadFinished (object sender, EventArgs e)
		{
			//LoadingView.Hide();
			Title = _art.Navn;
			activityIndicator.StopAnimating();
		}
		
		public void HandleWebViewLoadError (object sender, UIWebErrorArgs e)
		{
			MessageBox.Show("Siden kunne ikke lastes", "Sjekk internett-tilkoblingen og prøv igjen");
			activityIndicator.StopAnimating();
		}
		
		protected void NavigateToUrl (string url)
		{
			if (!(url.StartsWith ("http://") || url.StartsWith ("https://")))
				url = "http://" + url;
			
			webView.LoadRequest (new NSUrlRequest (new NSUrl (url)));
		}
	}
}

