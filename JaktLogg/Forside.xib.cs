
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class Forside : UIJaktViewController
	{
		[Export("initWithCoder:")]
		public Forside (NSCoder coder) : base(coder)
		{
			
		}
		
		public Forside () : base("Forside", null)
		{
			
		}

		
		public override void ViewDidLoad ()
		{
			Title = "Om Jaktloggen";
			
			buttonRating.SetBackgroundImage(new UIImage("Images/Buttons/Gray.png").StretchableImage(10, 0), UIControlState.Normal);
			buttonFeedback.SetBackgroundImage(new UIImage("Images/Buttons/Gray.png").StretchableImage(10, 0), UIControlState.Normal);

			
			buttonRating.TouchUpInside += HandleButtonRatingTouchUpInside;
			buttonFeedback.TouchUpInside += HandleButtonFeedbackTouchUpInside;
			
			base.ViewDidLoad ();
		}

		void HandleButtonFeedbackTouchUpInside (object sender, EventArgs e)
		{
			var webView = new FeedbackScreen();
			webView.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
			this.PresentModalViewController(webView, true);
		}

		void HandleButtonRatingTouchUpInside (object sender, EventArgs e)
		{
			var urlstr = "itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id={0}";
			NSUrl url = new NSUrl(string.Format(urlstr, "454071716"));
			if(!UIApplication.SharedApplication.OpenUrl(url))
				MessageBox.Show("Kan ikke åpne side", "Du må ha tilgang til internett for denne tjenesten.");
			
			
			/*
			 itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=337064413
			 */
		}
	}
}

