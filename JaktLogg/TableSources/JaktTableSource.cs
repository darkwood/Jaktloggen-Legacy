
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class JaktTableSource : UITableViewSource 
	{
		private JaktTableScreen _controller;
		
		public List<Jakt> JaktList {
			get{
				return _jaktList.OrderByDescending(o => o.DatoFra).ToList<Jakt>();
			}
			set{
				_jaktList = value;
			}
		}
		private List<Jakt> _jaktList = new List<Jakt>();
		
		public JaktTableSource(JaktTableScreen controller)
		{
			_controller = controller;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var row = indexPath.Row;
			var section = indexPath.Section;

			var datelist = (from item in JaktList
								select item.DatoFra.Year).Distinct().ToList();
			var currentItems = JaktList.Where(l => l.DatoFra.Year == datelist.ElementAt(section));
			
			var jakt = currentItems.ElementAt(row);
			
			var cell = tableView.DequeueReusableCell("JaktTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Subtitle, "JaktTableCell");
			
			cell.TextLabel.Text = jakt.DatoFra.ToNorwegianDateString();
			cell.DetailTextLabel.Text = jakt.Sted;
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			
			var imgstr = Utils.GetPath("jakt_"+jakt.ID+".jpg");
			
			if(!File.Exists(imgstr)){
				imgstr = "Images/Icons/pictureframe.png";
				cell.ImageView.Image = new UIImage(imgstr);
			}
			else
				cell.ImageView.Image = new UIImage(Utils.GetPath(imgstr));
			
			cell.ImageView.Layer.MasksToBounds = true;
			cell.ImageView.Layer.CornerRadius = 5.0f;
			
			return cell;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			var datelist = (from item in JaktList
								select item.DatoFra.Year).ToList();
			return datelist.Distinct().ElementAt(section).ToString();
		}
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			
			var title = TitleForHeader(tableView, section);
			var hView = new HeaderTableSection(title);
			return hView.View;
		}
		
		public override float GetHeightForHeader (UITableView tableView, int section)
		{		
			return 40.0f;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			var datelist = (from item in JaktList
								select item.DatoFra.Year).ToList();
			

			return datelist.Distinct().Count();
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			var datelist = (from item in JaktList
								select item.DatoFra.Year).Distinct().ToList();
			var currentItems = JaktList.Where(l => l.DatoFra.Year == datelist.ElementAt(section));
			
			return currentItems.Count();
			
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var row = indexPath.Row;
			var section = indexPath.Section;
			
			var datelist = (from item in JaktList
								select item.DatoFra.Year).Distinct().ToList();
			var currentItems = JaktList.Where(l => l.DatoFra.Year == datelist.ElementAt(section));
			
			var jakt = currentItems.ElementAt(row);
			var jaktItemScreen = new JaktItemScreen(jakt, screen =>{
				_controller.Refresh();
			});
			_controller.NavigationController.PushViewController(jaktItemScreen, true);
		}
		
		
		public override string TitleForDeleteConfirmation (UITableView tableView, NSIndexPath indexPath)
		{
			return "Slett";
		}
		
		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			
			var actionSheet = new UIActionSheet("") {"Slett", "Avbryt"};
			actionSheet.Title = "Alle logger under denne jakten blir også slettet.";
			actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 1;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object sender, UIButtonEventArgs e) {
				Console.WriteLine(e.ButtonIndex);
				switch (e.ButtonIndex)
				{
				case 0:
					//Slett
					var jaktItem = JaktList.ElementAt(indexPath.Row);
					JaktLoggApp.instance.DeleteJakt(jaktItem);
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

