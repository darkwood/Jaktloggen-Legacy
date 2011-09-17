
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class LoggFields : UIJaktNavigationController
	{
		[Export("initWithCoder:")]
		public LoggFields (NSCoder coder) : base(coder)
		{
		}
		
		public override void ViewDidLoad ()
		{
			this.PushViewController(new LoggSetupScreen(), false);
			
			base.ViewDidLoad ();
		}
	}
}

