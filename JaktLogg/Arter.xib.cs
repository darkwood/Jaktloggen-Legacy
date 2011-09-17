
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class Arter : UIJaktNavigationController
	{
		[Export("initWithCoder:")]
		public Arter (NSCoder coder) : base(coder)
		{
		}
		
		public override void ViewDidLoad ()
		{
			this.PushViewController(new ArterTableScreen(), false);
			
			base.ViewDidLoad ();
		}
	}
}

