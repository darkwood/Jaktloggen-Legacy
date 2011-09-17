
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public partial class HeaderJeger : UIJaktViewController
	{
		public EventHandler HandleButtonImageTouchUpInside;
		private Jeger _jeger;
		public HeaderJeger (Jeger j) : base("HeaderJeger", null)
		{
			_jeger = j;
		}
		
		public override void ViewDidLoad ()
		{
			if(_jeger.ID > 0){
				labelName.Text = _jeger.Navn;
				var jegerloggs = JaktLoggApp.instance.LoggList.Where(l => l.JegerId == _jeger.ID).ToList<Logg>();
				var shots = jegerloggs.Sum(l=> l.Skudd);
				var hits = jegerloggs.Sum(l=> l.Treff);
				if(shots > 0){
					var percent = shots > 0 ? Decimal.Round(hits*100/shots) : 0;
					labelNotes.Text = "Treffsikkerhet: " + percent + "%";
				}
				else
				{
					labelNotes.Text = "";
				}
			}
			else{
				labelName.Text = "";
				labelNotes.Text = "";
			}

			var imgstr = Utils.GetPath("jeger_"+_jeger.ID+".jpg");
			
			if(!File.Exists(imgstr)){
				imgstr = "Images/Icons/camera.png";
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

