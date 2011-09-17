
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

			var item = LoggTypes.ElementAt(indexPath.Row);
			var icon = JaktLoggApp.instance.SelectedLoggTypeIds.Contains(item.ID) ? "icon_checked.png" : "icon_unchecked.png";
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
			return 1;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			var rowsForSection = LoggTypes.Count();
			return rowsForSection;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			var title = "Tilpass loggen etter ditt behov";
			
			return title;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{	
			var item = LoggTypes.ElementAt(indexPath.Row);
			if(JaktLoggApp.instance.SelectedLoggTypeIds.Contains(item.ID))
			{
				for(var i=0; i<JaktLoggApp.instance.SelectedLoggTypeIds.Count; i++)
				{
					if(JaktLoggApp.instance.SelectedLoggTypeIds[i] == item.ID)
					{
						JaktLoggApp.instance.SelectedLoggTypeIds.RemoveAt(i);
					}
				}
			}
			else
			{
				JaktLoggApp.instance.SelectedLoggTypeIds.Add(item.ID);
			}
			
			tableView.DeselectRow(indexPath, true);
			
			var icon = JaktLoggApp.instance.SelectedLoggTypeIds.Contains(item.ID) ? "icon_checked.png" : "icon_unchecked.png";
			var file = "Images/Icons/"+icon;
			tableView.CellAt(indexPath).ImageView.Image = new UIImage(file);
			
			//Save the list:
			JaktLoggApp.instance.SaveSelectedLoggTypeIds();
		}
	}
}

