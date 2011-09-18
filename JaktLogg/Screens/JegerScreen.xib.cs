
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class JegerScreen : UIJaktTableViewController
	{
		public Jeger jeger;
		public bool IsNewItem;
		private Action<JegerScreen> _callback;
		
		public JegerScreen (Action<JegerScreen> callback) : base("JegerScreen", null)
		{
			jeger = new Jeger();
			_callback = callback;
			IsNewItem = true;
		}
		public JegerScreen (Jeger _jeger, Action<JegerScreen> callback) : base("JegerScreen", null)
		{
			jeger = _jeger;
			_callback = callback;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = IsNewItem ? "Ny jeger" : jeger.Fornavn;
			TableView.Source = new JegerItemTableSource(this, jeger);
			
			
			
			/*if(IsNewItem){
				var rightBarButton = new UIBarButtonItem("Ferdig", UIBarButtonItemStyle.Done, NewItemSaveClicked);
				NavigationItem.RightBarButtonItem = rightBarButton;
			}*/
		}
		
		public override void ViewDidAppear (bool animated)
		{
			var leftbtn = new UIBarButtonItem("Ferdig", UIBarButtonItemStyle.Done, DoneClicked);
			if(jeger.ID == 0)
				leftbtn = new UIBarButtonItem("Avbryt", UIBarButtonItemStyle.Plain, DoneClicked);
			
			NavigationItem.LeftBarButtonItem = leftbtn;
			
			base.ViewDidAppear (animated);
		}
		private void DoneClicked(object sender, EventArgs e)
		{
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}
		/*
		private void NewItemSaveClicked(object sender, EventArgs e){
			if(jeger.Fornavn == "")
			{
				MessageBox.Show("Navn mangler", "Du må skrive inn et fornavn før du lagrer.");
				return;
			}
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}*/
		
		public void Delete(Jeger j){
			JaktLoggApp.instance.DeleteJeger(j);
			NavigationController.PopViewControllerAnimated(true);
		}
		
		public void Refresh(){
			if(jeger.Fornavn == "")
				jeger.Fornavn = "Uten navn";
			
			JaktLoggApp.instance.SaveJegerItem(jeger);
			TableView.ReloadData();
			Title = jeger.Fornavn;
		}
	}
}

