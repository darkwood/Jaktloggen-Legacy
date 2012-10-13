
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class ArterTableScreen : UIJaktTableViewController
	{
		private ArterTableSource _tableSource;

		public ArterTableScreen () : base("ArterTableScreen", null)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();
			Title = "Arter";
			
			_tableSource = new ArterTableSource(this);
			TableView.Source = _tableSource;
			TableView.ReloadData();
			
			if(JaktLoggApp.instance.SelectedArtIds.Count == 0)
			{	
				var title = "Dette er alle jaktbare arter i Norge." + Environment.NewLine;
				title += "Kryss av for de artene du jakter på, slik at de dukker opp i jaktloggen" + Environment.NewLine;
				title += "Les mer om hver art ved å klikke på pila.";

				MessageBox.Show("Om artslista", title);
			}

			var rightBtn = new UIBarButtonItem(UIBarButtonSystemItem.Add, BarButtonClicked);
			NavigationItem.RightBarButtonItem = rightBtn;
		}

		void BarButtonClicked (object sender, EventArgs e)
		{

		}

		public void ShowAddScreeen()
		{
			var artScreen = new ArtScreen(screen => {
				Art a = screen.art;
				JaktLoggApp.instance.SaveArtItem(a);
				TableView.ReloadData();
			});
			NavigationController.PushViewController(artScreen, true);
		}
		
		public void Save(object sender, EventArgs e)
		{
			//_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}
	}
}

