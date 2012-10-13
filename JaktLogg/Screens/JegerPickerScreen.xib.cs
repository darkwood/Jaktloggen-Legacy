
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace JaktLogg
{
	public partial class JegerPickerScreen : UIJaktTableViewController
	{
		private JegerePickerSource _tableSource;
		public List<int> jegerIds;
		private Action<JegerPickerScreen> _callback;
		private FirstLoadJegereView firstView;
		private UIBarButtonItem saveBarButton, newBarButton;
		public string Footer = string.Empty;
		public string Header = string.Empty;
		public string Tittel;
		public JegerPickerScreen (List<int> _jegerIds, Action<JegerPickerScreen> callback) : base("JegerPickerScreen", null)
		{
			jegerIds = new List<int>();
			jegerIds.AddRange(_jegerIds);
			_callback = callback;
		}
		
		public override void ViewDidLoad ()
		{
			Title = Utils.Translate("jegere.choose");
			
			saveBarButton = new UIBarButtonItem(Utils.Translate("done"), UIBarButtonItemStyle.Done, Save);
			newBarButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, NewItemClicked);
			
			NavigationItem.RightBarButtonItem = newBarButton;
			NavigationItem.LeftBarButtonItem = saveBarButton;
		
			_tableSource = new JegerePickerSource(this);
			TableView.Source = _tableSource;
			
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
				jegerIds.Add(j.ID);
			});
			NavigationController.PushViewController(jegerScreen, true);
		}
		
		public void Refresh()
		{
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