using System;
using System.Net;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public class UIPopupLoginView : UIAlertView
	{
		private UITextField _UserName;
		private UITextField _Password;
		
		public UIPopupLoginView ()
		{
			InitializeControl ();
		}
		
		public UIPopupLoginView (string title, string message, UIAlertViewDelegate alertViewDelegate, string cancelBtnTitle, params string[] otherButtons) : base(title, message, alertViewDelegate, cancelBtnTitle, otherButtons)
		{
			InitializeControl ();
		}
		
		private void InitializeControl ()
		{
			this.AddButton("Ok");
			this.AddButton ("Cancel");
			this.CancelButtonIndex = 1;
			// shift the control up so the keyboard won't hide the control when activated.
			this.Transform = MonoTouch.CoreGraphics.CGAffineTransform.MakeTranslation (0f, 120f);
		}
		
		public string UserName {
			get { return string.IsNullOrEmpty(_UserName.Text)?"":_UserName.Text; }
		}
		
		public string Password {
			get { return string.IsNullOrEmpty(_Password.Text)?"":_Password.Text; }
		}
		
		public override void LayoutSubviews ()
		{
			// layout the stock UIAlertView control
			base.LayoutSubviews ();
			
			if(this.Subviews.Count() == 3)
			{
				// build out the text field
				_UserName = ComposeTextFieldControl (false);
				_Password = ComposeTextFieldControl (true);

				// add the text field to the alert view
				
				UIScrollView view = new UIScrollView(this.Frame);
				
				view.AddSubview(ComposeLabelControl("Username"));
				view.AddSubview (_UserName);
				view.AddSubview(ComposeLabelControl("Password"));
				view.AddSubview (_Password);
				
				this.AddSubview(view);
				
			}
			
			// shift the contents of the alert view around to accomodate the extra text field
			AdjustControlSize ();
			
		}
		
		private UITextField ComposeTextFieldControl (bool secureTextEntry)
		{
			UITextField textField = new UITextField (new System.Drawing.RectangleF (12f, 0f, 252f, 25f));
			textField.BackgroundColor = UIColor.White;
			textField.UserInteractionEnabled = true;
			textField.KeyboardType = UIKeyboardType.Default;
			textField.AutocorrectionType = UITextAutocorrectionType.No;
			textField.AutocapitalizationType = UITextAutocapitalizationType.None;
			textField.ReturnKeyType = secureTextEntry?UIReturnKeyType.Done:UIReturnKeyType.Next;
			textField.SecureTextEntry = secureTextEntry;
			return textField;
			
		}
		
		private UILabel ComposeLabelControl (string LabelText)
		{
			UILabel labelField = new UILabel(new System.Drawing.RectangleF (12f, 0f, 252f, 25f));
			labelField.UserInteractionEnabled = false;
			labelField.TextColor = UIColor.White;
			labelField.Text = LabelText;
			return labelField;
		}
		
		private void AdjustControlSize ()
		{
			float fStart = 5;
			float fInsideStart = 0;
			
			var view = this.Subviews[2];
			view.Frame = new RectangleF (view.Frame.X, fStart, view.Frame.Size.Width, view.Frame.Size.Height);
			fStart+=view.Frame.Size.Height;
			
			view = this.Subviews[3];
			view.Frame = new RectangleF(view.Frame.X,fStart,view.Frame.Size.Width,view.Frame.Size.Height);
			view.Alpha = this.Alpha;
			view.BackgroundColor = this.BackgroundColor;
			view.Opaque = false;
			
			view = this.Subviews[3].Subviews[0];
			view.Frame = new RectangleF (view.Frame.X, fInsideStart, view.Frame.Size.Width, view.Frame.Size.Height);
			fInsideStart+=view.Frame.Size.Height;
			view.Alpha = this.Alpha;
			view.BackgroundColor = this.BackgroundColor;
			view.Opaque = false;
			
			view = this.Subviews[3].Subviews[1];
			view.Frame = new RectangleF (view.Frame.X, fInsideStart, view.Frame.Size.Width, view.Frame.Size.Height);
			fInsideStart+=view.Frame.Size.Height;
			
			view = this.Subviews[3].Subviews[2];
			view.Frame = new RectangleF (view.Frame.X, fInsideStart, view.Frame.Size.Width, view.Frame.Size.Height);
			fInsideStart+=view.Frame.Size.Height;
			view.Alpha = this.Alpha;
			view.BackgroundColor = this.BackgroundColor;
			view.Opaque = false;
			
			view = this.Subviews[3].Subviews[3];
			view.Frame = new RectangleF (view.Frame.X, fInsideStart, view.Frame.Size.Width, view.Frame.Size.Height);
			fInsideStart+=view.Frame.Size.Height;
			
			fStart+=fInsideStart+5;
			
			view = this.Subviews[0];
			view.Frame = new RectangleF (view.Frame.X, fStart, view.Frame.Size.Width, view.Frame.Size.Height);
			
			view = this.Subviews[1];
			view.Frame = new RectangleF (view.Frame.X, fStart, view.Frame.Size.Width, view.Frame.Size.Height);
			fStart+=(view.Frame.Size.Height*2)-5;
			
			RectangleF frame = new RectangleF (this.Frame.X, 30, this.Frame.Size.Width, fStart);
			this.Frame = frame;
			
		}
		
	}
	
}