
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class DogScreen : UIJaktTableViewController
	{
		public Dog dog;
		public bool IsNewItem;
		private Action<DogScreen> _callback;
		
		public DogScreen (Action<DogScreen> callback) : base("DogScreen", null)
		{
			dog = new Dog();
			_callback = callback;
			IsNewItem = true;
		}
		public DogScreen (Dog _dog, Action<DogScreen> callback) : base("DogScreen", null)
		{
			dog = _dog;
			_callback = callback;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = IsNewItem ? Utils.Translate("dogs.new") : dog.Navn;
			TableView.Source = new DogItemTableSource(this, dog);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			var leftbtn = new UIBarButtonItem(Utils.Translate("done"), UIBarButtonItemStyle.Done, DoneClicked);
			if(dog.ID == 0)
				leftbtn = new UIBarButtonItem(Utils.Translate("cancel"), UIBarButtonItemStyle.Plain, DoneClicked);
			
			NavigationItem.LeftBarButtonItem = leftbtn;
			
			base.ViewDidAppear (animated);
		}
		private void DoneClicked(object sender, EventArgs e)
		{
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}

		public void Delete(Dog j){
			JaktLoggApp.instance.DeleteDog(j);
			NavigationController.PopViewControllerAnimated(true);
		}
		
		public void Refresh(){
			if(dog.Navn == "")
				dog.Navn = Utils.Translate("dog.noname");
			
			JaktLoggApp.instance.SaveDogItem(dog);
			TableView.ReloadData();
			Title = dog.Navn;
		}
	}
}

