
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public partial class HeaderLoggItem: UIJaktViewController
	{
		public EventHandler HandleButtonImageTouchUpInside;
		public EventHandler HandleButtonJegerTouchUpInside;
		public EventHandler HandleButtonDogTouchUpInside;
		private Logg _logg;
		public HeaderLoggItem (Logg logg) : base("HeaderLoggItem", null)
		{
			_logg = logg;
		}

		public override void ViewDidAppear (bool animated)
		{
			var imgstr = Utils.GetPath("jaktlogg_"+_logg.ID+".jpg");
			var dogstr = Utils.GetPath("dog_"+_logg.DogId+".jpg");
			var jegerstr = Utils.GetPath("jeger_"+_logg.JegerId+".jpg");
			
			SetButtonImage(buttonImage, imgstr, "Images/Icons/pictureplaceholder.png");
			SetButtonImage(buttonImageDog, dogstr, "Images/Icons/dogplaceholder.png");
			SetButtonImage(buttonImageJeger, jegerstr, "Images/Icons/jegerplaceholder.png");

			
			if(_logg.DogId > 0)
				lblHund.Text =  JaktLoggApp.instance.GetDog(_logg.DogId).Navn;
			
			if(_logg.JegerId > 0)
				lblJeger.Text =  JaktLoggApp.instance.GetJeger(_logg.JegerId).Fornavn;

			base.ViewDidAppear (animated);
		}
		public override void ViewDidLoad ()
		{

			
			buttonImage.TouchUpInside += HandleButtonImageTouchUpInside;
			buttonImageDog.TouchUpInside += delegate(object sender, EventArgs e) {
				Console.WriteLine("ok");
				HandleButtonDogTouchUpInside(sender, e);
			};
			buttonImageJeger.TouchUpInside += HandleButtonJegerTouchUpInside;

			base.ViewDidLoad ();
		}
		
		private void SetButtonImage(UIButton button, string imageurl, string placeholderurl)
		{
			if(File.Exists(imageurl))
				button.SetImage(new UIImage(imageurl), UIControlState.Normal);
			else if(File.Exists(placeholderurl))
				button.SetImage(new UIImage(placeholderurl), UIControlState.Normal);
			
			button.Layer.MasksToBounds = true;
			button.Layer.CornerRadius = 5.0f;
		}
	}
}

