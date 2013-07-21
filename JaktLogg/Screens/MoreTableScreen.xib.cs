
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class MoreTableScreen : UIJaktTableViewController
	{
		public MoreTableScreen () : base("MoreTableScreen", null)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			this.Title = Utils.Translate("more.header");
			TableView.Source = new MoreScreenTableSource(this);
			
			base.ViewDidLoad ();
		}
	}
}

