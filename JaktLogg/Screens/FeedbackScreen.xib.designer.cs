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
	[MonoTouch.Foundation.Register("FeedbackScreen")]
	public partial class FeedbackScreen {
		
		private MonoTouch.UIKit.UIView __mt_view;
		
		private MonoTouch.UIKit.UIWebView __mt_webView;
		
		private MonoTouch.UIKit.UIBarButtonItem __mt_buttonDone;
		
		private MonoTouch.UIKit.UIActivityIndicatorView __mt_activityIndicator;
		
		private MonoTouch.UIKit.UIBarButtonItem __mt_buttonRefresh;
		
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
		
		[MonoTouch.Foundation.Connect("webView")]
		private MonoTouch.UIKit.UIWebView webView {
			get {
				this.__mt_webView = ((MonoTouch.UIKit.UIWebView)(this.GetNativeField("webView")));
				return this.__mt_webView;
			}
			set {
				this.__mt_webView = value;
				this.SetNativeField("webView", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("buttonDone")]
		private MonoTouch.UIKit.UIBarButtonItem buttonDone {
			get {
				this.__mt_buttonDone = ((MonoTouch.UIKit.UIBarButtonItem)(this.GetNativeField("buttonDone")));
				return this.__mt_buttonDone;
			}
			set {
				this.__mt_buttonDone = value;
				this.SetNativeField("buttonDone", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("activityIndicator")]
		private MonoTouch.UIKit.UIActivityIndicatorView activityIndicator {
			get {
				this.__mt_activityIndicator = ((MonoTouch.UIKit.UIActivityIndicatorView)(this.GetNativeField("activityIndicator")));
				return this.__mt_activityIndicator;
			}
			set {
				this.__mt_activityIndicator = value;
				this.SetNativeField("activityIndicator", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("buttonRefresh")]
		private MonoTouch.UIKit.UIBarButtonItem buttonRefresh {
			get {
				this.__mt_buttonRefresh = ((MonoTouch.UIKit.UIBarButtonItem)(this.GetNativeField("buttonRefresh")));
				return this.__mt_buttonRefresh;
			}
			set {
				this.__mt_buttonRefresh = value;
				this.SetNativeField("buttonRefresh", value);
			}
		}
	}
}
