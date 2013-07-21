
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
		public Dog SelectedDog;
		public string Tittel = Utils.Translate("stats.kills");
		public string Mode = "Felt";
		public int FilterMonths = 0;
		public StatsArter () : base("StatsArter", null)
		{
			
		}
		private StatsArterTableSource _tableSource;
		public HeaderStatsFilter headerStatsFilter;
		
		public override void ViewDidLoad ()
		{
			Title = Tittel;
			headerStatsFilter = new HeaderStatsFilter();
			headerStatsFilter.HandleSegmentedControlValueChange = HandleSegmentedControlValueChange;
			headerStatsFilter.HandleButtonFilterClicked = HandleButtonFilterClicked;
			
			_tableSource = new StatsArterTableSource(this);
			TableView.Source = _tableSource;
			
			base.ViewDidLoad ();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			_tableSource.ItemList = GetRanking();
			TableView.ReloadData();
			
			base.ViewDidAppear (animated);
		}
		
		public void HandleButtonFilterClicked (object sender, EventArgs EventArgs)
		{
			var pickerScreen = new FooterStatsArter(screen => 
            {
				SelectedJeger = screen.SelectedJeger;
				SelectedDog = screen.SelectedDog;
				_tableSource.ItemList = GetRanking();
				TableView.ReloadData();
				
				var labelText = Utils.Translate("showingcountfor");
				if(SelectedJeger == null && SelectedDog == null)
					labelText += Utils.Translate("all_hunters_dogs");
				
				if(SelectedJeger != null)
					labelText += SelectedJeger.Fornavn;
				
				if(SelectedDog != null && SelectedJeger != null)
					labelText += "/";
				
				if(SelectedDog != null)
					labelText += SelectedDog.Navn;
				
				headerStatsFilter.SetFilterLabel(labelText);
				
			},SelectedJeger, SelectedDog);
			
			pickerScreen.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;
			PresentViewController(pickerScreen, true, null);
		}
		
		public void HandleSegmentedControlValueChange (object sender, EventArgs e)
		{
			switch((sender as UISegmentedControl).SelectedSegment){
			case 1:
				FilterMonths = -1;
				break;
			case 2:
				FilterMonths = -6;
				break;
			case 3:
				FilterMonths = -12;
				break;
			default:
				FilterMonths = 0;
				break;
			}

			_tableSource.ItemList = GetRanking();
			TableView.ReloadData();
				
		}
		
		private List<ArtRanking> GetRanking()
		{
			//bool onlyThisYear = FilterOnlyThisYear;
			var logger = JaktLoggApp.instance.LoggList.Where(x => x.ArtId > 0 && x.Treff > 0);
			if(Mode == "Sett")
				logger = JaktLoggApp.instance.LoggList.Where(x => x.ArtId > 0 && x.Sett > 0);

			if(FilterMonths != 0)
			{
				DateTime fromDate = DateTime.Now.AddMonths(FilterMonths);
				Console.WriteLine("date from: " + DateTime.Now.Month + ", "+ fromDate.ToShortDateString());
				logger = logger.Where(x => x.Dato > fromDate);

			}
			/*if(onlyThisYear){
				DateTime fromDate = new DateTime(DateTime.Now.Month > Utils.HUNTYEAR_STARTMONTH ? DateTime.Now.Year : DateTime.Now.Year - 1, Utils.HUNTYEAR_STARTMONTH, 1);
				DateTime toDate = new DateTime(DateTime.Now.Month > Utils.HUNTYEAR_STARTMONTH ? DateTime.Now.Year + 1 : DateTime.Now.Year, Utils.HUNTYEAR_STARTMONTH, 1);
				
				Console.WriteLine("date: " + DateTime.Now.Month + ", "+ fromDate.ToShortDateString() + " - " + toDate.ToShortDateString());
				logger = logger.Where(x => x.Dato > fromDate && x.Dato < toDate);
            }
			*/
			if(SelectedJeger != null)
			{
				logger = logger.Where(x => x.JegerId == SelectedJeger.ID);
			}
			if(SelectedDog != null)
			{
				logger = logger.Where(x => x.DogId == SelectedDog.ID);
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
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			return _controller.headerStatsFilter.View;
		}
		
		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			return 90.0f;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var currentItem = ItemList.ElementAt(indexPath.Row);
			var cell = tableView.DequeueReusableCell("StatsArterCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Value1, "StatsArterCell");

			var j = JaktLoggApp.instance.GetArt(currentItem.ArtId);
			cell.TextLabel.Text = j != null ? j.Navn : "?";
			cell.DetailTextLabel.Text = currentItem.Count.ToString();
			return cell;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, false);
		
		}
	}
	public class ArtRanking
	{
		public int ArtId;
		public int Count;
	}
}

