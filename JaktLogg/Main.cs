
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			tabBarController.TabBar.Items[0].Title = Utils.Translate("jakt.header");
			tabBarController.TabBar.Items[1].Title = Utils.Translate("setup.header");
			tabBarController.TabBar.Items[2].Title = Utils.Translate("species");
			tabBarController.TabBar.Items[3].Title = Utils.Translate("title_statistics");

			JaktLoggApp.instance.TabBarController = tabBarController;
			tabBarController.View.BackgroundColor = UIColor.Clear;
			imageView.UserInteractionEnabled = true;
			imageView.AddSubview (tabBarController.View);
			window.MakeKeyAndVisible ();
			
			//UIDevice.CurrentDevice.UniqueIdentifier
			
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
	}
}

