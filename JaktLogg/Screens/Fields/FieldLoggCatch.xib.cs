
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FieldLoggCatch: UIJaktViewController
	{
		private Action<FieldLoggCatch> _callback;
		public List<Art> Arter;
		public int CurrentArtId = 0;
		public int CurrentShots = 0;
		public int CurrentHits = 0;
		public int CurrentSeen = 0;
		#region Constructors

		public FieldLoggCatch (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public FieldLoggCatch (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		public FieldLoggCatch (string title, Action<FieldLoggCatch> callback) : base("FieldLoggCatch", null)
		{
			_callback = callback;
			Title = title;
		}

		void Initialize ()
		{
		}
		
		#endregion
		
		private FieldLoggCatchPickerViewModel pickerViewModel;
		
		public override void ViewDidLoad ()
		{
			Arter = JaktLoggApp.instance.ArtList.Where(a => JaktLoggApp.instance.SelectedArtIds.Contains(a.ID)).ToList<Art>();
			
			var leftBtn = new UIBarButtonItem(Utils.Translate("cancel"), UIBarButtonItemStyle.Plain, CancelClicked);
			NavigationItem.LeftBarButtonItem = leftBtn;
			
			var rightBtn = new UIBarButtonItem(Utils.Translate("done"), UIBarButtonItemStyle.Done, DoneClicked);
			NavigationItem.RightBarButtonItem = rightBtn;
			
			pickerViewModel = new FieldLoggCatchPickerViewModel(this);
			pickerView.Source = pickerViewModel;
			
			base.ViewDidLoad ();
			
		}
		
		public override void ViewDidAppear (bool animated)
		{
			Arter = JaktLoggApp.instance.ArtList.Where(a => JaktLoggApp.instance.SelectedArtIds.Contains(a.ID)).ToList<Art>();
			pickerView.ReloadAllComponents();
			SetSelectedComponents();
			
			base.ViewDidAppear (animated);
		}
		
		public void SetSelectedComponents ()
		{
			if(CurrentArtId > 0){
				var row = 1;
				foreach(var art in Arter){
					if(art.ID == CurrentArtId)
					{
						pickerView.Select(row, 0, true);
						break;
					}
					row ++;
				}
			}
			
			pickerView.Select(CurrentShots, 1, true);
			pickerView.Select(CurrentHits, 2, true);
			pickerView.Select(CurrentSeen, 3, true);
		}
		
		private void CancelClicked(object sender, EventArgs args)
		{
			NavigationController.PopViewControllerAnimated(true);
		}
		
		private void DoneClicked(object sender, EventArgs args)
		{
			SaveAndClose();
		}
		
		public void SaveAndClose()
		{
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}
	}
	
	// ========== PickerViewModel =========== 
	
	public class FieldLoggCatchPickerViewModel : UIPickerViewModel
	{
		private FieldLoggCatch _controller;
		
		public FieldLoggCatchPickerViewModel(FieldLoggCatch controller) : base()
		{
			_controller = controller;
		}
		
		public override void Selected (UIPickerView picker, int row, int component)
		{
			switch(component){
			case 0:
				_controller.CurrentArtId = row == 0 ? 0 : _controller.Arter[row-1].ID;
				break;
			case 1:
				_controller.CurrentShots = row;
				if(row < _controller.CurrentHits){
					//_controller.CurrentHits = row;
					//_controller.SetSelectedComponents();
				}
				
				break;
			case 2:
				_controller.CurrentHits = row;
				if(row > _controller.CurrentShots){
					//_controller.CurrentShots = row;
					//_controller.SetSelectedComponents();
				}
				
				break;
				
			case 3:
				_controller.CurrentSeen = row;
				break;
				
			default:
				break;
			}
		}
		public override int GetComponentCount (UIPickerView picker)
		{
			return 4;
		}
		
		public override float GetComponentWidth (UIPickerView picker, int component)
		{
			switch(component){
			case 0:
				return 130f;
			case 1:
				return 50f;
			case 2:
				return 50f;
			case 3:
				return 50f;
			default:
				return 10f;
			}
		}
		public override int GetRowsInComponent (UIPickerView picker, int component)
		{
			switch(component){
			case 0:
				return _controller.Arter.Count() + 1;
			case 1:
				return 101;
			case 2:
				return 101;
			case 3:
				return 101;
			default:
				return 0;
			}
		}
		
		public override string GetTitle (UIPickerView picker, int row, int component)
		{
			switch(component){
			case 0:
				return row == 0 ? Utils.Translate("choosespecies") : _controller.Arter[row-1].Navn;
			default:
				return row == 0 ? "-" : row.ToString();
			}
		}
	}
}


