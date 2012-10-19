
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
			var section2 = new SectionMapping("", "");
			var section3 = new SectionMapping("", "");

			
			section1.Rows.Add(new RowItemMapping {
				Label = "Jegere",
				GetValue = () => {
					return "Vis og rediger alle jegere";
				},
				RowSelected = () => {
					var fieldScreen = new JegereScreen(new List<int>(), screen => { });
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/Tabs/Jegere.png"
			});
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Hunder",
				GetValue = () => {
					return "Vis og rediger alle hunder";
				},
				RowSelected = () => {
					var fieldScreen = new DogsScreen(new List<int>(), screen => { });
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/Tabs/dog-paw.png"
			});
			/*
			section2.Rows.Add(new RowItemMapping {
				Label = "Last opp data",
				GetValue = () => {
					return "Under utvikling. Kun for testing.";
				},
				RowSelected = () => {
					var fieldScreen = new UploadJakterScreen(); 
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/Tabs/Jaktloggen.png"
			});
			*/
			section2.Rows.Add(new RowItemMapping {
				Label = "Om Jaktloggen",
				GetValue = () => {
					return "Gi tilbakemelding eller rating";
				},
				RowSelected = () => {
					var fieldScreen = new Forside(); 
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/Tabs/Gevir.png"
			});

			section2.Rows.Add(new RowItemMapping {
				Label = "Aktuelt",
				GetValue = () => {
					return "Oppdateringer og aktuelt";
				},
				RowSelected = () => {
					var fieldScreen = new FeedbackScreen("http://www.jaktloggen.no/aktuelt/", "Aktuelt"); 
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/Tabs/Gevir.png"
			});
			
			sections.Add(section1);
			sections.Add(section2);
			//sections.Add(section3);
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

