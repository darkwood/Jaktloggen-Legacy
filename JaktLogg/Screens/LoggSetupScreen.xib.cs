
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class LoggSetupScreen : UIJaktTableViewController
	{
		private LoggSetupTableSource _tableSource;
		
		public LoggSetupScreen () : base("LoggSetupScreen", null)
		{
		
		}
		
		public override void ViewDidLoad ()
		{
			Title = Utils.Translate("setup.header");
			_tableSource = new LoggSetupTableSource(this);
			TableView.Source = _tableSource;
			
			_tableSource.LoggTypes = JaktLoggApp.instance.LoggTypeList;
			
			base.ViewDidLoad ();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			TableView.ReloadData();
			
			base.ViewDidAppear (animated);
		}
		
	}
}


