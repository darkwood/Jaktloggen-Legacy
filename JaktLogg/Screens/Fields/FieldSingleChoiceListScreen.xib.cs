
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FieldSingleChoiceListScreen : UIJaktTableViewController
	{
		private Action<FieldSingleChoiceListScreen> _callback;
		public List<SingleSelectRowItem> ItemList = new List<SingleSelectRowItem>();
		public FieldSingleChoiceListScreen (string title, List<SingleSelectRowItem> itemlist, Action<FieldSingleChoiceListScreen> callback) : base("FieldSingleChoiceListScreen", null)
		{
			Title = title;
			ItemList = itemlist;
			_callback = callback;
		}
		public string SelectedKey = "";
		
		public override void ViewDidLoad ()
		{
			var tableSource = new FieldSingleChoiceListTableSource(this);
			TableView.Source = tableSource;
			tableSource.ItemList = ItemList;
			TableView.ReloadData();
			
			var rightButton = new UIBarButtonItem(UIBarButtonSystemItem.Trash, SelectNoneClicked);
			if(ItemList.Count() > 0)
				NavigationItem.RightBarButtonItem = rightButton;
			else{			
				MessageBox.Show("Ingen jegere med i jakt", "For å legge til, gå tilbake til jaktsiden og velg \"Jegere\".");
			}	
			
			base.ViewDidLoad ();
		}
		
		private void SelectNoneClicked(object sender, EventArgs arg)
		{
			SelectedKey = "";
			_callback(this);
			TableView.ReloadData();
			//Save();
		}
		public void Save(){
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}
	}
	
	public class FieldSingleChoiceListTableSource : UITableViewSource
	{
		private FieldSingleChoiceListScreen _controller;
		public List<SingleSelectRowItem> ItemList = new List<SingleSelectRowItem>();
		
		public FieldSingleChoiceListTableSource(FieldSingleChoiceListScreen controller)
		{
			_controller = controller;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return ItemList.Count;
		}
		
		/*public override string TitleForHeader (UITableView tableView, int section)
		{
			var s = "";
			if(ItemList.Count == 0){
				s = "Ingen jegere er tatt med i denne jakta." + Environment.NewLine + Environment.NewLine;
				s += "Gå tilbake til hovedsiden for denne jakta og velg \"Jegere\" for å legge til.";
			}
			
			return s;
		}*/
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var currentItem = ItemList.ElementAt(indexPath.Row);
			var cell = tableView.DequeueReusableCell("ItemPickerCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(currentItem.CellStyle, "ItemPickerCell");
			
			cell.TextLabel.Text = currentItem.TextLabel;
			//cell.DetailTextLabel.Text = currentItem.Key.ToString();
			
			if(currentItem.Image != null)
				cell.ImageView.Image = currentItem.Image;
			
			if(currentItem.Key == _controller.SelectedKey){
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			}
			else
				cell.Accessory = UITableViewCellAccessory.None;
			
			return cell;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			_controller.SelectedKey = ItemList.ElementAt(indexPath.Row).Key;
			_controller.Save();
		}
	}
	
	public class SingleSelectRowItem
	{
		public string Key;
		public string TextLabel = string.Empty;
		public string DetailTextLabel = string.Empty;
		public UIImage Image;
		public UITableViewCellStyle CellStyle = UITableViewCellStyle.Default;
	}
}

