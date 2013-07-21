// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace JaktLogg
{
	[Register ("FieldLoggCatch")]
	partial class FieldLoggCatch
	{
		[Outlet]
		MonoTouch.UIKit.UILabel lblSeen { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblHits { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblShots { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblChooseSpecies { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblInfo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIPickerView pickerView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblSeen != null) {
				lblSeen.Dispose ();
				lblSeen = null;
			}

			if (lblHits != null) {
				lblHits.Dispose ();
				lblHits = null;
			}

			if (lblShots != null) {
				lblShots.Dispose ();
				lblShots = null;
			}

			if (lblChooseSpecies != null) {
				lblChooseSpecies.Dispose ();
				lblChooseSpecies = null;
			}

			if (lblInfo != null) {
				lblInfo.Dispose ();
				lblInfo = null;
			}

			if (pickerView != null) {
				pickerView.Dispose ();
				pickerView = null;
			}
		}
	}
}
