
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
		public StatsLoggMap (List<Logg> loggItems) : base("StatsLoggMap", null)
		{
			_loggItems = loggItems;
		}
		
		public override void ViewDidLoad ()
		{
			Title = "Loggføringer i kart";
			
			mapView.Delegate = new MapViewDelegate(this);
			mapView.ShowsUserLocation = true;
			AddPinsToMap();
			ZoomToShowAllPins();
			
			base.ViewDidLoad ();
		}
		
		protected void AddPinsToMap()
		{	
			Pins = new Dictionary<int, MKAnnotation>();
			foreach(var logg in _loggItems.Where(l => l.Latitude != "" && l.Longitude != ""))
			{
				AddLocation(logg.ID, 
				            double.Parse(logg.Latitude), 
				            double.Parse(logg.Longitude), 
				            logg.Dato.ToNorwegianDateString() + " kl." +logg.Dato.ToNorwegianTimeString(), 
				            GetPinDescription(logg));
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
		
		protected void AddLocation(int id, double lat, double lon, string title, string description)
		{
			var a = new MyAnnotation(new CLLocationCoordinate2D(lat,lon), title, description);
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
				
				MKPinAnnotationView anv = new MKPinAnnotationView(annotation, "thisLocation");
				anv.CanShowCallout = true;
				anv.AnimatesDrop = false;
				anv.PinColor = MKPinAnnotationColor.Green;
				return anv;
		   }
		}
		
	    class MyAnnotation : MKAnnotation
	    {
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

