
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class MoreScreenTableSource : UITableViewSource 
	{
		private MoreTableScreen _controller;
		
		private List<SectionMapping> sections = new List<SectionMapping>();

		public MoreScreenTableSource(MoreTableScreen controller)
		{
			_controller = controller;
			var section1 = new SectionMapping("", "");
			sections.Add(section1);
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Mine felter",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var fieldScreen = new LoggSetupScreen(); 
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/Tabs/Felter.png"
			});
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Last opp data",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var fieldScreen = new UploadDataScreen(); 
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/Tabs/Jaktloggen.png"
			});
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Om Jaktloggen",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var fieldScreen = new Forside(); 
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/Tabs/Gevir.png"
			});
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("MoreTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Subtitle, "MoreTableCell");
			
			cell.TextLabel.Text = GetCellLabel(indexPath.Section, indexPath.Row);
			cell.DetailTextLabel.Text = GetCellValue(indexPath.Section, indexPath.Row);
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			
			string imgpath = GetImageFile(indexPath.Section, indexPath.Row);
			cell.ImageView.Image = new UIImage(imgpath);
		
			return cell;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return sections.Count;
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return sections.ElementAt(section).Rows.Count();
		}
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return sections.ElementAt(section).Header;
		}
		public override string TitleForFooter (UITableView tableView, int section)
		{
			return sections.ElementAt(section).Footer;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			sections.ElementAt(indexPath.Section).Rows.ElementAt(indexPath.Row).RowSelected();
		}
		
		
		private string GetCellLabel(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).Label;
		}
		
		private string GetCellValue(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).GetValue();
		}
		
		private string GetImageFile(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).ImageFile ?? "";
		}
	}
}

