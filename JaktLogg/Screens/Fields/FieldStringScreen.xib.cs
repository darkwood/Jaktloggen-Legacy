
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FieldStringScreen : UIJaktTableViewController
	{
		private Action<FieldStringScreen> _callback;
		public UIKeyboardType KeyboardType = UIKeyboardType.Default;
		public string Value = "";
		public string Placeholder = "Skriv inn tekst";
		public List<ItemCount> AutoSuggestions = new List<ItemCount>();
		
		public FieldStringScreen (string title, Action<FieldStringScreen> callback) : base("FieldStringScreen", null)
		{
			Title = title;
			_callback = callback;
		}
		public FieldStringScreen (Action<FieldStringScreen> callback) : base("FieldStringScreen", null)
		{
			_callback = callback;
		}
		
		
		private FieldStringTableSource tableSource;
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var leftBtn = new UIBarButtonItem("Avbryt", UIBarButtonItemStyle.Plain, CancelClicked);
			NavigationItem.LeftBarButtonItem = leftBtn;
			
			var rightBtn = new UIBarButtonItem("Ferdig", UIBarButtonItemStyle.Done, DoneClicked);
			NavigationItem.RightBarButtonItem = rightBtn;
			
			tableSource = new FieldStringTableSource(this);
			tableSource.AutoSuggestions = AutoSuggestions;
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
			SaveAndClose(tableSource.controller.TextField.Text);
		}
		public void SaveAndClose(string value)
		{
			Value = Utils.UppercaseFirst(value);
			_callback(this);
			
			if(NavigationController == null)
				DismissModalViewControllerAnimated(false);
			else
				NavigationController.PopViewControllerAnimated(true);
		}
	}
}

