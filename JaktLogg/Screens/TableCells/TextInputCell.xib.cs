
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class TextInputCell: UIJaktViewController
	{
		public UITableViewCell Cell {
			get{ return cell; }
		}
		public UITextField TextField {
			get{ return txt; }
			set { txt = value; }
		}
		public TextInputCell () : base("TextInputCell", null)
		{
				
		}
		
	}
}

