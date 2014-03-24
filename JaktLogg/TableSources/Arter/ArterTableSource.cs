
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
		private List<HeaderTableSection> headers = new List<HeaderTableSection>();
		public bool EditMode;

		public ArterTableSource(ArterTableScreen controller)
		{
			_controller = controller;

			foreach(var ag in JaktLoggApp.instance.ArtGroupList){
				if( ag.ID == 100)
					headers.Add(new HeaderTableSection(Utils.Translate("specie.mygroup")));
				else
					headers.Add(new HeaderTableSection(ag.Navn));
			}

		}

		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
		{
			var artsInSection = JaktLoggApp.instance.ArtList.Where(a => a.GroupId == 100).Count();
			if(indexPath.Section == 0 && indexPath.Row == artsInSection)
			    return UITableViewCellEditingStyle.Insert;
			return UITableViewCellEditingStyle.Delete;
		}
		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			switch (editingStyle) {
			case UITableViewCellEditingStyle.Delete:

				// remove the item from the underlying data source
				var groupId = JaktLoggApp.instance.ArtGroupList[indexPath.Section].ID;
				var artsInSection = JaktLoggApp.instance.ArtList.Where(a => a.GroupId == groupId);
				JaktLoggApp.instance.DeleteArt(artsInSection.ElementAt(indexPath.Row));
				// delete the row from the table
				//tableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
				_controller.TableView.ReloadData();
				break;
			case UITableViewCellEditingStyle.None:
				break;
			case UITableViewCellEditingStyle.Insert:
				//create new item
				_controller.ShowAddScreeen();
				break;
			}
		}
		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			//var artsInSection = JaktLoggApp.instance.ArtList.Where(a => a.GroupId == 100).Count();
			if(indexPath.Section == 0)
				return true;

			return false;
		}
		public override string TitleForDeleteConfirmation (UITableView tableView, NSIndexPath indexPath)
		{   // Optional – default text is 'Delete'
			return Utils.Translate("delete");
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("ArterTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Default, "ArterTableCell");

			var groupId = JaktLoggApp.instance.ArtGroupList[indexPath.Section].ID;
			var artsInSection = JaktLoggApp.instance.ArtList.Where(a => a.GroupId == groupId);
			var c = artsInSection.Count();
			//legg til art - knapp
			if(_controller.TableView.Editing && groupId == 100 && indexPath.Row == c){
				cell.TextLabel.Text = Utils.Translate("specie.new");
				//cell.Accessory = UITableViewCellAccessory.None;
				cell.ImageView.Image = null;
			}
			else
			{
				var art = artsInSection.ElementAt(indexPath.Row);
				var label = art.Navn;

				var icon = JaktLoggApp.instance.SelectedArtIds.Contains(art.ID) ? "icon_checked.png" : "icon_unchecked.png";
				var file = "Images/Icons/"+icon;
				cell.ImageView.Image = new UIImage(file);
				cell.ImageView.Layer.MasksToBounds = true;
				cell.ImageView.Layer.CornerRadius = 5.0f;

				cell.TextLabel.Text = label;
				cell.TextLabel.TextAlignment = UITextAlignment.Left;
				if(!EditMode)
					cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
			}
			return cell;
		}
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			if(section == 0 && JaktLoggApp.instance.ArtList.Where(g => g.GroupId == 100).Count() == 0)
				return new UIView();

			return headers.ElementAt(section).View;
		}
		
		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			if(section == 0 && JaktLoggApp.instance.ArtList.Where(g => g.GroupId == 100).Count() == 0)
				return 0.0f;

			return 40.0f;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			if(_controller.TableView.Editing)
				return 1;

			return JaktLoggApp.instance.ArtGroupList.Count();
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			var groupid = JaktLoggApp.instance.ArtGroupList[section].ID;
			var rowsForSection = JaktLoggApp.instance.ArtList.Where(a => a.GroupId == groupid).Count();
			if(groupid == 100 && _controller.TableView.Editing)
				rowsForSection ++;

			return rowsForSection;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			if(section == 0)
			{
				var groupid = JaktLoggApp.instance.ArtGroupList[section].ID;
				var rowsForSection = JaktLoggApp.instance.ArtList.Where(a => a.GroupId == groupid).Count();
				if(!_controller.TableView.Editing && rowsForSection == 0)
					return Utils.Translate("specie.howtoadd");
			}
			return JaktLoggApp.instance.ArtGroupList[section].Navn;
		}
		public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
		{
			var artsInSection = JaktLoggApp.instance.ArtList.Where(a => a.GroupId == JaktLoggApp.instance.ArtGroupList[indexPath.Section].ID);
			var art = artsInSection.ElementAt(indexPath.Row);
			var artScreen = new ArtWebViewController(art);
			_controller.NavigationController.PushViewController(artScreen, true);
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var groupId = JaktLoggApp.instance.ArtGroupList[indexPath.Section].ID;
			var artsInSection = JaktLoggApp.instance.ArtList.Where(a => a.GroupId == groupId);

			if(groupId == 100 && indexPath.Row == artsInSection.Count())
			{

			}
			else
			{
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
}

