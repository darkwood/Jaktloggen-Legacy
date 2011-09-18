
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;

namespace JaktLogg
{
	public partial class StatsLoggMap: UIJaktViewController
	{
		public Dictionary<int, MKAnnotation> Pins;
		private List<Logg> _loggItems;
		public string Filter = "";
		
		public StatsLoggMap (List<Logg> loggItems) : base("StatsLoggMap", null)
		{
			_loggItems = loggItems;
		}
		
		public override void ViewDidLoad ()
		{
			Title = "Loggføringer i kart";
			
			mapView.Delegate = new MapViewDelegate(this);
			mapView.ShowsUserLocation = true;
			RefreshMap();
			
			btRefresh.Clicked += delegate(object sender, EventArgs e) {
				RefreshMap();
			};
			btPageCurl.Clicked += HandleBtPageCurlClicked;
			mapTypeControl.ValueChanged += HandleMapTypeControlValueChanged;
			base.ViewDidLoad ();
		}

		void HandleBtPageCurlClicked (object sender, EventArgs e)
		{
			var actionSheet = new UIActionSheet("") {"Treff", "Bom", "Observasjoner", "Alle loggføringer"};
			actionSheet.Title = "Vis bare:";
			actionSheet.CancelButtonIndex = 3;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object s, UIButtonEventArgs evt) {

				switch (evt.ButtonIndex)
				{
				case 0:
					Filter = "Treff";
					break;
				case 1:
					Filter = "Bom";
				break;
				case 2:
					Filter = "Obs";
				break;
				case 3:
				default:
					Filter = "";
				break;
				}
				
				RefreshMap();
			};
		}
	

		void HandleMapTypeControlValueChanged (object sender, EventArgs e)
		{
			switch (mapTypeControl.SelectedSegment)
		    {
		        case 1: 
		            mapView.MapType = MKMapType.Satellite;
					break;
				case 2:
		            mapView.MapType = MKMapType.Hybrid;
					break;
		        default:
		            mapView.MapType = MKMapType.Standard;
					break;
		    }
		}
		
		
		private void RefreshMap(){
			AddPinsToMap();
			ZoomToShowAllPins();
		}
		
		
		protected void AddPinsToMap()
		{	
			if(Pins != null){
				while(Pins.Count > 0){
					RemoveLocation(Pins.First().Value, Pins.First().Key);
				}
			}
			Pins = new Dictionary<int, MKAnnotation>();
			foreach(var logg in _loggItems.Where(l => l.Latitude != "" && l.Longitude != ""))
			{
				AddLocation(logg);
			}
		}
		
		private string GetPinDescription(Logg l){
			var desc =  "";
			if(l.Skudd > 0)
				desc += l.Skudd + " skudd. " + l.Treff + " treff. "; 
			
			if(l.Sett > 0)
				desc += l.Sett + " sett. ";
			
			if(l.ArtId > 0)
				desc += JaktLoggApp.instance.GetArt(l.ArtId).Navn + ". ";
			
			if(l.JegerId > 0)
				desc += "Av " + JaktLoggApp.instance.GetJeger(l.JegerId).Fornavn;
			
			return desc;
		}
		
		protected void AddLocation(Logg logg)
		{
			if(Filter != "")
			{
				if(Filter == "Treff" && logg.Treff == 0)
					return;
				if(Filter == "Bom" && (logg.Skudd == 0 || logg.Treff > 0))
					return;
				if(Filter == "Obs" && (logg.Sett == 0 || logg.Skudd > 0 || logg.Treff > 0))
					return;
			}
			var id = logg.ID;
			var lat = double.Parse(logg.Latitude);
			var lon = double.Parse(logg.Longitude);
			var title = logg.Dato.ToNorwegianDateString() + " kl." +logg.Dato.ToNorwegianTimeString();
			var description = GetPinDescription(logg);
			
			var a = new MyAnnotation(new CLLocationCoordinate2D(lat,lon), title, description);
			a.CurrentLogg = logg;
			Pins.Add(id, a);
			mapView.AddAnnotation(a);
		}
		
		protected void RemoveLocation(MKAnnotation a, int i)
		{
			Pins.Remove(i);
			mapView.RemoveAnnotation(a);
		}
		
