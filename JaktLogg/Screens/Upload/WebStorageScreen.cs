
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class WebStorageScreen : UIJaktViewController
	{
		public WebStorageScreen () : base ("WebStorageScreen", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var v = new UIPopupLoginView("Test", "message blafdkglasdf slkdfjgsldkfjgsldkf gjsdlfkg sdflg", null, "Avbryt", null);
			v.Show();
		}

		void HandleSave ()
		{
			throw new NotImplementedException ();
		}

		void HandleCanceled ()
		{
			var a = new UIPopupLoginView("Avbrøt", "Test test here...", null, "Avbryt", null);
			a.Show();
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

