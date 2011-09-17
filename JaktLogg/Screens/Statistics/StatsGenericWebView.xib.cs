
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class StatsGenericWebView: UIJaktViewController
	{
		public string Header = "";
		public string Footer = "";
		public string Html = "";
		
		public StatsGenericWebView () : base("StatsGenericWebView", null)
		{
			
		}
		public StatsGenericWebView (string header, string footer, string html) : base("StatsGenericWebView", null)
		{
			Header = header;
			Footer = footer;
			Html = html;
		}
		
		public override void ViewDidLoad ()
		{
			lblFooter.Text = Footer;
			webView.LoadHtmlString(Html, null);
			base.ViewDidLoad ();
		}
	}
}

