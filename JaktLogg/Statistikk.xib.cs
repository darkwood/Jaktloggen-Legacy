
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace JaktLogg
{
	public partial class Statistikk: UIJaktViewController
	{
		private const string graphWidth = "300";
		private const string graphHeight = "225";
		public List<Logg> list = new List<Logg>();
		
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code

		public Statistikk (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public Statistikk (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		public Statistikk () : base("Statistikk", null)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
		
		#endregion
		
		public override void ViewDidLoad ()
		{
			if(JaktLoggApp.instance.LoggList.Count() == 0)
			{
				LoadingView.Show();
				JaktLoggApp.instance.InitializeAllData(() => {
					InvokeOnMainThread(() => {
						LoadingView.Hide();
						CreateStatisticsViews();
					});
				});
			}
			scrollView.Delegate = new ScrollViewDelegate(this);
			
			base.ViewDidLoad ();
		}
		public override void ViewDidAppear (bool animated)
		{
			CreateStatisticsViews();
			
			base.ViewDidAppear (animated);
		}
		
		private void CreateStatisticsViews()
		{
			list = JaktLoggApp.instance.LoggList;
			
			int count = 6;
		    for(int i=0; i<count; i++)
		    {
				RectangleF frame = scrollView.Frame;
		        PointF location = new PointF();
		        location.X = frame.Width * i;
		        frame.Location = location;
				var v = CreateStatisticsScreen(i);
				v.View.Frame = frame;
				scrollView.AddSubview(v.View);
				
		    }
			
			RectangleF scrollFrame = scrollView.Frame;
		    pageControl.Pages = count;
			scrollFrame.Width = scrollFrame.Width * count;
		    scrollView.ContentSize = scrollFrame.Size;
			
			pageControl.Pages = count;
		}
		
		private UIViewController CreateStatisticsScreen(int i)
		{
			
			switch(i)
			{
				case 0:
				return new StatsGenericWebView("Statistikk", "<h1>Litt statistikk...</h1>", "Bla gjennom for å se din statistikk ->");

				case 1:
				var shots = list.Sum(l=> l.Skudd);
				var hits = list.Sum(l=> l.Treff);
				var percent = shots > 0 ? Decimal.Round(hits*100/shots) : 0;
					
				var f = string.Format("{0} treff av {1} skudd som gir {2}% treff", hits, shots, percent);
				var img = string.Format("http://chart.apis.google.com/chart?chs="+graphWidth+"x"+graphHeight+"&cht=p3&chd=t:{0},{1}&chdl=Skudd|Treff", shots, hits);
				return new StatsGenericWebView("Treffprosent", f, "<img src=\""+img+"\" />");
				
			case 2:
				return CreateSpeciesCount();
			case 3:
				return new StatsGenericWebView("Skarpskyttere", "", "Rangering av jegere her...");
			case 4:
				return new StatsArter();
			case 5:
				return new StatsGenericWebView("Annet?", "", "Mer statistikk?");
			default:
				break;	
			}	
			return new UIViewController();
		}
		
		private StatsGenericWebView CreateSpeciesCount()
		{
			var vc = new StatsGenericWebView();
			vc.Header = "Fangst pr. art";
			if(JaktLoggApp.instance.ArtList.Count > 0)
			{
				var artcount = "";
				var artnames = "";
				foreach(var item in JaktLoggApp.instance.ArtList){
					var itemslist = (from l in list
							 where l.ArtId == item.ID
							 select l).ToList();
					
					int count = itemslist.Sum(c => c.Treff);
					
					if(count > 0){
					artcount += count+",";
					artnames += item.Navn + "("+ count + ")|";
					}
				}
				artcount = artcount.Length >0 ? artcount.Substring(0, artcount.Length-1) : artcount;
				artnames = artnames.Length >0 ? artnames.Substring(0, artnames.Length-1) : artnames;
				
				var img = "http://chart.apis.google.com/chart?chs="+graphWidth+"x"+graphHeight+"&cht=p3&chd=t:{0}&chdl={1}";
				vc.Html = "<img src=\""+string.Format(img, artcount, artnames)+"\" />";
				Console.Write(vc.Html);
			}
			else
			{
				vc.Footer = "Ingen arter i databasen";
			}
			return vc;
		}
		
		private void SetCurrentInfo()
		{
			pageControl.CurrentPage = _currentPageIndex;
		}
		
		
		
		private SizeF pageSize(){ return this.scrollView.Frame.Size; }
		private int _currentPageIndex;
		
		class ScrollViewDelegate : UIScrollViewDelegate{
		
			private Statistikk _controller;
			
			public ScrollViewDelegate(Statistikk controller){
				_controller = controller;
			}
			
			public override void Scrolled (UIScrollView scrollView)
			{
	            SizeF pageSize = _controller.pageSize();
				
	            int newPageIndex = ((int)_controller.scrollView.ContentOffset.X + (int)pageSize.Width / 2) / (int)pageSize.Width;
				
	            if (newPageIndex == _controller._currentPageIndex)
	                return;
	
	            _controller._currentPageIndex = newPageIndex;
				_controller.SetCurrentInfo();
			}	
		}
		
		private void RunStats(){
			
			var stats = "========Statistikk test========";
			
			if(list.Count == 0){
				stats += "\n\n\nDet må nok legges inn jakt og logger før statistikk kan lages.";
				return;
			}
			var shots = list.Sum(l=> l.Skudd);
			var hits = list.Sum(l=> l.Treff);
				
			stats +="\nAntall logger: " + list.Count;
			stats +="\nSum skudd: " + shots;
			stats +="\nSum treff: " + hits;
			stats +="\nTreffprosent: " + hits*100/shots + "%";
			
			if(JaktLoggApp.instance.ArtList.Count > 0){
				stats +="\n-------ANTALL ARTER I LOGGEN -------";
				foreach(var item in JaktLoggApp.instance.ArtList){
					stats += "\n"+item.Navn +": " + list.Where(l => l.ArtId == item.ID).Count();
				}
			}
			
			if(JaktLoggApp.instance.JegerList.Count > 0){
				stats +="\n------ANTALL JEGERE I LOGGEN -------";
				foreach(var item in JaktLoggApp.instance.JegerList){
					stats += "\n"+item.Fornavn +": " + list.Where(l => l.JegerId == item.ID).Count();
				}
			}
			Console.WriteLine(stats);
		}
	}
}

