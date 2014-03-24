
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace JaktLogg
{
	public partial class DogPickerScreen : UIJaktTableViewController
	{
		private DogsPickerSource _tableSource;
		public List<int> dogIds;
		private Action<DogPickerScreen> _callback;
		private FirstLoadDogsView firstView;
		private UIBarButtonItem saveBarButton, newBarButton;
		public string Footer = string.Empty;
		public string Header = string.Empty;
		public string Tittel;
		
		public DogPickerScreen (List<int> _dogIds, Action<DogPickerScreen> callback) : base("DogPickerScreen", null)
		{
			dogIds = _dogIds;
			_callback = callback;
			_tableSource = new DogsPickerSource(this);
		}
		
		
		
		public override void ViewDidLoad ()
		{
			Title = Tittel ?? "Velg hunder";
			
			saveBarButton = new UIBarButtonItem(Utils.Translate("done"), UIBarButtonItemStyle.Done, Save);
			newBarButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, NewItemClicked);
			
			NavigationItem.RightBarButtonItem = newBarButton;
			NavigationItem.LeftBarButtonItem = saveBarButton;
		
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
				//JaktLoggApp.instance.SaveDogItem(j);
				dogIds.Add(j.ID);
				//Refresh();
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

