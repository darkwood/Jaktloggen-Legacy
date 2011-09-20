
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class FirstLoadLoggView : UIJaktViewController
	{
		private EventHandler HandleBtNewTouchUpInside;
		public FirstLoadLoggView (EventHandler buttonEvent) : base("FirstLoadLoggView", null)
		{
			HandleBtNewTouchUpInside = buttonEvent;
		}
		
		public override void ViewDidLoad ()
		{
			btNew.SetBackgroundImage(new UIImage("Images/Buttons/Gray.png").StretchableImage(10, 0), UIControlState.Normal);
			btNew.TouchUpInside += HandleBtNewTouchUpInside;
			
			var pics = new List<string>();
			pics.Add("Hunter");
			pics.Add("MountainsHunter");
			pics.Add("Raadyr");
			pics.Add("RypeHagle");
			
			var i = new Random().Next(pics.Count);
			Console.WriteLine(i);
			img.Image = new UIImage(string.Format("Images/Backgrounds/{0}.png", pics.ElementAt(i)));
			
			base.ViewDidLoad ();
		}
	}
}

