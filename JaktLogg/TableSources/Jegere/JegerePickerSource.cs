
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class JegerePickerSource : UITableViewSource 
	{
		private JegerPickerScreen _controller;
		public List<Jeger> JegerList { get{ return JaktLoggApp.instance.JegerList; } }
		public List<int> JegerIds = new List<int>();
		private HeaderTableSection footer;

		public JegerePickerSource(JegerPickerScreen controller)
		{
			_controller = controller;
			footer = new HeaderTableSection(_controller.Footer);
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("JegereTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Default, "JegereTableCell");
			
			var jeger = JegerList.ElementAt(indexPath.Row);
			var label = jeger.Navn;
			
			var icon = JegerIds.Contains(jeger.ID) ? "icon_checked.png" : "icon_unchecked.png";
			var file = "Images/Icons/"+icon;
			cell.ImageView.Image = new UIImage(file);
			
			cell.TextLabel.Text = label;
			cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
			
			return cell;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return _controller.Header;
		}
		
		public override string TitleForFooter (UITableView tableView, int section)
		{
			return _controller.Footer;
		}
		
		public override UIView GetViewForFooter (UITableView tableView, int section)
		{
			return footer.View;
		}
		
		public override float GetHeightForFooter (UITableView tableView, int section)
		{
			if(!string.IsNullOrEmpty(_controller.Footer))
				return 40.0f;
			
			return 0.0f;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return JegerList.Count;
		}
		
		public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
		{
			var jeger = JegerList.ElementAt(indexPath.Row);
			var jegerScreen = new JegerScreen(jeger, screen => {
				Jeger j = screen.jeger;
				JaktLoggApp.instance.SaveJegerItem(j);
			});
			_controller.NavigationController.PushViewController(jegerScreen, true);
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var jeger = JegerList.ElementAt(indexPath.Row);
			
			
			if(JegerIds.Contains(jeger.ID))
			{
				for(var i=0; i<JegerIds.Count; i++)
				{
					if(JegerIds[i] == jeger.ID)
					{
						JegerIds.RemoveAt(i);
					}
				}
			}
			else
			{
				JegerIds.Add(JegerList.ElementAt(indexPath.Row).ID);
			}
			
			tableView.DeselectRow(indexPath, true);
			_controller.jegerIds = JegerIds;
			
			var icon = JegerIds.Contains(jeger.ID) ? "icon_checked.png" : "icon_unchecked.png";
			var file = "Images/Icons/"+icon;
			tableView.CellAt(indexPath).ImageView.Image = new UIImage(file);
			
		}
		
		public override string TitleForDeleteConfirmation (UITableView tableView, NSIndexPath indexPath)
		{
			return Utils.Translate("delete");
		}
		
		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			var jeger = JegerList.ElementAt(indexPath.Row);
			
			var actionSheet = new UIActionSheet("") {"Slett jeger", Utils.Translate("cancel")};
			actionSheet.Title = jeger.Navn + " vil bli slettet og fjernet fra alle logger.";
			actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 1;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object sender, UIButtonEventArgs e) {
				Console.WriteLine(e.ButtonIndex);
				switch (e.ButtonIndex)
				{
				case 0:
					//Slett
					JaktLoggApp.instance.DeleteJeger(jeger);
					_controller.Refresh();
					break;
				case 1:
					//Avbryt
					break;
				}
			};
		}
	}
}

