
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class JegereNavigationController : UIJaktNavigationController
	{
		[Export("initWithCoder:")]
		public JegereNavigationController (NSCoder coder) : base(coder)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			
			this.PushViewController(new JegereScreen(new List<int>(), screen => {
				
			}), false);
			
			base.ViewDidLoad ();
		}
		
	}
}

