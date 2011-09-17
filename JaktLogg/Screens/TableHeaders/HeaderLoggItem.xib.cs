
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
		private Logg _logg;
		public HeaderLoggItem (Logg logg) : base("HeaderLoggItem", null)
		{
			_logg = logg;
		}
		
		public override void ViewDidLoad ()
		{
			var ok = new UIImage("Images/Icons/icon_checked.png");
			var notOk = new UIImage("Images/Icons/icon_unchecked.png");
			
			imgArt.Image = _logg.ArtId > 0 ? ok : notOk;
			imgJeger.Image = _logg.JegerId > 0 ? ok : notOk;
			imgNotater.Image = _logg.Notes.Length > 0 ? ok : notOk;
			imgPosisjon.Image = _logg.Latitude.Length > 0 ? ok : notOk;
			imgSkudd.Image = _logg.Skudd > 0 ? ok : notOk;
			imgBilde.Image = _logg.ImagePath.Length > 0 ? ok : notOk;
				
			var imgstr = Utils.GetPath("jaktlogg_"+_logg.ID+".jpg");
			
			if(!File.Exists(imgstr)){
				imgstr = "Images/Icons/pictureplaceholder.png";
				buttonImage.SetImage(new UIImage(imgstr), UIControlState.Normal);
			}
			else
				buttonImage.SetImage(new UIImage(Utils.GetPath(imgstr)), UIControlState.Normal);
			buttonImage.Layer.MasksToBounds = true;
			buttonImage.Layer.CornerRadius = 5.0f;
			buttonImage.TouchUpInside += HandleButtonImageTouchUpInside;
			
			base.ViewDidLoad ();
		}
	}
}

