
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class ArterTableSource : UITableViewSource 
	{
		private ArterTableScreen _controller;
		
		public List<Art> ArtList = new List<Art>();
		
		public ArterTableSource(ArterTableScreen controller)
		{
			_controller = controller;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("ArterTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Default, "ArterTableCell");
			
			var artsInSection = ArtList.Where(a => a.GroupId == JaktLoggApp.instance.ArtGroupList[indexPath.Section].ID);
			var art = artsInSection.ElementAt(indexPath.Row);
			var label = art.Navn;
			
			var icon = JaktLoggApp.instance.SelectedArtIds.Contains(art.ID) ? "icon_checked.png" : "icon_unchecked.png";
			var file = "Images/Icons/"+icon;
			cell.ImageView.Image = new UIImage(file);
			cell.ImageView.Layer.MasksToBounds = true;
			cell.ImageView.Layer.CornerRadius = 5.0f;
			
			cell.TextLabel.Text = label;
			cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
			
			return cell;
		}
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			return new HeaderTableSection(TitleForHeader(tableView, section)).View;
		}
		
		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			return 40.0f;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return JaktLoggApp.instance.ArtGroupList.Count();
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			var rowsForSection = ArtList.Where(a => a.GroupId == JaktLoggApp.instance.ArtGroupList[section].ID).Count();
			return rowsForSection;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			var title = "";
			/*if(section == 0)
			{
				title = "Alle jaktbare arter i Norge." + Environment.NewLine + Environment.NewLine;
			}*/
			title += JaktLoggApp.instance.ArtGroupList[section].Navn;
			
			return title;
		}
		public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
		{
			var artsInSection = ArtList.Where(a => a.GroupId == JaktLoggApp.instance.ArtGroupList[indexPath.Section].ID);
			var art = artsInSection.ElementAt(indexPath.Row);
			var artScreen = new ArtWebViewController(art);
			_controller.NavigationController.PushViewController(artScreen, true);
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var artsInSection = ArtList.Where(a => a.GroupId == JaktLoggApp.instance.ArtGroupList[indexPath.Section].ID);
			var art = artsInSection.ElementAt(indexPath.Row);
			
			if(JaktLoggApp.instance.SelectedArtIds.Contains(art.ID))
			{
				for(var i=0; i<JaktLoggApp.instance.SelectedArtIds.Count; i++)
				{
					if(JaktLoggApp.instance.SelectedArtIds[i] == art.ID)
					{
						JaktLoggApp.instance.SelectedArtIds.RemoveAt(i);
					}
				}
			}
			else
			{
				JaktLoggApp.instance.SelectedArtIds.Add(art.ID);
			}
			
			tableView.DeselectRow(indexPath, true);
			
			var icon = JaktLoggApp.instance.SelectedArtIds.Contains(art.ID) ? "icon_checked.png" : "icon_unchecked.png";
			var file = "Images/Icons/"+icon;
			tableView.CellAt(indexPath).ImageView.Image = new UIImage(file);
			
			//Save the artidlist:
			JaktLoggApp.instance.SaveSelectedArtIds();
		}
	}
}

