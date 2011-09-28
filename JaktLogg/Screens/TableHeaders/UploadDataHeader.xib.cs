
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace JaktLogg
{
	public partial class UploadDataHeader : UIJaktViewController
	{
		public string Header = string.Empty;
		public EventHandler HandleButtonUploadTouchUpInside;
		
		public UploadDataHeader (string header) : base("UploadDataHeader", null)
		{
			Header = header;
		}
		
		public override void ViewDidLoad ()
		{
			label.Text = Header;
			//label.BackgroundColor = UIColor.Clear;
			//label.ShadowOffset = new SizeF(0.0f, 1.0f);
			//label.ShadowColor = UIColor.White;
			
			if(Header.Length == 0)
				label.Hidden = true;
			
			btUpload.TouchUpInside += HandleButtonUploadTouchUpInside;
			
			
			base.ViewDidLoad ();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			var datestring = DataService.GetLastUploadedJakt();
			label.Text = datestring;
			
			base.ViewDidAppear (animated);
		}
	}
}

