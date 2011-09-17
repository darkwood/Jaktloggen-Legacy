
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

		public override void ViewDidLoad ()
		{
			label.Text = Header;
			label.BackgroundColor = UIColor.Clear;
			label.ShadowOffset = new SizeF(0.0f, 1.0f);
			label.ShadowColor = UIColor.White;
			
			if(Header.Length == 0)
				label.Hidden = true;
			
			base.ViewDidLoad ();
		}
	}
}

