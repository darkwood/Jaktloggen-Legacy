
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class MoreScreen : UIJaktNavigationController
	{
		[Export("initWithCoder:")]
		public MoreScreen (NSCoder coder) : base(coder)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			this.PushViewController(new MoreTableScreen(), false);
			
			base.ViewDidLoad ();
		}
	}
}

