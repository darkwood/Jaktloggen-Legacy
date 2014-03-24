using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class WebScreen : UIJaktViewController
	{
		public string Url {
			get;
			set;
		}
		public WebScreen () : base ("WebScreen", null)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			var rightbtn = new UIBarButtonItem(UIBarButtonSystemItem.Action, HandleBtActionClicked);
			NavigationItem.RightBarButtonItem = rightbtn;
			btRefresh.Clicked += HandleButtonRefreshClicked;
			btBack.Clicked += HandleBtBackClicked;
			btForward.Clicked += HandleBtForwardClicked;
			webView.LoadStarted += HandleWebViewLoadStarted;
			webView.LoadFinished += HandleWebViewLoadFinished;
			webView.LoadError += HandleWebViewLoadError;
	
			NavigateToUrl(Url);
			
			base.ViewDidLoad ();
		}

		void HandleBtActionClicked (object sender, EventArgs e)
		{
			var actionSheet = new UIActionSheet("") {"Del på facebook", "Send link på e-post", Utils.Translate("cancel")};
			actionSheet.Title = "Del denne siden";
			//actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 2;
			actionSheet.ShowInView(this.View);
			
			actionSheet.Clicked += delegate(object s, UIButtonEventArgs evt) 
			{
				switch (evt.ButtonIndex)
				{
				case 0:
					//Del på facebook
					
					break;
				case 1:
					//Send link på e-post
					var url = webView.Request.MainDocumentURL;
					var htmlstr ="<a href='"+url+"'>"+url+"</a>";
					var reportScreen = new ReportJakt(htmlstr);
					this.NavigationController.PushViewController(reportScreen, true);
					break;
				/*case 2:
					//Del på face
					MessageBox.Show("Ennå ikke implementert...", "");
					break;
				*/default:
					//Avbryt
					
					break;
				}
			};
		}

		void HandleBtForwardClicked (object sender, EventArgs e)
		{
			webView.GoForward();
		}

		void HandleBtBackClicked (object sender, EventArgs e)
		{
			webView.GoBack();
		}
		
		void HandleWebViewLoadStarted (object sender, EventArgs e)
		{
			activityIndicator.StartAnimating();
		}
		
		void HandleWebViewLoadFinished (object sender, EventArgs e)
		{
			activityIndicator.StopAnimating();
			btForward.Enabled = webView.CanGoForward;
			btBack.Enabled = webView.CanGoBack;
		}
		
		public void HandleWebViewLoadError (object sender, UIWebErrorArgs e)
		{
			MessageBox.Show("Kan ikke åpne side", "Du må ha tilgang til internett for å se innholdet.");
			activityIndicator.StopAnimating();
		}
		
		protected void NavigateToUrl (string url)
		{
			webView.LoadRequest (new NSUrlRequest (new NSUrl (url)));
		}
		
		void HandleButtonDoneClicked (object sender, EventArgs e)
		{
			this.DismissModalViewControllerAnimated(true);
		}
		
		void HandleButtonRefreshClicked (object sender, EventArgs e)
		{
			webView.Reload();
		}

	}
}

