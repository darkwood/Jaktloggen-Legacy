
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public class UIJaktTableViewCell : UITableViewCell
	{
		public UIJaktTableViewCell (UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
		{
			BackgroundColor = UIColor.FromPatternImage(new UIImage("Images/Backgrounds/YellowTile.png"));
			
			
			if(TextLabel != null){
				TextLabel.BackgroundColor = UIColor.Clear;
				TextLabel.TextColor = UIColor.Black;
				TextLabel.ShadowColor = UIColor.White;
			}
			if(DetailTextLabel != null){
				DetailTextLabel.BackgroundColor = UIColor.Clear;
				DetailTextLabel.TextColor = UIColor.DarkGray;
				//DetailTextLabel.ShadowColor = UIColor.White;
				
			}
			if(AccessoryView != null)
				AccessoryView.BackgroundColor = UIColor.Clear;
			
			
			
		}
	}
}

