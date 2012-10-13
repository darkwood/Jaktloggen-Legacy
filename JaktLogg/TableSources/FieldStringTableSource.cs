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
		public List<ItemCount> AutoSuggestions = new List<ItemCount>();
		
		public FieldStringTableSource (FieldStringScreen tableViewController) 
		{
			_tableViewController = tableViewController;
		}
		
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			if(section == 1) //autosuggest section
				return AutoSuggestions.Count;
			
			return 1;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 2;
		}
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			return new HeaderTableSection(TitleForHeader(tableView, section)).View;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			if(section == 1 && AutoSuggestions.Count > 0) //autosuggest
				return Utils.Translate("jakt.location.earlierused");
			
			return "";
		}
		
		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			if(section == 1 && AutoSuggestions.Count > 0)
				return 40.0f;
			
			return 0.0f;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			
			if(indexPath.Section == 0)
			{
				//Input-box
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
				//Load autosuggestions
				var cell = tableView.DequeueReusableCell("FieldAutoSuggestCell");
				if(cell == null)
					cell = new UIJaktTableViewCell(UITableViewCellStyle.Value1, "FieldAutoSuggestCell");
				
				var suggestion = AutoSuggestions.ElementAt(indexPath.Row);
				cell.TextLabel.Text = suggestion.Name;
				cell.DetailTextLabel.Text = suggestion.Count.ToString();
				return cell;
				
			}
			/*else
			{
				//Cancel-button
				var cell = tableView.DequeueReusableCell("FieldCancelCell");
				if(cell == null)
					cell = new UIJaktTableViewCell(UITableViewCellStyle.Default, "FieldCancelCell");
				
				cell.TextLabel.Text = Utils.Translate("cancel");
				cell.TextLabel.TextAlignment = UITextAlignment.Center;
				cell.Hidden = _tableViewController.NavigationController != null;
				return cell;
			}*/
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			
			if(indexPath.Section == 1)
			{
				controller.TextField.Text = Utils.UppercaseFirst(AutoSuggestions.ElementAt(indexPath.Row).Name);
				SaveText(controller.TextField);
			}
			/*else if(indexPath.Section == 2)
			{
				_tableViewController.Cancel();
			}*/
			tableView.DeselectRow(indexPath, false);
		}
		
		private bool SaveText (UITextField tf)
		{
			/*if(tf.Text.Length == 0)
			{
				MessageBox.Show("Feltet er tomt", "Skriv inn en verdi og prøv igjen.");
				return false;
			}*/
			
		    tf.ResignFirstResponder ();
			_tableViewController.SaveAndClose(tf.Text);
			return true;
		}
	}
}

