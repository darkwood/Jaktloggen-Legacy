
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FooterStatsArter : UIJaktViewController
	{
		private Action<FooterStatsArter> _callback;
		public Jeger SelectedJeger;
		public FooterStatsArter (Action<FooterStatsArter> callback, Jeger selectedJeger) : base("FooterStatsArter", null)
		{
			_callback = callback;	
			SelectedJeger = selectedJeger;
		}
		
		public override void ViewDidLoad ()
		{
			pickerView.Model = new FooterStatsArterPickerViewModel(this);
			
			base.ViewDidLoad ();
		}
		
		public override void ViewWillAppear (bool animated)
		{
			if(SelectedJeger != null)
			{
				var row = 0;
				foreach(var j in JaktLoggApp.instance.JegerList){
					row ++;
					if(j.ID == SelectedJeger.ID)
						break;
				}
				pickerView.Select(row , 0, true);
			}
			
			base.ViewWillAppear (animated);
		}
		
		public void JegerSelected(Jeger j)
		{
			SelectedJeger = j;
			_callback(this);
			this.DismissModalViewControllerAnimated(true);
		}

	}
	
	
	
	// ========== PickerViewModel =========== 
	
	public class FooterStatsArterPickerViewModel : UIPickerViewModel
	{
		private FooterStatsArter _controller;
		public FooterStatsArterPickerViewModel(FooterStatsArter controller) : base()
		{
			_controller = controller;
			
		}
		
		public override void Selected (UIPickerView picker, int row, int component)
		{
			if(row == 0)
				_controller.JegerSelected(null);
			else{
				var j = JaktLoggApp.instance.JegerList[row-1];
				if(_controller.SelectedJeger == null || j.ID != _controller.SelectedJeger.ID)
					_controller.JegerSelected(j);
			}
		}
		public override int GetComponentCount (UIPickerView picker)
		{
			return 1;
		}
		
		public override int GetRowsInComponent (UIPickerView picker, int component)
		{
			return JaktLoggApp.instance.JegerList.Count() + 1;
		} 
		
		public override string GetTitle (UIPickerView picker, int row, int component)
		{
			if(row == 0)
				return "Alle jegere";	
			else
				return JaktLoggApp.instance.JegerList[row-1].Navn;
		}
	}
	
	
}

