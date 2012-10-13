
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace JaktLogg
{
	public class UIJaktTableViewCell : UITableViewCell
	{
		public UIJaktTableViewCell (UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
		{
			//BackgroundColor = UIColor.FromPatternImage(new UIImage("Images/Backgrounds/YellowTile.png"));
			BackgroundColor = UIColor.FromRGB(231, 234, 196);
			
			if(TextLabel != null){
				TextLabel.BackgroundColor = UIColor.Clear;
				TextLabel.TextColor = UIColor.Black;
				TextLabel.ShadowColor = UIColor.White;
			}
			if(DetailTextLabel != null){
				DetailTextLabel.BackgroundColor = UIColor.Clear;
				DetailTextLabel.TextColor = UIColor.FromRGB(0.2f, 0.2f, 0.2f);
				//DetailTextLabel.ShadowColor = UIColor.White;
				
			}
			if(AccessoryView != null)
				AccessoryView.BackgroundColor = UIColor.Clear;
			
		}
	}
}

