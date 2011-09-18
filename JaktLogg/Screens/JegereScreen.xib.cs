
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace JaktLogg
{
	public partial class JegereScreen : UIJaktTableViewController
	{
		private JegereTableSource _tableSource;
		public List<int> jegerIds;
		private Action<JegereScreen> _callback;
		private FirstLoadJegereView firstView;
		private UIBarButtonItem newBarButton;
		
		public JegereScreen (List<int> _jegerIds, Action<JegereScreen> callback) : base("JegereScreen", null)
		{
			jegerIds = _jegerIds;
			_callback = callback;
		}
		
		public override void ViewDidLoad ()
		{
			Title = "Alle jegere";
			
			newBarButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, NewItemClicked);
			NavigationItem.RightBarButtonItem = newBarButton;
		
			_tableSource = new JegereTableSource(this);
			TableView.Source = _tableSource;
			
			//Refresh();
			
			base.ViewDidLoad();			
		}
		
		public override void ViewDidAppear (bool animated)
		{
			Refresh();
			base.ViewDidAppear (animated);
		}
			
		private void NewItemClicked(object sender, EventArgs e){
			var jegerScreen = new JegerScreen(screen => {
				Jeger j = screen.jeger;
				JaktLoggApp.instance.SaveJegerItem(j);
				jegerIds.Add(JaktLoggApp.instance.JegerList.Last().ID);
				Refresh();
			});
			NavigationController.PushViewController(jegerScreen, true);
		}
		
		public void Refresh(){
			_tableSource.JegerIds = jegerIds;
			TableView.ReloadData();
			
			if(firstView != null){
				firstView.View.RemoveFromSuperview();
				firstView = null;
			}
			
			if(JaktLoggApp.instance.JegerList.Count == 0)
			{
				firstView = new FirstLoadJegereView(NewItemClicked);
				TableView.AddSubview(firstView.View);
			}
		}
		
		public void Save(object sender, EventArgs e)
		{
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}
	}
}