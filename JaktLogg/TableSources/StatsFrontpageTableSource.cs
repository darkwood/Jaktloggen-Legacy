
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class StatsFrontpageTableSource : UITableViewSource 
	{
		private const string graphWidth = "300";
		private const string graphHeight = "225";
		private StatsFrontpage _controller;
		
		private List<SectionMapping> sections = new List<SectionMapping>();

		public StatsFrontpageTableSource(StatsFrontpage controller)
		{
			_controller = controller;
			var section1 = new SectionMapping("", "");
			sections.Add(section1);
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Antall felt vilt",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var statScreen = new StatsArter(); 
					_controller.NavigationController.PushViewController(statScreen, true);
				}
			});
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Antall observerte arter",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var statScreen = new StatsArter(); 
					statScreen.Tittel ="Antall observerte arter";
					statScreen.Mode = "Sett";
					_controller.NavigationController.PushViewController(statScreen, true);
				}
			});
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Beste jegere",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var statScreen = new StatsBesteJegere(); 
					_controller.NavigationController.PushViewController(statScreen, true);
				}
			});
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Treffprosent",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var statScreen = new StatsHitrate(); 
					_controller.NavigationController.PushViewController(statScreen, true);
				}
			});
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Jaktsituasjoner i kart",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var statScreen = new StatsLoggMap(JaktLoggApp.instance.LoggList); 
					_controller.NavigationController.PushViewController(statScreen, true);
				}
			});
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Beste jakttid på døgnet",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var statScreen = new StatsTimeOfDay(); 
					_controller.NavigationController.PushViewController(statScreen, true);
				}
			});
			
			/*section1.Rows.Add(new RowItemMapping {
				Label = "Treffprosent",
				GetValue = () => {
					return "";
				},
				RowSelected = () => {
					var list = JaktLoggApp.instance.LoggList;
					var shots = list.Sum(l=> l.Skudd);
					var hits = list.Sum(l=> l.Treff);
					var percent = shots > 0 ? Decimal.Round(hits*100/shots) : 0;
						
					var f = string.Format("{0} treff, {1} skudd : {2}% treff", hits, shots, percent);
					var img = string.Format("http://chart.apis.google.com/chart?chs="+graphWidth+"x"+graphHeight+"&cht=p3&chd=t:{0},{1}&chdl=Skudd|Treff", shots, hits);
					var webView = new StatsGenericWebView("Treffprosent", f, "<img src=\""+img+"\" />");
					webView.Title = "Treffprosent";
					_controller.NavigationController.PushViewController(webView, true);
				}
			});*/
			

			
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("StatsTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Subtitle, "StatsTableCell");
			
			cell.TextLabel.Text = GetCellLabel(indexPath.Section, indexPath.Row);
			cell.DetailTextLabel.Text = GetCellValue(indexPath.Section, indexPath.Row);
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			return cell;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return sections.Count;
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return sections.ElementAt(section).Rows.Count();
		}
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return sections.ElementAt(section).Header;
		}
		public override string TitleForFooter (UITableView tableView, int section)
		{
			return sections.ElementAt(section).Footer;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			sections.ElementAt(indexPath.Section).Rows.ElementAt(indexPath.Row).RowSelected();
		}
		
		
		private string GetCellLabel(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).Label;
		}
		
		private string GetCellValue(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).GetValue();
		}
	}
}

