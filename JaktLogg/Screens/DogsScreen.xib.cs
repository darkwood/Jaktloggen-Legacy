
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace JaktLogg
{
	public partial class DogsScreen : UIJaktTableViewController
	{
		private DogsTableSource _tableSource;
		public List<int> dogIds;
		private Action<DogsScreen> _callback;
		private FirstLoadDogsView firstView;
		private UIBarButtonItem newBarButton;
		
		public DogsScreen (List<int> _dogIds, Action<DogsScreen> callback) : base("DogsScreen", null)
		{
			dogIds = _dogIds;
			_callback = callback;
		}
		
		public override void ViewDidLoad ()
		{
			Title = Utils.Translate("all_dogs");
			
			newBarButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, NewItemClicked);
			NavigationItem.RightBarButtonItem = newBarButton;
		
			_tableSource = new DogsTableSource(this);
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
			var dogScreen = new DogScreen(screen => {
				Dog j = screen.dog;
				JaktLoggApp.instance.SaveDogItem(j);
				dogIds.Add(JaktLoggApp.instance.DogList.Last().ID);
				Refresh();
			});
			NavigationController.PushViewController(dogScreen, true);
		}
		
		public void Refresh(){
			_tableSource.DogIds = dogIds;
			TableView.ReloadData();
			
			if(firstView != null){
				firstView.View.RemoveFromSuperview();
				firstView = null;
			}
			
			if(JaktLoggApp.instance.DogList.Count == 0)
			{
				firstView = new FirstLoadDogsView(NewItemClicked);
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