
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace JaktLogg
{
	public partial class JaktScreen : UIJaktNavigationController
	{
		[Export("initWithCoder:")]
		public JaktScreen (NSCoder coder) : base(coder)
		{
		}
		
		public override void ViewDidLoad ()
		{
			
			LoadingView.Show("Henter jaktdata...");
			JaktLoggApp.instance.InitializeAllData(() => {
				InvokeOnMainThread(() => {
					LoadingView.Hide();
					
					var jaktTableScreen = new JaktTableScreen();
					PushViewController(jaktTableScreen, false);					
				});
			});
			
			base.ViewDidLoad ();
		}
	}
}

