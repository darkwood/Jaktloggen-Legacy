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
	[MonoTouch.Foundation.Register("FooterStatsArter")]
	public partial class FooterStatsArter {
		
		private MonoTouch.UIKit.UIView __mt_view;
		
		private MonoTouch.UIKit.UIPickerView __mt_pickerView;
		
		private MonoTouch.UIKit.UIBarButtonItem __mt_btDone;
		
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
		
		[MonoTouch.Foundation.Connect("pickerView")]
		private MonoTouch.UIKit.UIPickerView pickerView {
			get {
				this.__mt_pickerView = ((MonoTouch.UIKit.UIPickerView)(this.GetNativeField("pickerView")));
				return this.__mt_pickerView;
			}
			set {
				this.__mt_pickerView = value;
				this.SetNativeField("pickerView", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("btDone")]
		private MonoTouch.UIKit.UIBarButtonItem btDone {
			get {
				this.__mt_btDone = ((MonoTouch.UIKit.UIBarButtonItem)(this.GetNativeField("btDone")));
				return this.__mt_btDone;
			}
			set {
				this.__mt_btDone = value;
				this.SetNativeField("btDone", value);
			}
		}
	}
}
