
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class HeaderStatsFilter : UIJaktViewController
	{
		public EventHandler HandleSegmentedControlValueChange;
		public EventHandler HandleButtonFilterClicked;
		public HeaderStatsFilter () : base("HeaderStatsFilter", null)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			SetFilterLabel(Utils.Translate("showingcountfor") + " " + Utils.Translate("all_hunters_dogs"));
			segmentedControl.ValueChanged += HandleSegmentedControlValueChange;
			buttonFilter.TouchUpInside += HandleButtonFilterClicked;
			base.ViewDidLoad ();
		}
		
		public void SetFilterLabel(string txt){
			
			labelFilter.Text = txt;
		}

	}
}

