
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public partial class StatsGenericTableView : UIJaktTableViewController
	{
		public Jeger SelectedJeger;
		public string Tittel = "Antall felt vilt";
		public string Mode = "Felt";

		public StatsGenericTableView (string tittel) : base("StatsGenericTableView", null)
		{
			Tittel = tittel;
		}
		/*private UIBarButtonItem _rightButton;
		private StatsGenericTableSource _tableSource;
		public override void ViewDidLoad ()
		{
			Title = Tittel;
			
			_rightButton = new UIBarButtonItem("Alle jegere", UIBarButtonItemStyle.Done, HandleRightButtonClicked);			
			NavigationItem.RightBarButtonItem = _rightButton;
			_tableSource = new StatsGenericTableSource(this);
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
			var logger = JaktLoggApp.instance.LoggList;
			
			if(onlyThisYear){
				DateTime fromDate = new DateTime(DateTime.Now.Month > 7 ? DateTime.Now.Year : DateTime.Now.Year - 1, Utils.HUNTYEAR_STARTMONTH, 1);
				DateTime toDate = new DateTime(DateTime.Now.Month > 7 ? DateTime.Now.Year + 1 : DateTime.Now.Year, Utils.HUNTYEAR_STARTMONTH, 1);
				logger = logger.Where(x => x.Dato > fromDate && x.Dato < toDate);
            }
			
			if(SelectedJeger != null)
			{
				logger = logger.Where(x => x.JegerId == SelectedJeger.ID);
			}
			
			logger = logger.ToList<Logg>();
			
			var itemsDistinct = (from l in logger
			               select l.ArtId).Distinct();
			
			var ranking = new List<ItemRanking>();
			
			foreach(var artid in arterDistinct){
					var count = (from l in logger
								where l.Age == artid
								select l.Treff).Sum();
					ranking.Add(new ArtRanking(){ArtId = artid, Count = count});
				}
			
			return ranking.OrderByDescending(x => x.Count).ToList<ArtRanking>();
		}

	}
	
	public class StatsGenericTableSource : UITableViewSource
	{
		private StatsArter _controller;
		public List<ItemRanking> ItemList = new List<ItemRanking>();
		
		public StatsGenericTableSource(StatsArter controller)
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
		}*/
	}
}

