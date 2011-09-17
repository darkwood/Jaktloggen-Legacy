
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class StatsHitrate: UIJaktViewController
	{
		private string GoogleGraphURI;
		public StatsHitrate () : base("StatsHitrate", null)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			//label1.Hidden = label2.Hidden = true;
			label1.Text = label2.Text = "";
			Title = "Treffprosent";
			GoogleGraphURI = "http://chart.apis.google.com/chart?chco=c7d7a6|374716&chs=" + 
			imageView.Frame.Width+"x"+imageView.Frame.Height+"&cht=p3&chd=t:{0},{1}&chdl={0}%20treff|{1}%20bom";
			
			GoogleGraphURI = GoogleGraphURI.Replace("|", "%7C");
			
			pickerView.Model = new StatsHitratePickerViewModel(this);
			
			base.ViewDidLoad ();
		}
	
		public override void ViewDidAppear (bool animated)
		{
			JegerSelected(null);
			base.ViewDidAppear (animated);
		}
		
		public void JegerSelected (Jeger jeger)
		{
			var list = JaktLoggApp.instance.LoggList.Where(j => jeger == null || j.JegerId == jeger.ID);
			var hits = list.Sum(l=> l.Treff);
			var shots = list.Sum(l=> l.Skudd);
			var misses = shots - hits;
			var percent = shots > 0 ? Decimal.Round(hits*100/shots) : 0;

			GoogleGraphURI += jeger != null ? "&chtt="+HttpUtility.UrlEncode(jeger.Navn)+"" : "&chtt=Alle+jegere";
			GoogleGraphURI += HttpUtility.UrlEncode(string.Format(": {0} skudd, {1}% treff", shots, percent));
			var str = string.Format(GoogleGraphURI, hits, misses);
			
			if(Reachability.IsHostReachable("www.google.no")){
				imageView.Image = UIImage.LoadFromData(NSData.FromUrl(NSUrl.FromString(str)));
				//label1.Hidden = label2.Hidden = true;
				imageView.Hidden = false;
			}
			else
			{
				label1.Hidden = label2.Hidden = false;
				imageView.Hidden = true;
				label1.Text = string.Format("{0} treff. {1} bom.", hits, misses);
				label2.Text = string.Format("{0}% treff", percent);
			}
		}
	}
	
	
	// ========== PickerViewModel =========== 
	
	public class StatsHitratePickerViewModel : UIPickerViewModel
	{
		private StatsHitrate _controller;
		public StatsHitratePickerViewModel(StatsHitrate controller) : base()
		{
			_controller = controller;
		}
		
		public override void Selected (UIPickerView picker, int row, int component)
		{
			if(row == 0)
			{
				_controller.JegerSelected(null);
			}
			else
			{
				var jeger = JaktLoggApp.instance.JegerList[row-1];
				_controller.JegerSelected(jeger);
			}
		}
		public override int GetComponentCount (UIPickerView picker)
		{
			return 1;
		}
		
		public override int GetRowsInComponent (UIPickerView picker, int component)
		{
			return JaktLoggApp.instance.JegerList.Count() + 1;
		}
		
		public override string GetTitle (UIPickerView picker, int row, int component)
		{
			if(row == 0)
			{
				return "Alle jegere";	
			}
			else
			{
				var jeger = JaktLoggApp.instance.JegerList[row-1];
				return jeger.Navn;
			}
		}
	}
}

