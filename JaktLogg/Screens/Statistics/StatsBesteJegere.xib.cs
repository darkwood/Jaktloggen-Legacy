
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public partial class StatsBesteJegere : UIJaktTableViewController
	{
		public StatsBesteJegere () : base("StatsBesteJegere", null)
		{
			
		}	
		private StatsBesteJegereTableSource _tableSource;
		public override void ViewDidLoad ()
		{
			Title = "Beste jegere";
			_tableSource = new StatsBesteJegereTableSource(this);
			TableView.Source = _tableSource;
			_tableSource.ItemList = GetRanking(false);
			TableView.ReloadData();
			
			segmentedControl.ValueChanged += HandleSegmentedControlValueChange;
			
			base.ViewDidLoad ();
		}
		
		
		void HandleSegmentedControlValueChange (object sender, EventArgs e)
		{
			switch(segmentedControl.SelectedSegment){
			case 0:
				_tableSource.ItemList = GetRanking(false);
				break;
			case 1:
				_tableSource.ItemList = GetRanking(true);
				break;
			default:
				break;
			}
			
			TableView.ReloadData();
				
		}
		
		private List<JegerRanking> GetRanking(bool onlyThisYear)
		{
			var logger = JaktLoggApp.instance.LoggList.Where(x => x.JegerId > 0 && x.Skudd > 0).ToList<Logg>();
			if(onlyThisYear){
				DateTime fromDate = new DateTime(DateTime.Now.Month > 9 ? DateTime.Now.Year : DateTime.Now.Year - 1, 9, 1);
				DateTime toDate = new DateTime(DateTime.Now.Month > 9 ? DateTime.Now.Year + 1 : DateTime.Now.Year, 9, 1);
				
				logger = logger.Where(x => x.Dato > fromDate && x.Dato < toDate).ToList<Logg>();
            }
			
			var jegereDistinct = (from l in logger
			               select l.JegerId).Distinct();
			
			var ranking = new List<JegerRanking>();
			foreach(var jegerid in jegereDistinct){
				var hits = (from l in logger
							where l.JegerId == jegerid
							select l.Treff).Sum();
				
				var shots = (from l in logger
							where l.JegerId == jegerid
							select l.Skudd).Sum();
				
				ranking.Add(new JegerRanking(){ JegerId = jegerid, Hits = hits, Shots = shots });
			}
			
			return ranking.OrderByDescending(x => x.Hits).ToList<JegerRanking>();
		}

	}
	
	public class StatsBesteJegereTableSource : UITableViewSource
	{
		private StatsBesteJegere _controller;
		public List<JegerRanking> ItemList = new List<JegerRanking>();
		
		public StatsBesteJegereTableSource(StatsBesteJegere controller)
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
			var cell = tableView.DequeueReusableCell("StatsJegererCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Subtitle, "StatsJegererCell");
			
			var jeger = JaktLoggApp.instance.GetJeger(currentItem.JegerId);
			cell.TextLabel.Text = jeger.Navn;
			cell.DetailTextLabel.Text = currentItem.Hits + " treff av "+currentItem.Shots + " skudd";
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			
			var imgstr = Utils.GetPath("jeger_"+jeger.ID+".jpg");
			
			if(!File.Exists(imgstr)){
				imgstr = "Images/Icons/icon_placeholder_jeger.png";
				cell.ImageView.Image = new UIImage(imgstr);
			}
			else
				cell.ImageView.Image = new UIImage(Utils.GetPath(imgstr));
			
			cell.ImageView.Layer.MasksToBounds = true;
			cell.ImageView.Layer.CornerRadius = 5.0f;
				
			return cell;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var item = ItemList.ElementAt(indexPath.Row);
			var jeger = JaktLoggApp.instance.GetJeger(item.JegerId);
			var jegerScreen = new JegerScreen(jeger, screen => {
				Jeger j = screen.jeger;
				JaktLoggApp.instance.SaveJegerItem(j);
			});
			//artScreen.ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal;
			//_controller.PresentModalViewController(artScreen, true);
			_controller.NavigationController.PushViewController(jegerScreen, true);
		}
	}
	public class JegerRanking
	{
		public int JegerId;
		public int Hits;
		public int Shots;
	}
}


