
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FirstLoadJaktView : UIJaktViewController
	{
		private EventHandler HandleBtNewTouchUpInside;
		public FirstLoadJaktView (EventHandler buttonEvent) : base("FirstLoadJaktView", null)
		{
			HandleBtNewTouchUpInside = buttonEvent;
		}
		
		public override void ViewDidLoad ()
		{
			btNew.SetBackgroundImage(new UIImage("Images/Buttons/Gray.png").StretchableImage(10, 0), UIControlState.Normal);
			btNew.TouchUpInside += HandleBtNewTouchUpInside;
			base.ViewDidLoad ();
		}

	}
}

