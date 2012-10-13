
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class JaktItemScreen : UIJaktTableViewController
	{
		public Jakt jakt;
		public bool IsNewItem;
		private JaktItemTableSource _tableSource;
		public JaktItemScreen (Action<JaktItemScreen> callback) : base("JaktItemScreen", null)
		{
			jakt = new Jakt();
			IsNewItem = true;
		}
		public JaktItemScreen (Jakt _jakt, Action<JaktItemScreen> callback) : base("JaktItemScreen", null)
		{
			jakt = _jakt;
			JaktLoggApp.instance.CurrentJakt = jakt;
		}
		
		public override void ViewDidLoad ()
		{
			Title = IsNewItem ? Utils.Translate("jakt.newhunt") : jakt.Sted;
			
			_tableSource = new JaktItemTableSource(this, jakt);
			TableView.Source = _tableSource;
			
			var rightbtn = new UIBarButtonItem(UIBarButtonSystemItem.Action, RightBarButtonClicked);
			NavigationItem.RightBarButtonItem = rightbtn;
			
			base.ViewDidLoad ();
		}
		
		private void RightBarButtonClicked(object sender, EventArgs args)
		{
			var actionSheet = new UIActionSheet("") {Utils.Translate("email.sendbymail"), Utils.Translate("cancel")};
			actionSheet.Title = Utils.Translate("actionsheet.reportheader");
			//actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 2;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object s, UIButtonEventArgs e) 
			{
				switch (e.ButtonIndex)
				{
				case 0:
					//Enkel rapport
					var reportScreen = new ReportJakt(jakt);
					this.NavigationController.PushViewController(reportScreen, true);
					break;
				/*case 1:
					//Jaktbok
					var uploadScreen = new UploadScreen(jakt);
					this.NavigationController.PushViewController(uploadScreen, true);
					break;
				case 2:
					//Del på face
					MessageBox.Show("Ennå ikke implementert...", "");
					break;
				*/default:
					//Avbryt
					
					break;
				}
			};
			
		}
		
		public override void ViewDidAppear (bool animated)
		{
			_tableSource.jakt = jakt;
			TableView.ReloadData();
		
			base.ViewDidAppear (animated);
		}
		
		public void Refresh(){
			JaktLoggApp.instance.SaveJaktItem(jakt);
			JaktLoggApp.instance.CurrentJakt = jakt;
			_tableSource.jakt = jakt;
			TableView.ReloadData();
			Title = jakt.Sted;
		}
		
		public void DeleteJakt(){
			JaktLoggApp.instance.DeleteJakt(jakt);
			NavigationController.PopViewControllerAnimated(true);
		}
	}
}

