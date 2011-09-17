
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
			//buttonRating.TouchUpInside += HandleButtonRatingTouchUpInside;
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
			var urlstr = "https://userpub.itunes.apple.com/WebObjects/MZUserPublishing.woa/wa/addUserReview?id={0}&type=Purple+Software";
			NSUrl url = new NSUrl(string.Format(urlstr, "454071716"));
			if(!UIApplication.SharedApplication.OpenUrl(url))
				MessageBox.Show("Kan ikke åpne side", "Du må ha tilgang til internett for denne tjenesten.");
			
		}
	}
}

