
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public partial class HeaderJaktItem : UIJaktViewController
	{
		private Jakt _jakt;
		public EventHandler HandleButtonImageTouchUpInside;
		public EventHandler HandleButtonStedTouchUpInside;

		public HeaderJaktItem (Jakt jakt) : base("HeaderJaktItem", null)
		{
			_jakt = jakt;
		}
		
		public override void ViewDidLoad ()
		{
			labelSted.Text = _jakt.Sted == "" ? "Legg inn stedsnavn" : _jakt.Sted;
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
			
			buttonImage.Layer.MasksToBounds = true;
			buttonImage.Layer.CornerRadius = 5.0f;
			buttonImage.TouchUpInside += HandleButtonImageTouchUpInside;
			
			
			buttonSted.TouchUpInside += delegate(object sender, EventArgs e) {
				Console.WriteLine("ok");
				//HandleButtonStedTouchUpInside(sender, e);
			};
			buttonStedLabel.TouchUpInside += HandleButtonStedTouchUpInside;
			
			
			base.ViewDidLoad ();
		}
	}
}

