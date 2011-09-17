
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
namespace JaktLogg
{
	public class UIJaktNavigationController : UINavigationController
	{
		public UIJaktNavigationController (NSCoder coder) : base(coder)
		{
			
		}
		
		
		public override void ViewDidLoad ()
		{
			/*
			var v = new UIView (new RectangleF(0f,0f,NavigationBar.Frame.Width, NavigationBar.Frame.Height));
			v.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("Images/Backgrounds/NavBarTile.png"));
			v.UserInteractionEnabled = false;
			NavigationBar.InsertSubview (v, 0);
			*/
			NavigationBar.BarStyle = UIBarStyle.Black;
			//NavigationBar.TintColor = UIColor.FromPatternImage(UIImage.FromBundle("Images/Backgrounds/NavBarTile.png"));
			NavigationBar.Translucent = true;
			
			base.ViewDidLoad ();
		}
	}
}

