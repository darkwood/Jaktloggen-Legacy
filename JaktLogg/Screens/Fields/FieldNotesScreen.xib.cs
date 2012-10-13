
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FieldNotesScreen : UIJaktTableViewController
	{
		private Action<FieldNotesScreen> _callback;
		public UIKeyboardType KeyboardType = UIKeyboardType.Default;
		public string Value = "";
		public string Placeholder = Utils.Translate("entertext");
	
		public FieldNotesScreen (string title, Action<FieldNotesScreen> callback) : base("FieldNotesScreen", null)
		{
			Title = title;
			_callback = callback;
		}
		public FieldNotesScreen (Action<FieldNotesScreen> callback) : base("FieldNotesScreen", null)
		{
			_callback = callback;
		}
		
		
		private FieldNotesTableSource tableSource;
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var leftBtn = new UIBarButtonItem(Utils.Translate("cancel"), UIBarButtonItemStyle.Plain, CancelClicked);
			NavigationItem.LeftBarButtonItem = leftBtn;
			
			var rightBtn = new UIBarButtonItem(Utils.Translate("done"), UIBarButtonItemStyle.Done, DoneClicked);
			NavigationItem.RightBarButtonItem = rightBtn;
			
			tableSource = new FieldNotesTableSource(this);
			TableView.Source = tableSource;
			
		}
		
		private void CancelClicked(object sender, EventArgs args)
		{
			NavigationController.PopViewControllerAnimated(true);
		}
		
		public void Cancel()
		{
			if(NavigationController != null)
				NavigationController.PopViewControllerAnimated(true);
			else
				DismissModalViewControllerAnimated(true);
		}
		
		private void DoneClicked(object sender, EventArgs args)
		{
			SaveAndClose(tableSource.controller.TextView.Text);
		}
		public void SaveAndClose(string value)
		{
			Value = value;
			_callback(this);
			
			if(NavigationController == null)
				DismissModalViewControllerAnimated(false);
			else
				NavigationController.PopViewControllerAnimated(true);
		}
	}
	
	
	
	
	
	
	public class FieldNotesTableSource : UITableViewSource
	{
		public	 MultilineTextinputCell controller = new MultilineTextinputCell();
		private FieldNotesScreen _tableViewController;
		
		public FieldNotesTableSource (FieldNotesScreen tableViewController) 
		{
			_tableViewController = tableViewController;
		}
		
		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 180f;
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return 1;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			
			if(indexPath.Section == 0)
			{
				var cell = tableView.DequeueReusableCell("FieldStringCell");
				if(cell == null){
					
					NSBundle.MainBundle.LoadNib("MultilineTextinputCell", controller, null);
					cell = controller.Cell;
					controller.TextView.BecomeFirstResponder();
					controller.TextView.ReturnKeyType = UIReturnKeyType.Default;
					controller.TextView.ShouldEndEditing = SaveText;
					controller.TextView.KeyboardType = _tableViewController.KeyboardType;
				}
				
				controller.TextView.Text = _tableViewController.Value;
				//controller.TextView.Placeholder = _tableViewController.Placeholder;
			
			return cell;
			}
			else
			{
				var cell = tableView.DequeueReusableCell("FieldCancelCell");
				if(cell == null)
					cell = new UIJaktTableViewCell(UITableViewCellStyle.Default, "FieldCancelCell");
				
				cell.TextLabel.Text = Utils.Translate("cancel");
				cell.TextLabel.TextAlignment = UITextAlignment.Center;
				cell.Hidden = _tableViewController.NavigationController != null;
				return cell;
			}
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if(indexPath.Section == 1)
			{
				_tableViewController.Cancel();
			}
			tableView.DeselectRow(indexPath, false);
		}
		
		private bool SaveText (UITextView tf)
		{	
		    tf.ResignFirstResponder ();
			_tableViewController.SaveAndClose(tf.Text);
			return true;
		}
	}
}