
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
		public Dog SelectedDog;
		public FooterStatsArter (Action<FooterStatsArter> callback, Jeger selectedJeger, Dog selectedDog) : base("FooterStatsArter", null)
		{
			_callback = callback;	
			SelectedJeger = selectedJeger;
			SelectedDog = selectedDog;
		}
		
		public override void ViewDidLoad ()
		{
			pickerView.Model = new FooterStatsArterPickerViewModel(this);
			btDone.Clicked += HandleBtDoneClicked;
			btDone.Title = Utils.Translate("done");
			base.ViewDidLoad ();
		}

		void HandleBtDoneClicked (object sender, EventArgs e)
		{
			_callback(this);
			this.DismissModalViewControllerAnimated(true);
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
			
			if(SelectedDog != null)
			{
				var row = 0;
				foreach(var d in JaktLoggApp.instance.DogList){
					row ++;
					if(d.ID == SelectedDog.ID)
						break;
				}
				pickerView.Select(row , 1, true);
			}
			
			
			base.ViewWillAppear (animated);
		}
		
		public void JegerSelected(Jeger j)
		{
			SelectedJeger = j;
		}
		public void DogSelected(Dog d){
			SelectedDog = d;
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
			if(component == 0){
				if(row != 0)
					_controller.JegerSelected(JaktLoggApp.instance.JegerList[row-1]);
				else
					_controller.JegerSelected(null);
			}
			else if (component == 1){
				if(row != 0)
					_controller.DogSelected(JaktLoggApp.instance.DogList[row-1]);
				else
					_controller.DogSelected(null);
			}
			
		}
		public override int GetComponentCount (UIPickerView picker)
		{
			return 2;
		}
		
		public override int GetRowsInComponent (UIPickerView picker, int component)
		{
			if(component == 0)
				return JaktLoggApp.instance.JegerList.Count() + 1;
			else
				return JaktLoggApp.instance.DogList.Count() + 1;
		} 
		
		public override string GetTitle (UIPickerView picker, int row, int component)
		{
			if(component == 0){
				if(row == 0)
					return Utils.Translate("all_hunters");	
				else
					return JaktLoggApp.instance.JegerList[row-1].Navn;
			}
			else{
				if(row == 0)
					return Utils.Translate("all_dogs");	
				else
					return JaktLoggApp.instance.DogList[row-1].Navn;
			}
			
		}
	}
	
	
}

