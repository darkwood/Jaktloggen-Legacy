
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class StatsFrontpage : UIJaktTableViewController
	{
		public StatsFrontpage () : base("StatsFrontpage", null)
		{
			
		}

		public override void ViewDidLoad ()
		{
			Title = Utils.Translate("title_statistics");
			TableView.Source = new StatsFrontpageTableSource(this);
			
			base.ViewDidLoad ();
		}
	}
}

