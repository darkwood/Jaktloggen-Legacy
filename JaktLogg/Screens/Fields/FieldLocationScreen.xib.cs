
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;

namespace JaktLogg
{
	public partial class FieldLocationScreen: UIJaktViewController
	{
		public string Longitude;
		public string Latitude;
		public Logg CurrentLogg;
		public CLLocationManager locationManager;
		public LocationManagerDelegate locationManagerDelegate;
		private Action<FieldLocationScreen> _callback;

		public FieldLocationScreen (Logg _logg, Action<FieldLocationScreen> callback) : base("FieldLocationScreen", null)
		{
			Title = Utils.Translate("position");
			_callback = callback;
			CurrentLogg = _logg;
			Latitude = CurrentLogg.Latitude;
			Longitude = CurrentLogg.Longitude;
		}

		public override void ViewDidLoad ()
		{
			SetInfo("");	
			
			locationManager = new CLLocationManager();
			locationManager.DesiredAccuracy = CLLocation.AccuracyNearestTenMeters;
			locationManagerDelegate = new LocationManagerDelegate(mapView, this);
			locationManager.Delegate = locationManagerDelegate;
			
			//mapView.ShowsUserLocation = true;
			mapView.Delegate = new MapViewDelegate(this);
			
			var leftBtn = new UIBarButtonItem(Utils.Translate("cancel"), UIBarButtonItemStyle.Plain, CancelClicked);
			NavigationItem.LeftBarButtonItem = leftBtn;
			
			var rightBtn = new UIBarButtonItem(Utils.Translate("done"), UIBarButtonItemStyle.Done, DoneClicked);
			NavigationItem.RightBarButtonItem = rightBtn;
			
			btnGps.Clicked += GpsClicked;
			btnClear.Clicked += ClearClicked;
			mapTypeControl.ValueChanged += HandleMapTypeControlValueChanged;
			
			base.ViewDidLoad ();
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
				
		public override void ViewDidAppear (bool animated)
		{	
			double lat, lng;
			if(string.IsNullOrEmpty(Longitude)){
				if (CLLocationManager.LocationServicesEnabled){
					locationManager.StartUpdatingLocation();
					SetInfo(Utils.Translate("localizingposition"));	
				}
				else
					MessageBox.Show(Utils.Translate("error_nogps"), Utils.Translate("error_nogps_details"));
			}
			else if(double.TryParse(Latitude, out lat) && double.TryParse(Longitude, out lng))
			{
				CLLocation loc = new CLLocation(lat, lng);
				locationManagerDelegate.SetLocation(loc, Title, Utils.Translate("pin_dragtochange"));
			}
			base.ViewDidAppear (animated);
		}
		
		public void SetInfo(string info){
			txtLocation.Text = info;
		}
		
		private void ClearClicked(object sender, EventArgs args)
		{
			var actionSheet = new UIActionSheet("") {Utils.Translate("removeposition"), Utils.Translate("cancel")};
			actionSheet.Title = Utils.Translate("confirm_removeposition");
			actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 1;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object s, UIButtonEventArgs e) {
				Console.WriteLine(e.ButtonIndex);
				switch (e.ButtonIndex)
				{
				case 0:
					//Slett
					Latitude = Longitude = string.Empty;	
					locationManagerDelegate.ClearLocation();
					SetInfo(Utils.Translate("position_removed"));
					break;
				case 1:
					//Avbryt
					break;
				}
			};
			
		}
		private void GpsClicked(object sender, EventArgs args)
		{
			if (CLLocationManager.LocationServicesEnabled){
				locationManager.StartUpdatingLocation();
				SetInfo(Utils.Translate("localizingposition"));		
			}
			else
				MessageBox.Show(Utils.Translate("error_nogps"), Utils.Translate("error_nogps_details"));

		}
		private void CancelClicked(object sender, EventArgs args)
		{
			NavigationController.PopViewControllerAnimated(true);
		}
		private void DoneClicked(object sender, EventArgs args)
		{
			SaveAndClose();
		}
		public void SaveAndClose()
		{
			_callback(this);
			NavigationController.PopViewControllerAnimated(true);
		}
	}
	
	
	
	
	
	public class MapViewDelegate : MKMapViewDelegate {
		
		private FieldLocationScreen _controller;
		public MapViewDelegate (FieldLocationScreen controller):base()
	   {
			_controller = controller;
	   }
		
		public override void ChangedDragState (MKMapView mapView, MKAnnotationView annotationView, MKAnnotationViewDragState newState, MKAnnotationViewDragState oldState)
		{
			if(newState == MKAnnotationViewDragState.Ending){
				_controller.Latitude = _controller.locationManagerDelegate.CurrentAnnotation.Coordinate.Latitude.ToString();
				_controller.Longitude = _controller.locationManagerDelegate.CurrentAnnotation.Coordinate.Longitude.ToString();
			}
		}
		
		public override void RegionChanged (MKMapView mapView, bool animated)
		{
			//_controller.locationManager.StopUpdatingLocation();
		}
		
	   public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, NSObject annotation)
	   {
			MKPinAnnotationView anv = new MKPinAnnotationView(annotation, "thisLocation");
			
			anv.CanShowCallout = true;
			anv.AnimatesDrop = false;
			anv.Draggable = true;
			
			if(_controller.CurrentLogg.Treff > 0)
				anv.PinColor = MKPinAnnotationColor.Green;
			else if(_controller.CurrentLogg.Skudd > 0)
				anv.PinColor = MKPinAnnotationColor.Red;
			else
				anv.PinColor = MKPinAnnotationColor.Purple;
						
	      return anv;
	   }
	}
	
	
    public class MyAnnotation : MKAnnotation
    {
		private CLLocationCoordinate2D _coordinate;
        private string _title, _subtitle;
        
    	#region implemented abstract members of MonoTouch.MapKit.MKAnnotation
    	
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
		#endregion
       
 		[Export("_original_setCoordinate:")]
		public void setCoordinate(CLLocationCoordinate2D newCoordinate)
		{
			_coordinate = newCoordinate;
			
		}
        public MyAnnotation (CLLocationCoordinate2D coord, string t, string s)
        {
            _coordinate=coord;
             _title=t; 
            _subtitle=s;
			
        }
	}
	
	public class LocationManagerDelegate : CLLocationManagerDelegate
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
	        MKCoordinateSpan span = new MKCoordinateSpan(0.010, 0.010);
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
		
	    public override void UpdatedLocation(CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
	    {
	    
			_controller.SetInfo(string.Format(Utils.Translate("posiont_found_radius"), newLocation.HorizontalAccuracy, newLocation.VerticalAccuracy));
			_controller.Latitude = newLocation.Coordinate.Latitude.ToString();
			_controller.Longitude = newLocation.Coordinate.Longitude.ToString();
		
			SetLocation(newLocation, Utils.Translate("yourposition"), Utils.Translate("pin_dragtochange"));

			//Stop updating location if this is close enough...
			if(newLocation.HorizontalAccuracy <= CLLocation.AccuracyHundredMeters &&
			   newLocation.VerticalAccuracy <= CLLocation.AccuracyHundredMeters)
			{
				manager.StopUpdatingLocation();
	   		}
		}		
	}
}

