using System;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Net;
using System.Threading;
using System.IO;
//using System.Json;
using MonoTouch.Foundation;

namespace JaktLogg
{
	public class Location
	{	
		public static CLLocationCoordinate2D CurrentLocation { get; set; }
		
		// Implementation of the Haversine Forumla (http://en.wikipedia.org/wiki/Haversine_formula)
        public double CalcDistance(double fromLat, double fromLon, double toLat, double toLon)
        {
            double R = 6378.1; // radius of earth.

            double dLat = ToRadian(toLat - fromLat);
            double dLon = ToRadian(toLon - fromLon);

            double a =
                Math.Sin(dLat/2.0)*Math.Sin(dLat/2.0) +
                Math.Cos(ToRadian(fromLat))*Math.Cos(ToRadian(toLat))*
                Math.Sin(dLon/2.0)*Math.Sin(dLon/2.0);

            double c = 2.0*Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R*c;
            return d;
        }

        private static double ToRadian(double value)
        {
            return (Math.PI/180.0)*value;
        }
		
		static public void FindMe(MKMapView map, string markerText)
		{
			map.Delegate = new MapViewDelegate();
			
			map.SetCenterCoordinate(CurrentLocation, true);
			map.Region = new MKCoordinateRegion(CurrentLocation, new MKCoordinateSpan(0.005, 0.005));
			
			map.SetCenterCoordinate(CurrentLocation, true);
			
			var meAnn = new SpotAnnotation(CurrentLocation, markerText, "");
			map.AddAnnotationObject(meAnn);
		}
		
		
		private static ManualResetEvent allDone = new ManualResetEvent(false);
		private HttpWebRequest request;
		private WebResponse response;
		
		/// <summary>
		/// Performs a reverse Geolocation lookup.
		/// </summary>
		/// <param name="loc">
		/// A <see cref="Location"/>
		/// </param>
		/// <returns>
		/// A JSON object Documentation at link below
		/// http://code.google.com/intl/no-NO/apis/maps/documentation/geocoding/#ReverseGeocoding
		/// Returns an empty string if lookup was unsuccessful
		/// </returns>
	/*	public JsonValue GetAddressFromLocation (CLLocationCoordinate2D loc)
		{
			//http://maps.googleapis.com/maps/api/geocode/json?latlng=40.714224,-73.961452&sensor=true_or_false
			allDone.Reset();
			// prepare the web page we will be asking for
			request  = (HttpWebRequest)WebRequest.Create("http://maps.googleapis.com/maps/api/geocode/json?latlng=" +
			                                             loc.Latitude.ToString().Replace(",",".") + "," + loc.Longitude.ToString().Replace(",",".") + "&sensor=true");
			
			request.BeginGetResponse(new AsyncCallback(FinishWebRequest), null);
			
			allDone.WaitOne();
			
			string json = "";
			using (var sr = new StreamReader(response.GetResponseStream()))
			{
				json = sr.ReadToEnd();
			}
			
			var reader = new JsonReader(new StringReader(json));
			
			return reader.Read();
		}
	*/	
		void FinishWebRequest(IAsyncResult result)
		{
			response = request.EndGetResponse(result);
			allDone.Set();
		}
		
		
		public class SpotAnnotation : MKAnnotation
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
			public void setCoordinate(CLLocationCoordinate2D newCoordinate){
				
				_coordinate = newCoordinate;
				
			}
			
	        public SpotAnnotation (CLLocationCoordinate2D coord, string t, string s)
	        {
	            _coordinate=coord;
	            _title=t; 
	            _subtitle=s;
	        }
		}
		
		public class MapViewDelegate : MKMapViewDelegate
		{
		    public MapViewDelegate ():base()
		    {}
		    public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
		    {
				try
				{
					var anv = mapView.DequeueReusableAnnotation("thisLocation");
					if (anv == null)
					{
				    	MKPinAnnotationView pinanv = new MKPinAnnotationView(annotation, "thisLocation");
			
				      	pinanv.CanShowCallout = true;
					  	pinanv.Draggable =true;
					  	pinanv.AnimatesDrop = true;
					  	pinanv.PinColor = MKPinAnnotationColor.Green;
						anv = pinanv;
					}
					else
					{
						anv.Annotation = annotation;
					}
			      	return anv;
				}
				catch (Exception e)
				{
					Console.WriteLine("Failed to get view for annotation " + e);
					return null;
				}
		   	}
		
		}
	}
}

