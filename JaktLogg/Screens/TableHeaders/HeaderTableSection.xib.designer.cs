// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace JaktLogg {
	
	
	// Base type probably should be MonoTouch.UIKit.UIViewController or subclass
	[MonoTouch.Foundation.Register("HeaderTableSection")]
	public partial class HeaderTableSection {
		
		private MonoTouch.UIKit.UIView __mt_view;
		
		private MonoTouch.UIKit.UILabel __mt_label;
		
		#pragma warning disable 0169
		[MonoTouch.Foundation.Connect("view")]
		private MonoTouch.UIKit.UIView view {
			get {
				this.__mt_view = ((MonoTouch.UIKit.UIView)(this.GetNativeField("view")));
				return this.__mt_view;
			}
			set {
				this.__mt_view = value;
				this.SetNativeField("view", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("label")]
		private MonoTouch.UIKit.UILabel label {
			get {
				this.__mt_label = ((MonoTouch.UIKit.UILabel)(this.GetNativeField("label")));
				return this.__mt_label;
			}
			set {
				this.__mt_label = value;
				this.SetNativeField("label", value);
			}
		}
	}
}
