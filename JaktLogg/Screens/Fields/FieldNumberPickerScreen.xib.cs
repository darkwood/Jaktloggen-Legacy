
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FieldNumberPickerScreen: UIJaktViewController
	{
		public string Value = string.Empty;
		public string LabelExtension= string.Empty;
		//public int Rows = 10;
		
		private Action<FieldNumberPickerScreen> _callback;
		public FieldNumberPickerScreen (Action<FieldNumberPickerScreen> callback) : base("FieldNumberPickerScreen", null)
		{
			_callback = callback;
		}
		private UIPickerViewModel pickerViewModel;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var leftBtn = new UIBarButtonItem(Utils.Translate("cancel"), UIBarButtonItemStyle.Plain, CancelClicked);
			NavigationItem.LeftBarButtonItem = leftBtn;
			
			var rightBtn = new UIBarButtonItem(Utils.Translate("done"), UIBarButtonItemStyle.Done, DoneClicked);
			NavigationItem.RightBarButtonItem = rightBtn;
			
			pickerViewModel = new PickerViewModel(this);
			picker.Source = pickerViewModel;
			
			if(Value.Length == 1)
				Value = "00" + Value;
			
			if(Value.Length == 2)
				Value = "0" + Value;
			
			int cols = Value.Length;
			foreach(var c in Value.Reverse()){
				cols --;
				picker.Select(Int32.Parse(c.ToString()), cols, false); 
			}
			
			NumberDidChange();
			
		}
		
	
		private void CancelClicked(object sender, EventArgs args)
		{
			NavigationController.PopViewControllerAnimated(true);
		}
		
		private void DoneClicked(object sender, EventArgs args)
		{
			var number = GetNumberStringFromPicker();
			SaveAndClose(Int32.Parse(number).ToString());
		}
		public void SaveAndClose(string value)
		{
			Value = value;
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}
		
		public void NumberDidChange()
		{
			Value = GetNumberStringFromPicker();
			lblNumber.Text = GetNumberStringFromPicker() + LabelExtension;
		}
		
		public string GetNumberStringFromPicker(){
			var number = "0";
			for( var i = 0; i< picker.NumberOfComponents; i++){
				number += picker.SelectedRowInComponent(i);
			}
			return Int32.Parse(number).ToString();
		}
	}
	
	public class PickerViewModel : UIPickerViewModel
	{
		private FieldNumberPickerScreen _controller;
		public PickerViewModel(FieldNumberPickerScreen controller) : base()
		{
			_controller = controller;
		}
		
		public override void Selected (UIPickerView picker, int row, int component)
		{
			_controller.NumberDidChange();
		}
		
		public override int GetComponentCount (UIPickerView picker)
		{
			return 3;
		}
		
		public override int GetRowsInComponent (UIPickerView picker, int component)
		{
			return 10;
		}
		
		public override string GetTitle (UIPickerView picker, int row, int component)
		{
			return row.ToString();
		}
		
		
	}
}

