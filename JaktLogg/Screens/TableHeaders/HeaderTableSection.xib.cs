
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace JaktLogg
{
	public partial class HeaderTableSection : UIJaktViewController
	{
		public string Header {
			get;
			set;
		}
		public HeaderTableSection (string header) : base("HeaderTableSection", null)
		{
			Header = header;
		}

		public override void ViewDidAppear (bool animated)
		{
			
			label.Text = Header;

			if(Header.Length == 0)
				label.Hidden = true;

			base.ViewDidAppear (animated);
		}
		public override void ViewDidLoad ()
		{
			label.BackgroundColor = UIColor.Clear;

			base.ViewDidLoad ();
		}
	}
}

