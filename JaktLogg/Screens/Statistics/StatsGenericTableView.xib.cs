
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class StatsGenericTableView : UIJaktTableViewController
	{
		public Jeger SelectedJeger;
		public string Tittel = "";
		public string Mode = "Felt";

		public StatsGenericTableView (string tittel) : base("StatsGenericTableView", null)
		{
			Tittel = tittel;
		}

	}
}

