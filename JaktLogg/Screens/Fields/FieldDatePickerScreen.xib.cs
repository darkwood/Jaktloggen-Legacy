
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FieldDatePickerScreen: UIJaktViewController
	{
		public DateTime Date = DateTime.Now;
		public UIDatePickerMode Mode = UIDatePickerMode.Date;
		
		private Action<FieldDatePickerScreen> _callback;
		public FieldDatePickerScreen (Action<FieldDatePickerScreen> callback) : base("FieldDatePickerScreen", null)
		{
			_callback = callback;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			datePicker.TimeZone = NSTimeZone.FromAbbreviation("GMT");
			datePicker.Date = Utils.DateTimeToNSDate(Date);
			datePicker.Mode = Mode;
			
			var leftBarButton = new UIBarButtonItem(Utils.Translate("cancel"), UIBarButtonItemStyle.Plain, CancelClicked);
			NavigationItem.LeftBarButtonItem = leftBarButton;
			
			var rightBarButton = new UIBarButtonItem(Utils.Translate("done"), UIBarButtonItemStyle.Done, SaveClicked);
			NavigationItem.RightBarButtonItem = rightBarButton;
		}
		
		private void CancelClicked(object sender, EventArgs args){
			
			NavigationController.PopViewControllerAnimated(true);	
		}
		private void SaveClicked(object sender, EventArgs args){
			Date = Utils.NSDateToDateTime(datePicker.Date);
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);	
		}
	}
}

