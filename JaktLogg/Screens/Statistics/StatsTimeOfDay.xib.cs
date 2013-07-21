
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class StatsTimeOfDay: UIJaktViewController
	{
		private string GoogleGraphURI;
		public StatsTimeOfDay () : base("StatsTimeOfDay", null)
		{
			
		}
		
		public override void ViewDidLoad ()
		{
			Title = Utils.Translate("stats.timeofday");
			GoogleGraphURI = "http://chart.apis.google.com/chart" +
						   "?chxr=0,0,24|1,0,{0}" +
							"&chxl=0:|0||2||4||6|8||10||12||14|16||18||20||22|24" +
   							"&chxp=0,0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23" + 
						   "&chs="+imageView.Frame.Width+"x"+imageView.Frame.Height +
						   "&chxs=0,676767,8.333,0,l,676767" +
						   "&chxtc=0,1" +
						   "&chxt=x" +
						   "&chbh=a,0,1" +
						   "&cht=bvg" +
						   "&chco=c7d7a6" +
						   "&chds=0,{0}" +
						   "&chd=t:{1}";
		
			GoogleGraphURI = GoogleGraphURI.Replace("|", "%7C");
			
			pickerView.Model = new StatsTimeOfDayPickerViewModel(this);
			
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

			var valueList = "";
			var max = 0;
			for(var i=0; i<24; i++){
				var count = list.Where(v => v.Dato.Hour == i).Count();
				valueList += count.ToString() + ",";
				max = count > max ? count : max;
			}
			if(valueList.Length > 0)
				valueList = valueList.Substring(0, valueList.Length-1);
			
				var str = string.Format(GoogleGraphURI, max, valueList);
			
			if(Reachability.IsHostReachable("www.google.com")){
				imageView.Image = UIImage.LoadFromData(NSData.FromUrl(NSUrl.FromString(str)));
				imageView.Hidden = false;
			}
			else
			{
				imageView.Hidden = true;	
			}
		}
	}
	
	
	// ========== PickerViewModel =========== 
	
	public class StatsTimeOfDayPickerViewModel : UIPickerViewModel
	{
		private StatsTimeOfDay _controller;
		public StatsTimeOfDayPickerViewModel(StatsTimeOfDay controller) : base()
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
				return Utils.Translate("all_hunters");	
			}
			else
			{
				var jeger = JaktLoggApp.instance.JegerList[row-1];
				return jeger.Navn;
			}
		}
	}
}

