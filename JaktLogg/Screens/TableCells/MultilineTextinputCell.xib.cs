
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class MultilineTextinputCell : UIJaktViewController
	{
		public UITableViewCell Cell {
			get{ return cell; }
		}
		public UITextView TextView {
			get{ return txt; }
			set { txt = value; }
		}
		public MultilineTextinputCell () : base("MultilineTextinputCell", null)
		{
		
		}
	}
}

