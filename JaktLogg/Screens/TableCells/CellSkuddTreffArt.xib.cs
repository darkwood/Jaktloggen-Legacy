
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class CellSkuddTreffArt: UIJaktViewController
	{
		public UITableViewCell Cell {
			get{ return cell; }
		}
		
		public UIPickerView Picker {
			get{ return picker; }
		}
		
		public CellSkuddTreffArt () : base("CellSkuddTreffArt", null)
		{
			
		}
	}
}

