
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using MonoTouch.CoreGraphics;

namespace JaktLogg
{
	public class DogsTableSource : UITableViewSource 
	{
		private DogsScreen _controller;
		private List<Dog> DogList { get{ return JaktLoggApp.instance.DogList; } }
		public List<int> DogIds = new List<int>();
		
		public DogsTableSource(DogsScreen controller)
		{
			_controller = controller;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("DogsTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Subtitle, "DogsTableCell");
			
			var dog = DogList.ElementAt(indexPath.Row);
			var label = dog.Navn;
				
			var imgstr = Utils.GetPath("dog_"+dog.ID+".jpg");
			
			if(!File.Exists(imgstr)){
				imgstr = "Images/Icons/Tabs/dog-paw.png";
				cell.ImageView.Image = new UIImage(imgstr);
			}
			else
				cell.ImageView.Image = new UIImage(Utils.GetPath(imgstr));
			
			cell.ImageView.Layer.MasksToBounds = true;
			cell.ImageView.Layer.CornerRadius = 5.0f;
			cell.TextLabel.Text = label;
			cell.DetailTextLabel.Text = dog.Rase;
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			
			return cell;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return DogList.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "";;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var dog = DogList.ElementAt(indexPath.Row);
			var dogScreen = new DogScreen(dog, screen => {
				Dog j = screen.dog;
				JaktLoggApp.instance.SaveDogItem(j);
				
			});
			_controller.NavigationController.PushViewController(dogScreen, true);
			
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

