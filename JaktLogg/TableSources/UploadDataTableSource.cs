using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class UploadDataTableSource : UITableViewSource 
	{
		private UploadDataScreen _controller;
		
		public UploadDataTableSource(UploadDataScreen controller)
		{
			_controller = controller;
		}
		
		private void HandleButtonUploadTouchUpInside (object sender, EventArgs e)
		{
			//Start uploading... in controller...
			_controller.HandleBtUploadTouchUpInside(sender, e);
		}
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			if(section > 0)
				return new HeaderTableSection(TitleForHeader(tableView, section)).View;
			
			var headerView = new UploadDataHeader("Henter data...");
			headerView.HandleButtonUploadTouchUpInside = HandleButtonUploadTouchUpInside;
			return headerView.View;
		}

		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			if(section > 0)
				return 0.0f;
			
			return 160f;
		}
		
		public override float GetHeightForFooter (UITableView tableView, int section)
		{
			return 0.0f;
		}
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return JaktLoggApp.instance.JaktList.Count();
		}
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "";
		}
		public override string TitleForFooter (UITableView tableView, int section)
		{
			return "";
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			//sections.ElementAt(indexPath.Section).Rows.ElementAt(indexPath.Row).RowSelected();
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("JaktItemCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Subtitle, "JaktItemCell");
			
			var j = JaktLoggApp.instance.JaktList.ElementAt(indexPath.Row);
			cell.TextLabel.Text = j.Sted;
			cell.DetailTextLabel.Text = j.DatoFra.ToNorwegianDateAndYearString();
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			var picturefile = Utils.GetPath("jakt_"+j.ID+".jpg");
			if(File.Exists(picturefile))
				cell.ImageView.Image = new UIImage(picturefile);
			
			return cell;
		}
	}
}

