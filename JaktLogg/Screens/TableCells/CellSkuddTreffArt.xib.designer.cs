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
	[MonoTouch.Foundation.Register("CellSkuddTreffArt")]
	public partial class CellSkuddTreffArt {
		
		private MonoTouch.UIKit.UITableViewCell __mt_cell;
		
		private MonoTouch.UIKit.UIPickerView __mt_picker;
		
		#pragma warning disable 0169
		[MonoTouch.Foundation.Connect("cell")]
		private MonoTouch.UIKit.UITableViewCell cell {
			get {
				this.__mt_cell = ((MonoTouch.UIKit.UITableViewCell)(this.GetNativeField("cell")));
				return this.__mt_cell;
			}
			set {
				this.__mt_cell = value;
				this.SetNativeField("cell", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("picker")]
		private MonoTouch.UIKit.UIPickerView picker {
			get {
				this.__mt_picker = ((MonoTouch.UIKit.UIPickerView)(this.GetNativeField("picker")));
				return this.__mt_picker;
			}
			set {
				this.__mt_picker = value;
				this.SetNativeField("picker", value);
			}
		}
	}
}