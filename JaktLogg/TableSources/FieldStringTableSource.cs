using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public class FieldStringTableSource : UITableViewSource
	{
		public	 TextInputCell controller = new TextInputCell();
		private FieldStringScreen _tableViewController;
		
		public FieldStringTableSource (FieldStringScreen tableViewController) 
		{
			_tableViewController = tableViewController;
		}
		
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return 1;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 2;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			
			if(indexPath.Section == 0)
			{
				var cell = tableView.DequeueReusableCell("FieldStringCell");
				if(cell == null){
					
					NSBundle.MainBundle.LoadNib("TextInputCell", controller, null);
					cell = controller.Cell;
					controller.TextField.BecomeFirstResponder();
					controller.TextField.ReturnKeyType = UIReturnKeyType.Done;
					controller.TextField.ShouldReturn = SaveText;
					controller.TextField.KeyboardType = _tableViewController.KeyboardType;
				}
				
				controller.TextField.Text = _tableViewController.Value;
				controller.TextField.Placeholder = _tableViewController.Placeholder;
			
			return cell;
			}
			else
			{
				var cell = tableView.DequeueReusableCell("FieldCancelCell");
				if(cell == null)
					cell = new UIJaktTableViewCell(UITableViewCellStyle.Default, "FieldCancelCell");
				
				cell.TextLabel.Text = "Avbryt";
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
		
		private bool SaveText (UITextField tf)
		{
			if(tf.Text.Length == 0)
			{
				MessageBox.Show("Sted mangler", "Skriv inn et stedsnavn og prøv igjen.");
				return false;
			}
			
		    tf.ResignFirstResponder ();
			_tableViewController.SaveAndClose(tf.Text);
			return true;
		}
	}
}

