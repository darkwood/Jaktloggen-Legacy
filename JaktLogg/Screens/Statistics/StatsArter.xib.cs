
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class StatsArter : UIJaktTableViewController
	{
		public Jeger SelectedJeger;
		public string Tittel = "Antall felt vilt";
		public string Mode = "Felt";
		public StatsArter () : base("StatsArter", null)
		{
			
		}
		private UIBarButtonItem _rightButton;
		private StatsArterTableSource _tableSource;
		public override void ViewDidLoad ()
		{
			Title = Tittel;
			
			_rightButton = new UIBarButtonItem("Alle jegere", UIBarButtonItemStyle.Done, HandleRightButtonClicked);			
			NavigationItem.RightBarButtonItem = _rightButton;
			_tableSource = new StatsArterTableSource(this);
			TableView.Source = _tableSource;
			
			segmentedControl.ValueChanged += HandleSegmentedControlValueChange;
			
			base.ViewDidLoad ();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			_tableSource.ItemList = GetRanking();
			TableView.ReloadData();
			
			base.ViewDidAppear (animated);
		}
		
		void HandleRightButtonClicked (object sender, EventArgs EventArgs)
		{
			var pickerScreen = new FooterStatsArter(screen => 
            {
				SelectedJeger = screen.SelectedJeger;
				_tableSource.ItemList = GetRanking();
				TableView.ReloadData();
				
				
				_rightButton.Title = SelectedJeger == null ? "Alle jegere" : SelectedJeger.Fornavn;
			},SelectedJeger);
			
			pickerScreen.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;
			this.PresentModalViewController(pickerScreen, true);
		}
		
		void HandleSegmentedControlValueChange (object sender, EventArgs e)
		{
			_tableSource.ItemList = GetRanking();
			TableView.ReloadData();
				
		}
		
		private List<ArtRanking> GetRanking()
		{
			bool onlyThisYear = segmentedControl.SelectedSegment > 0;
			var logger = JaktLoggApp.instance.LoggList.Where(x => x.ArtId > 0 && x.Treff > 0);
			if(Mode == "Sett")
				logger = JaktLoggApp.instance.LoggList.Where(x => x.ArtId > 0 && x.Sett > 0);
			
			if(onlyThisYear){
				DateTime fromDate = new DateTime(DateTime.Now.Month > 7 ? DateTime.Now.Year : DateTime.Now.Year - 1, Utils.HUNTYEAR_STARTMONTH, 1);
				DateTime toDate = new DateTime(DateTime.Now.Month > 7 ? DateTime.Now.Year + 1 : DateTime.Now.Year, Utils.HUNTYEAR_STARTMONTH, 1);
				
				Console.WriteLine("date: " + DateTime.Now.Month + ", "+ fromDate.ToShortDateString() + " - " + toDate.ToShortDateString());
				logger = logger.Where(x => x.Dato > fromDate && x.Dato < toDate);
            }
			
			if(SelectedJeger != null)
			{
				logger = logger.Where(x => x.JegerId == SelectedJeger.ID);
			}
			
			logger = logger.ToList<Logg>();
			
			var arterDistinct = (from l in logger
			               select l.ArtId).Distinct();
			
			var ranking = new List<ArtRanking>();
			
			if(Mode == "Sett")
			{
				foreach(var artid in arterDistinct){
				var count = (from l in logger
							where l.ArtId == artid
							select l.Sett).Sum();
				ranking.Add(new ArtRanking(){ArtId = artid, Count = count});
				}
			}
			else
			{
				foreach(var artid in arterDistinct){
					var count = (from l in logger
								where l.ArtId == artid
								select l.Treff).Sum();
					ranking.Add(new ArtRanking(){ArtId = artid, Count = count});
				}
			}
			return ranking.OrderByDescending(x => x.Count).ToList<ArtRanking>();
		}

	}
	
	public class StatsArterTableSource : UITableViewSource
	{
		private StatsArter _controller;
		public List<ArtRanking> ItemList = new List<ArtRanking>();
		
		public StatsArterTableSource(StatsArter controller)
		{
			_controller = controller;
			
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return ItemList.Count;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var currentItem = ItemList.ElementAt(indexPath.Row);
			var cell = tableView.DequeueReusableCell("StatsArterCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Value1, "StatsArterCell");
			
			cell.TextLabel.Text = JaktLoggApp.instance.GetArt(currentItem.ArtId).Navn;
			cell.DetailTextLabel.Text = currentItem.Count.ToString();
			//cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			return cell;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, false);
			/*var item = ItemList.ElementAt(indexPath.Row);
			var art = JaktLoggApp.instance.GetArt(item.ArtId);
			var artScreen = new ArtWebViewController(art);
			_controller.NavigationController.PushViewController(artScreen, true);
		*/
		}
	}
	public class ArtRanking
	{
		public int ArtId;
		public int Count;
	}
}

