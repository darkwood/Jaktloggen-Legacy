
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class LoggItemScreen : UIJaktTableViewController
	{
		public Logg logg;
		public bool IsNewItem;
		private Action<LoggItemScreen> _callback;
		public LoggItemScreen (int jaktId, Action<LoggItemScreen> callback) : base("LoggItemScreen", null)
		{
			logg = new Logg(jaktId);
			_callback = callback;
			IsNewItem = true;
		}
		public LoggItemScreen (Logg _logg, Action<LoggItemScreen> callback) : base("LoggItemScreen", null)
		{
			logg = _logg;
			_callback = callback;
		}
		
		private LoggItemTableSource _tableSource;
		public override void ViewDidLoad ()
		{
			
			Title =IsNewItem ? Utils.Translate("log.new") : Utils.Translate("log");
			
			logg.Dato = new DateTime(JaktLoggApp.instance.CurrentJakt.DatoFra.Year, 
					                                logg.Dato.Month, 
					                                logg.Dato.Day, 
					                                logg.Dato.Hour, 
					                                logg.Dato.Minute, 
					                                logg.Dato.Second);
			
			_tableSource = new LoggItemTableSource(this, logg);
			TableView.Source = _tableSource;

			base.ViewDidLoad ();
		}

		public override void ViewDidAppear (bool animated)
		{
			Refresh();
			base.ViewDidAppear (animated);
		}

		public void Refresh()
		{
			logg = _tableSource.loggItem;
			JaktLoggApp.instance.SaveLoggItem(logg);
			_tableSource.loggItem = logg;

			if(logg.ID > 0){
				Title = Utils.Translate("log");
				NavigationItem.RightBarButtonItem = null;
			}
			TableView.ReloadData();
		}
		
		public void DeleteLogg(){
			JaktLoggApp.instance.DeleteLogg(logg);
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}
	}
}

