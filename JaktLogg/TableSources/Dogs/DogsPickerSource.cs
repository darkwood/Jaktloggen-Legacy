
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class DogsPickerSource : UITableViewSource 
	{
		private DogPickerScreen _controller;
		public List<Dog> DogList { get{ return JaktLoggApp.instance.DogList; } }
		public List<int> DogIds = new List<int>();
		private HeaderTableSection footer;

		public DogsPickerSource(DogPickerScreen controller)
		{
			_controller = controller;
			footer = new HeaderTableSection(_controller.Footer);

		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("DogsTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Default, "DogsTableCell");
			
			var dog = DogList.ElementAt(indexPath.Row);
			var label = dog.Navn;
			
			var icon = DogIds.Contains(dog.ID) ? "icon_checked.png" : "icon_unchecked.png";
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
			return DogList.Count;
		}
		
		public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
		{
			var dog = DogList.ElementAt(indexPath.Row);
			var dogScreen = new DogScreen(dog, screen => {
				Dog j = screen.dog;
				JaktLoggApp.instance.SaveDogItem(j);
			});
			_controller.NavigationController.PushViewController(dogScreen, true);
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var dog = DogList.ElementAt(indexPath.Row);
			
			
			if(DogIds.Contains(dog.ID))
			{
				for(var i=0; i<DogIds.Count; i++)
				{
					if(DogIds[i] == dog.ID)
					{
						DogIds.RemoveAt(i);
					}
				}
			}
			else
			{
				DogIds.Add(DogList.ElementAt(indexPath.Row).ID);
			}
			
			tableView.DeselectRow(indexPath, true);
			_controller.dogIds = DogIds;
			
			var icon = DogIds.Contains(dog.ID) ? "icon_checked.png" : "icon_unchecked.png";
			var file = "Images/Icons/"+icon;
			tableView.CellAt(indexPath).ImageView.Image = new UIImage(file);
			
		}
		
		public override string TitleForDeleteConfirmation (UITableView tableView, NSIndexPath indexPath)
		{
			return Utils.Translate("delete");
		}
		
		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			var dog = DogList.ElementAt(indexPath.Row);
			
			var actionSheet = new UIActionSheet("") {"Slett hund", Utils.Translate("cancel")};
			actionSheet.Title = dog.Navn + " vil bli slettet og fjernet fra alle logger.";
			actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 1;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object sender, UIButtonEventArgs e) {
				Console.WriteLine(e.ButtonIndex);
				switch (e.ButtonIndex)
				{
				case 0:
					//Slett
					JaktLoggApp.instance.DeleteDog(dog);
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