		protected void ZoomToShowAllPins(){
			
			if(mapView.Annotations.Count() > 0)
			{
				var southWest = (mapView.Annotations.First() as MKAnnotation).Coordinate;
				var northEast = (mapView.Annotations.First() as MKAnnotation).Coordinate;
				
				//Calclulate bounds by looking at all annontations:
				foreach(var obj in mapView.Annotations){
					if(obj is MKAnnotation)
					{
						MKAnnotation a = obj as MKAnnotation;

						southWest.Latitude = Math.Min(southWest.Latitude, a.Coordinate.Latitude);
						southWest.Longitude = Math.Min(southWest.Longitude, a.Coordinate.Longitude);
						
						northEast.Latitude = Math.Max(northEast.Latitude, a.Coordinate.Latitude);
						northEast.Longitude = Math.Max(northEast.Longitude, a.Coordinate.Longitude);
					}
				}
				
				var locSouthWest = new CLLocation(southWest.Latitude, southWest.Longitude);
				var locNorthEast = new CLLocation(northEast.Latitude, northEast.Longitude);
				
				var spanLatitudeDelta = Math.Abs(locNorthEast.Coordinate.Latitude - locSouthWest.Coordinate.Latitude) * 2;
				var spanLongitudeDelta = Math.Abs(locNorthEast.Coordinate.Longitude - locSouthWest.Coordinate.Longitude) * 2;
	           	
				var regionCenterLatitude = (southWest.Latitude + northEast.Latitude) / 2;
				var regionCenterLongitude = (southWest.Longitude + northEast.Longitude) / 2;
				
				var span = new MKCoordinateSpan(spanLatitudeDelta, spanLongitudeDelta);
				var regionCenter = new CLLocationCoordinate2D(regionCenterLatitude, regionCenterLongitude);
				
				var region = new MKCoordinateRegion(regionCenter, span);
				
	            mapView.SetRegion(region, true);
			}
		}
		
		
		class MapViewDelegate : MKMapViewDelegate {
		
			private StatsLoggMap _controller;
			public MapViewDelegate (StatsLoggMap controller):base()
		   {
				_controller = controller;
		   }
			
		  public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, NSObject annotation)
		   {
				if(annotation is MonoTouch.MapKit.MKUserLocation)
				  	return null;
				var a = annotation as MyAnnotation;
				
				MKPinAnnotationView anv = new MKPinAnnotationView(annotation, "thisLocation");
				anv.CanShowCallout = true;
				anv.AnimatesDrop = true;
				
				if(a.CurrentLogg.Treff > 0)
					anv.PinColor = MKPinAnnotationColor.Green;
				else if(a.CurrentLogg.Skudd > 0)
					anv.PinColor = MKPinAnnotationColor.Red;
				else
					anv.PinColor = MKPinAnnotationColor.Purple;
					
				return anv;
		   }
		}
		
	    class MyAnnotation : MKAnnotation
	    {
			public Logg CurrentLogg;
			private CLLocationCoordinate2D _coordinate;
	        private string _title, _subtitle;
	    	public override CLLocationCoordinate2D Coordinate {
	    		get {
					return _coordinate;
	    		}
	    		set {
					_coordinate = value;
	      		}
	    	}
			public override string Title {
				get {
					return _title;
				}
			}
	    	public override string Subtitle {
				get {
					return _subtitle;
				}
			}
			
	        public MyAnnotation (CLLocationCoordinate2D coord, string t, string s)
	        {
	            _coordinate=coord;
	             _title=t; 
	            _subtitle=s;
				
	        }
		}
		
	}
	
	
	
	/*public class LocationManagerDelegate : CLLocationManagerDelegate
	{
		public MyAnnotation CurrentAnnotation;
	    private MKMapView _mapview;
	    private FieldLocationScreen _controller;
	    public LocationManagerDelegate(MKMapView mapview, FieldLocationScreen controller)
	    {
	        _mapview = mapview; 
			_controller = controller;
	    }
		
		public void ClearLocation(){
			_mapview.RemoveAnnotation(CurrentAnnotation);					
		}
		
		public void SetLocation(CLLocation newLocation, string title, string description)
		{
			if(_mapview.Annotations.Contains(CurrentAnnotation))
			{
				_mapview.RemoveAnnotation(CurrentAnnotation);	
			}
	        MKCoordinateSpan span = new MKCoordinateSpan(0.2, 0.2);
	        MKCoordinateRegion region = new MKCoordinateRegion(newLocation.Coordinate, span);
	        
	        _mapview.SetRegion(region, true);
			
			if(CurrentAnnotation == null)
				CurrentAnnotation = new MyAnnotation(new CLLocationCoordinate2D(newLocation.Coordinate.Latitude, 
				                                                            newLocation.Coordinate.Longitude), 
											                                 title, 
											                                 description);
			else
			{
				CurrentAnnotation.Coordinate = newLocation.Coordinate;
			}
			_mapview.AddAnnotation(CurrentAnnotation);
			_mapview.SelectAnnotation(CurrentAnnotation, false);
		}
	}*/
}

