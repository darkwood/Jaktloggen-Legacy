
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class JaktTableScreen : UIJaktTableViewController
	{
		private JaktTableSource _tableSource;
		private FirstLoadJaktView firstView;
		public JaktTableScreen () : base("JaktTableScreen", null)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = "Jaktloggen";
			
			var rightBarButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, NewItemClicked);
			NavigationItem.RightBarButtonItem = rightBarButton;
			
			_tableSource = new JaktTableSource(this);
			TableView.Source = _tableSource;
			
			//Refresh();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			Refresh();
			base.ViewDidAppear (animated);
		}
	
		public void NewItemClicked(object sender, EventArgs e)
		{
			/*var jakt = new Jakt();
			var fieldScreen = new FieldStringScreen("Navn på jaktsted", screen => {
				jakt.Sted = screen.Value;
				JaktLoggApp.instance.SaveJaktItem(jakt);
				//Redirect user to the new jakt screen:
				var jaktItemScreen = new JaktItemScreen(jakt, screen2 => {
					JaktLoggApp.instance.SaveJaktItem(screen2.jakt);
					Refresh();
				});
				NavigationController.PushViewController(jaktItemScreen, true);
			});
			fieldScreen.Placeholder = "Skriv inn jaktsted";
			this.PresentModalViewController(fieldScreen, true);
			*/
			
			var jaktItemScreen = new JaktItemScreen(screen => {
					JaktLoggApp.instance.SaveJaktItem(screen.jakt);
					Refresh();
				});
			NavigationController.PushViewController(jaktItemScreen, true);
		}
		
		public void Refresh(){
			_tableSource.JaktList = JaktLoggApp.instance.JaktList;
			TableView.ReloadData();
			
			if(firstView != null){
				firstView.View.RemoveFromSuperview();
				firstView = null;
			}
			
			if(JaktLoggApp.instance.JaktList.Count == 0)
			{
				firstView = new FirstLoadJaktView(NewItemClicked);
				TableView.AddSubview(firstView.View);
			}
		}
		

	}
}

