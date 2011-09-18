
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FirstLoadJegereView : UIJaktViewController
	{
		private EventHandler HandleBtNewTouchUpInside;
		public FirstLoadJegereView (EventHandler buttonEvent) : base("FirstLoadJegereView", null)
		{
			HandleBtNewTouchUpInside = buttonEvent;
		}
		
		public override void ViewDidLoad ()
		{
			btNew.TouchUpInside += HandleBtNewTouchUpInside;
			base.ViewDidLoad ();
		}
	}
}

