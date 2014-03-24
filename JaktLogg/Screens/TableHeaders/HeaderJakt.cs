using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public partial class HeaderJakt : UIJaktViewController
	{
		private Jakt _jakt;
		public EventHandler HandleButtonImageTouchUpInside;
		public EventHandler HandleButtonStedTouchUpInside;
		
		public HeaderJakt (Jakt jakt) : base ("HeaderJakt", null)
		{
			_jakt = jakt;
		}
		public override void ViewDidAppear (bool animated)
		{
			labelSted.Text = _jakt.Sted == "" ? Utils.Translate("jakt.setlocationname") : _jakt.Sted;
			labelDato.Text = _jakt.DatoFra.ToLocalDateString() == _jakt.DatoTil.ToLocalDateString() ? 
				_jakt.DatoFra.ToLocalDateAndYearString() : 
					_jakt.DatoFra.ToLocalDateString() + " - " + _jakt.DatoTil.ToLocalDateAndYearString();
			
			
			var imgstr = Utils.GetPath("jakt_"+_jakt.ID+".jpg");
			
			if(!File.Exists(imgstr)){
				imgstr = "Images/Icons/pictureplaceholder.png";
				buttonImage.SetImage(new UIImage(imgstr), UIControlState.Normal);
			}
			else
				buttonImage.SetImage(new UIImage(Utils.GetPath(imgstr)), UIControlState.Normal);

			base.ViewDidAppear (animated);
		}
		public override void ViewDidLoad ()
		{

			buttonImage.Layer.MasksToBounds = true;
			buttonImage.Layer.CornerRadius = 5.0f;
			buttonImage.TouchUpInside += HandleButtonImageTouchUpInside;
			buttonSted.TouchUpInside += HandleButtonStedTouchUpInside;
			buttonStedLabel.TouchUpInside += HandleButtonStedTouchUpInside;
			base.ViewDidLoad ();
		}
		
		
	}
}
