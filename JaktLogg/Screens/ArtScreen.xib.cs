
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class ArtScreen: UIJaktViewController
	{
		private Art _art;
		public ArtScreen (Art art) : base("ArtScreen", null)
		{
			_art = art;
		}
		public override void ViewDidLoad ()
		{
			Title = _art.Navn;
			lblTitle.Text = _art.Navn;
			txtDescription.Text = _art.Description;
			//imageView.Image = new UIImage(_art.ImagePath);
			
			var rightBtn = new UIBarButtonItem("Mer info", UIBarButtonItemStyle.Plain, BarButtonClicked);
			NavigationItem.RightBarButtonItem = rightBtn;
			base.ViewDidLoad ();
		}
		
		void BarButtonClicked (object sender, EventArgs e)
		{
			var webView = new ArtWebViewController(_art);
			webView.ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal;
			this.PresentModalViewController(webView, true);
		}
	}
}

