
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
		private List<Logg> _loggItems;
		private bool _hasGeotaggedItems;
		public EventHandler HandleButtonImageTouchUpInside;
		//public EventHandler HandleButtonKartTouchUpInside;
		
		public HeaderJaktItem (Jakt jakt) : base("HeaderJaktItem", null)
		{
			_jakt = jakt;
			_loggItems = JaktLoggApp.instance.LoggList.Where(x => x.JaktId == _jakt.ID).ToList<Logg>();
			_hasGeotaggedItems =   _loggItems.Where(x => x.Latitude != "" && x.Longitude != "").ToList<Logg>().Count() > 0;
				
		}
		
		public override void ViewDidLoad ()
		{
			labelSted.Text = _jakt.Sted;
			labelDato.Text = _jakt.DatoFra.ToNorwegianDateString() == _jakt.DatoTil.ToNorwegianDateString() ? 
				_jakt.DatoFra.ToNorwegianDateAndYearString() : 
				_jakt.DatoFra.ToNorwegianDateString() + " - " + _jakt.DatoTil.ToNorwegianDateAndYearString();
			
			
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
			
			//buttonKart.TouchUpInside += HandleButtonKartTouchUpInside;
			//buttonKart.Hidden = !_hasGeotaggedItems;
			
			
			base.ViewDidLoad ();
		}
	}
}

