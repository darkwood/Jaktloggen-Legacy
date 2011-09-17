
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class StatistikkNavigationController : UIJaktNavigationController
	{
		[Export("initWithCoder:")]
		public StatistikkNavigationController (NSCoder coder) : base(coder)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			this.PushViewController(new StatsFrontpage(), false);
			
			base.ViewDidLoad ();
		}
	}
}

