
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using System.IO;

namespace JaktLogg
{
	public partial class FieldImagePickerScreen: UIJaktViewController
	{
		private UIImagePickerController picker;
		private string _filename;
		private bool _imagePicked = false;
		private Action<FieldImagePickerScreen> _callback;
		public FieldImagePickerScreen (string title, string filename, Action<FieldImagePickerScreen> callback) : base("FieldImagePickerScreen", null)
		{
			Title = title;
			_filename = filename;
			_callback = callback;
		}
		
		
		
		public override void ViewDidLoad ()
		{
			var pickImageBarButton = new UIBarButtonItem(UIBarButtonSystemItem.Camera, ButtonPickImageClick);
			NavigationItem.RightBarButtonItem = pickImageBarButton;
			InitBarButtons();
			
			base.ViewDidLoad ();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			var imagePath = Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), _filename);
			if(File.Exists(imagePath))
			{
				imageView.Image = new UIImage(imagePath);
			}
			else if(!_imagePicked)
				PickImage();
			
			base.ViewDidAppear (animated);
		}
		
		void InitBarButtons(){
			var buttonTrash = new UIBarButtonItem(UIBarButtonSystemItem.Trash, HandleButtonTrashClicked);
			var imagePath = Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), _filename);
			
			if(!File.Exists(imagePath))
				toolbar.SetItems(new UIBarButtonItem[]{}, false);
			else
				toolbar.SetItems(new[]{buttonTrash}, false);
		}
		
		void HandleButtonTrashClicked (object sender, EventArgs e)
		{
			string filepath = Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), _filename);
			if(File.Exists(filepath))
			{
				var actionSheet = new UIActionSheet("") {Utils.Translate("delete"), Utils.Translate("cancel")};
				actionSheet.Title =  Utils.Translate("confirmdeleteimage");
				actionSheet.DestructiveButtonIndex = 0;
				actionSheet.CancelButtonIndex = 1;
				actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
				
				actionSheet.Clicked += delegate(object s, UIButtonEventArgs evt) {
					switch (evt.ButtonIndex)
					{
					case 0:
						//Slett
						File.Delete(filepath);
						_filename = string.Empty;
						imageView.Image = null;
						_callback(this);
						NavigationController.PopViewControllerAnimated(true);
						break;
					case 1:
						//Avbryt
						break;
					}
				};
			}
		}
		
		public void ButtonPickImageClick(object s , EventArgs args){
			PickImage();
		}
		
		public void PickImage(){

			loadView.StartAnimating();
			picker = new UIImagePickerController();
			picker.Delegate = new pickerDelegate(this);
			picker.AllowsEditing = true;
			var actionSheet = new UIActionSheet("") 
									{
										Utils.Translate("choosefromlibrary"), 
										Utils.Translate("takepicture"), 
										Utils.Translate("cancel")
									};
			actionSheet.Style = UIActionSheetStyle.Default;
			
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object sender, UIButtonEventArgs e) {
				switch (e.ButtonIndex)
				{
				case 0:
					//choose photo
					picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
					this.PresentModalViewController(picker, true);
					break;
				case 1:
					//take photo
					picker.SourceType = UIImagePickerControllerSourceType.Camera;
					this.PresentModalViewController(picker, true);
					break;
				case 2:
					//cancel
					NavigationController.PopViewControllerAnimated(true);
					break;
				}
			};
			
		}
		public void CancelImagePicker()
		{
			_imagePicked = true;
			loadView.StopAnimating();
		}
		public void SaveImage(UIImage image)
		{
			_imagePicked = true;

			string filepath = Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), _filename);
			   
			using (NSData imageData = image.AsJPEG())
			{
				NSError err = new NSError();
				imageData.Save(filepath, true, out err);
			}
			imageView.Image = image;
			
			loadView.StopAnimating();
			InitBarButtons();
			_callback(this);
			
			this.DismissModalViewControllerAnimated(true);
		}
		
		private class pickerDelegate : MonoTouch.UIKit.UIImagePickerControllerDelegate
		{
			private FieldImagePickerScreen _controller;
			
			public pickerDelegate (FieldImagePickerScreen controller):base()
			{
				_controller = controller;
				
			}
			
			public override void FinishedPickingImage (UIImagePickerController picker, UIImage image, NSDictionary editingInfo)
			{
				if (picker.SourceType == UIImagePickerControllerSourceType.Camera)
				{
					image.SaveToPhotosAlbum(delegate(UIImage savedImage, NSError error) {
						_controller.SaveImage(savedImage);
					});
				}
				else 
				{
					_controller.SaveImage(image);
				}
				picker.DismissModalViewControllerAnimated(true);
			}
			
			public override void Canceled (UIImagePickerController picker)
			{
				_controller.CancelImagePicker();
				_controller.DismissModalViewControllerAnimated(true);
			}
			
			private void ShowImage(UIImage image, UIImagePickerController picker)
			{
				if (this.HasHighResScreen())
					image = image.Scale(new SizeF(new PointF(640, 640)));
				
				_controller.imageView.Image = image;
				picker.DismissModalViewControllerAnimated(true);
			}	
			
			private bool HasHighResScreen()
			{
				var scale = UIScreen.MainScreen.Scale;
				if (scale > 1)
					return true;
				
				return false;
			}
		}
	}
}