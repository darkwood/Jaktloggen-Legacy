
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class LoggerScreen : UIJaktTableViewController
	{
		public List<Logg> LoggList = new List<Logg>();
		private int _jaktId;
		private FirstLoadLoggView firstView;
		public LoggerScreen (int jaktId) : base("LoggerScreen", null)
		{
			_jaktId = jaktId;
			//_callback = callback;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			var rightBarButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, NewItemClicked);
			NavigationItem.RightBarButtonItem = rightBarButton;
		}
		
		public override void ViewDidAppear (bool animated)
		{
			Refresh();
			base.ViewDidAppear (animated);
		}
		
		public void Refresh()
		{
			LoggList = JaktLoggApp.instance.LoggList.Where(l => l.JaktId == _jaktId).ToList();
			Title = Utils.Translate("log.new.title");
			var tableSource = new LoggerTableSource(this, LoggList);
			TableView.Source = tableSource;
			TableView.ReloadData();
			
			if(firstView != null){
				firstView.View.RemoveFromSuperview();
				firstView = null;
			}
			
			if(LoggList.Count == 0)
			{
				firstView = new FirstLoadLoggView(NewItemClicked);
				TableView.AddSubview(firstView.View);
			}
		}
		
		public void NewItemClicked(object sender, EventArgs e){
			
			var loggItemScreen = new LoggItemScreen(_jaktId, screen => {
					JaktLoggApp.instance.SaveLoggItem(screen.logg);
					Refresh();
				});
			NavigationController.PushViewController(loggItemScreen, true);
		}
	}
}

