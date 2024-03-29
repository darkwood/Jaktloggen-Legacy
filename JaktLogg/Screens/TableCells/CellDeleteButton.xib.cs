
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class CellDeleteButton: UIJaktViewController
	{
		
		public UITableViewCell Cell {
			get{ return cell; }
		}
		public string ButtonText = Utils.Translate("delete");
		private Action HandleDeleteButtonTouchUpInside;
		
		public CellDeleteButton (Action handleDeleteButtonTouchUpInside) : base("CellDeleteButton", null)
		{
			HandleDeleteButtonTouchUpInside = handleDeleteButtonTouchUpInside;
		}
		
		
		public void LoadGUI ()
		{
			UIButton button = new UIButton();
			
			button.Frame = new System.Drawing.RectangleF(8, -2, this.cell.Bounds.Size.Width-16, 50);
			button.SetTitle(ButtonText, UIControlState.Normal);
			button.Font = UIFont.BoldSystemFontOfSize(20);
			
			button.SetBackgroundImage(new UIImage("Images/Buttons/Delete.png").StretchableImage(10, 0), UIControlState.Normal);
			
			button.TouchUpInside += delegate(object sender, EventArgs e) {
				HandleDeleteButtonTouchUpInside();
			};
			
			this.cell.AddSubview(button);
		}
		
		

	}
}

