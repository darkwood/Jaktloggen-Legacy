
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
			this.Title = "Statistikk";
			TableView.Source = new StatsFrontpageTableSource(this);
			
			base.ViewDidLoad ();
		}
	}
}

