
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
namespace JaktLogg
{
	public class UIJaktViewController : UIViewController
	{
		public UIJaktViewController (NSCoder coder) : base(coder)
		{
			
		}
		public UIJaktViewController (IntPtr handle) : base(handle)
		{
			
		}
		public UIJaktViewController (string nibName, NSBundle bundle) : base(nibName, bundle)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			View.BackgroundColor = UIColor.Clear;
			base.ViewDidLoad ();
		}
	}
}

