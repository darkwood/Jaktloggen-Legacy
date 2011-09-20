
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class LoggSetupTableSource : UITableViewSource 
	{
		private LoggSetupScreen _controller;
		
		public List<LoggType> LoggTypes = new List<LoggType>();
		
		public LoggSetupTableSource(LoggSetupScreen controller)
		{
			_controller = controller;
			
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("LoggSetupTableCell");
			if(cell == null)
					cell = new UIJaktTableViewCell(UITableViewCellStyle.Subtitle, "LoggSetupTableCell");
			
			var loggtypesInSection = LoggTypes.Where(a => a.GroupId == JaktLoggApp.instance.LoggTypeGroupList[indexPath.Section].ID);
			var item = loggtypesInSection.ElementAt(indexPath.Row);
			var icon = JaktLoggApp.instance.SelectedLoggTypeIds.Contains(item.Key) ? "icon_checked.png" : "icon_unchecked.png";
			var file = "Images/Icons/"+icon;
			cell.ImageView.Image = new UIImage(file);
			cell.ImageView.Layer.MasksToBounds = true;
			cell.ImageView.Layer.CornerRadius = 5.0f;
			cell.TextLabel.Text = item.Navn;
			cell.DetailTextLabel.Text = item.Beskrivelse;
			
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
			return JaktLoggApp.instance.LoggTypeGroupList.Count();
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			var rowsForSection = LoggTypes.Where(a => a.GroupId == JaktLoggApp.instance.LoggTypeGroupList[section].ID).Count();
			return rowsForSection;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			var title = JaktLoggApp.instance.LoggTypeGroupList[section].Navn;
			
			return title;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{	
			var itemsInSection = LoggTypes.Where(a => a.GroupId == JaktLoggApp.instance.LoggTypeGroupList[indexPath.Section].ID);
			var item = itemsInSection.ElementAt(indexPath.Row);
		
			if(JaktLoggApp.instance.SelectedLoggTypeIds.Contains(item.Key))
			{
				for(var i=0; i<JaktLoggApp.instance.SelectedLoggTypeIds.Count; i++)
				{
					if(JaktLoggApp.instance.SelectedLoggTypeIds[i] == item.Key)
					{
						JaktLoggApp.instance.SelectedLoggTypeIds.RemoveAt(i);
					}
				}
			}
			else
			{
				JaktLoggApp.instance.SelectedLoggTypeIds.Add(item.Key);
			}
			
			tableView.DeselectRow(indexPath, true);
			
			var icon = JaktLoggApp.instance.SelectedLoggTypeIds.Contains(item.Key) ? "icon_checked.png" : "icon_unchecked.png";
			var file = "Images/Icons/"+icon;
			tableView.CellAt(indexPath).ImageView.Image = new UIImage(file);
			
			//Save the list:
			JaktLoggApp.instance.SaveSelectedLoggTypeIds();
		}
	}
}

