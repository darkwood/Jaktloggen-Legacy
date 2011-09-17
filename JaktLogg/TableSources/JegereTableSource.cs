
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using MonoTouch.CoreGraphics;

namespace JaktLogg
{
	public class JegereTableSource : UITableViewSource 
	{
		private JegereScreen _controller;
		private List<Jeger> JegerList { get{ return JaktLoggApp.instance.JegerList; } }
		public List<int> JegerIds = new List<int>();
		
		public JegereTableSource(JegereScreen controller)
		{
			_controller = controller;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("JegereTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Subtitle, "JegereTableCell");
			
			var jeger = JegerList.ElementAt(indexPath.Row);
			var label = jeger.Navn;
			
			//var imgpath = jeger.ImagePath;
			//if(imgpath == string.Empty)
			//	imgpath = "Images/Icons/icon_placeholder_jeger.png";
			
			var imgstr = Utils.GetPath("jeger_"+jeger.ID+".jpg");
			
			if(!File.Exists(imgstr)){
				imgstr = "Images/Icons/icon_placeholder_jeger.png";
				cell.ImageView.Image = new UIImage(imgstr);
			}
			else
				cell.ImageView.Image = new UIImage(Utils.GetPath(imgstr));
			
			cell.ImageView.Layer.MasksToBounds = true;
			cell.ImageView.Layer.CornerRadius = 5.0f;
			
			cell.TextLabel.Text = label;
			cell.DetailTextLabel.Text = jeger.Email;
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			
			return cell;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return JegerList.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "";//"Legg til eller rediger dine jaktkamerater her";
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var jeger = JegerList.ElementAt(indexPath.Row);
			var jegerScreen = new JegerScreen(jeger, screen => {
				Jeger j = screen.jeger;
				JaktLoggApp.instance.SaveJegerItem(j);
				
			});
			_controller.NavigationController.PushViewController(jegerScreen, true);
			
		}
		
		public override string TitleForDeleteConfirmation (UITableView tableView, NSIndexPath indexPath)
		{
			return "Slett";
		}
		
		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			var jeger = JegerList.ElementAt(indexPath.Row);
			
			var actionSheet = new UIActionSheet("") {"Slett jeger", "Avbryt"};
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

