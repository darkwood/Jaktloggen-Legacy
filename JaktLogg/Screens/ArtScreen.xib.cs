
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class ArtScreen: UIJaktTableViewController
	{
		public Art art;
		public bool IsNewItem;
		private Action<ArtScreen> _callback;

		public ArtScreen (Action<ArtScreen> callback) : base("ArtScreen", null)
		{
			art = new Art();
			_callback = callback;
			IsNewItem = true;
		}
		public ArtScreen (Art _art, Action<ArtScreen> callback) : base("ArtScreen", null)
		{
			art = _art;
			_callback = callback;
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = IsNewItem ? Utils.Translate("specie.new") : art.Navn;
			TableView.Source = new ArtTableSource(this, art);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			var leftbtn = new UIBarButtonItem(Utils.Translate("done"), UIBarButtonItemStyle.Done, DoneClicked);
			if(art.ID == 0)
				leftbtn = new UIBarButtonItem(Utils.Translate("cancel"), UIBarButtonItemStyle.Plain, DoneClicked);
			
			NavigationItem.LeftBarButtonItem = leftbtn;
			
			base.ViewDidAppear (animated);
		}
		private void DoneClicked(object sender, EventArgs e)
		{
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}
		
		public void Delete(Art a){
			JaktLoggApp.instance.DeleteArt(a);
			NavigationController.PopViewControllerAnimated(true);
		}

		public void Refresh(){
			if(art.Navn == "")
				art.Navn = Utils.Translate("specie.noname");

			art.GroupId = 100; //egendefinert gruppe
			JaktLoggApp.instance.SaveArtItem(art);
			TableView.ReloadData();
			Title = art.Navn;
		}

	}
}

