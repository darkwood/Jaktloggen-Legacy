using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public class LoadingView 
	{
	    private static UIActivityIndicatorView _activityView;
	
		private static UIAlertView _alertView;
		
		public static void Show()
		{
			
			Show(Utils.Translate("defaultmessage"));
		}
		
	    public static void Show(string title)
	    {
			if(_alertView != null){
				_alertView.Title = title;
				return;
			}
			_alertView = new UIAlertView();
			
	    	_alertView.Title = title;
	    	_alertView.Show();
			
			_activityView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			
	    	_activityView.Frame = new RectangleF((_alertView.Bounds.Width / 2) - 15, _alertView.Bounds.Height - 50, 30, 30);
	    	_activityView.StartAnimating();
			
			_alertView.AddSubview(_activityView);
	    }
	
	    public static void Hide()
	    {
			if (_alertView != null)
				_alertView.DismissWithClickedButtonIndex(0, false);
	    }
	}
	
}

