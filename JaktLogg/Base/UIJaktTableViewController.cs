
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
namespace JaktLogg
{
	public class UIJaktTableViewController : UITableViewController
	{
		public UIJaktTableViewController (string nibName, NSBundle bundle) : base(nibName, bundle)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			//TableView.BackgroundView.BackgroundColor = UIColor.Clear;
			var tblBackground=UIImage.FromFile ("Images/Backgrounds/Stripes.png");
			var clr=UIColor.FromPatternImage (tblBackground);
			TableView.BackgroundView=new UIView(ParentViewController.View.Frame)
			{
				BackgroundColor=clr
			};
			TableView.SizeToFit (); 
			//TableView.BackgroundColor = UIColor.Clear;
			TableView.SeparatorColor = UIColor.DarkGray;
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLineEtched;

			base.ViewDidLoad ();
		}
	}
}

