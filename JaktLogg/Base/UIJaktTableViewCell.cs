
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
			BackgroundColor = UIColor.FromRGBA(1f, 1f, 1f, 0.4f);
			
			if(TextLabel != null)
				TextLabel.BackgroundColor = UIColor.Clear;
			
			if(DetailTextLabel != null)
				DetailTextLabel.BackgroundColor = UIColor.Clear;
			
			if(AccessoryView != null)
				AccessoryView.BackgroundColor = UIColor.Clear;
			
			
			
		}
	}
}

