
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FieldArtPickerScreen : UIJaktTableViewController
	{
		private string _label;
		private Action<FieldArtPickerScreen> _callback;
		public int SelectedArtId;
		private FieldArtPickerTableSource _tableSource;
		public FieldArtPickerScreen (string label, int selectedArtId, Action<FieldArtPickerScreen> callback) : base("FieldArtPickerScreen", null)
		{
			_label = label;
			SelectedArtId = selectedArtId;
			_callback = callback;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = _label;
			
			_tableSource = new FieldArtPickerTableSource(this);
			TableView.Source = _tableSource;
			
			_tableSource.ArtList = JaktLoggApp.instance.ArtList.Where(a => JaktLoggApp.instance.SelectedArtIds.Contains(a.ID)).ToList<Art>();
			TableView.ReloadData();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			_tableSource.ArtList = JaktLoggApp.instance.ArtList.Where(a => JaktLoggApp.instance.SelectedArtIds.Contains(a.ID)).ToList<Art>();
			TableView.ReloadData();
			base.ViewDidAppear (animated);
		}
		public void Save(){
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}
	}
	
	public class FieldArtPickerTableSource : UITableViewSource
	{
		private FieldArtPickerScreen _controller;
		public List<Art> ArtList = new List<Art>();
		
		public FieldArtPickerTableSource(FieldArtPickerScreen controller)
		{
			_controller = controller;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return ArtList.Count;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("ArtPickerCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Default, "ArtPickerCell");
			
			var currentArt = ArtList.ElementAt(indexPath.Row);
				
			cell.TextLabel.Text = currentArt.Navn;
			if(currentArt.ID == _controller.SelectedArtId){
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			}
			
			return cell;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			_controller.SelectedArtId = ArtList.ElementAt(indexPath.Row).ID;
			_controller.Save();
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return Utils.Translate("fieldartpickerscreen.header");
		}
		public override string TitleForFooter (UITableView tableView, int section)
		{
			return string.Format(Utils.Translate("fieldartpickerscreen.footer"), ArtList.Count);
		}
	}
}

