
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
			
			_tableSource.ArtList = JaktLoggApp.instance.ArtList;
			TableView.ReloadData();
			
			if(JaktLoggApp.instance.SelectedArtIds.Count == 0)
			{	
				var title = "Dette er alle jaktbare arter i Norge." + Environment.NewLine;
				title += "Kryss av for de artene du jakter på, slik at de dukker opp i jaktloggen" + Environment.NewLine;
				title += "Les mer om hver art ved å klikke på pila.";

				MessageBox.Show("Om artslista", title);
			}
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			TableView.ReloadData();
		}
		
		public void Save(object sender, EventArgs e)
		{
			//_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}
	}
}

