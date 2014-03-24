
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.IO;
using System.Drawing;

namespace JaktLogg
{
	public class LoggerTableSource : UITableViewSource 
	{
		private LoggerScreen _controller;
		private List<Logg> _logger = new List<Logg>();
		private List<string> datelist = new List<string>();
		private List<HeaderTableSection> headers = new List<HeaderTableSection>();

		public LoggerTableSource(LoggerScreen controller, List<Logg> logger)
		{
			_controller = controller;
			_logger = logger.OrderByDescending(l => l.Dato).ToList<Logg>();
			datelist = (from logg in _logger select new DateTime(JaktLoggApp.instance.CurrentJakt.DatoFra.Year,
			                                                     logg.Dato.Month,
			                                                     logg.Dato.Day,
			                                                     logg.Dato.Hour,
			                                                     logg.Dato.Minute,
			                                                     logg.Dato.Second)
			            .ToLocalDateString()).Distinct().ToList<string>();

			foreach(var d in datelist){
				headers.Add(new HeaderTableSection(d));
			}
			//for the mapbutton
			headers.Add(new HeaderTableSection(""));


			
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			
			var row = indexPath.Row;
			var section = indexPath.Section;
			
			// IF MAP BUTTON
			if(datelist.Count() <= section)
			{
				var mapCell = tableView.DequeueReusableCell("LoggTableCellLast");
				if(mapCell == null)
					mapCell = new UIJaktTableViewCell(UITableViewCellStyle.Default, "LoggTableCellLast");
				
				mapCell.TextLabel.Text = Utils.Translate("log.showlogsinmap");
				mapCell.ImageView.Image = new UIImage("Images/Icons/radar.png");
				mapCell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				//mapCell.BackgroundColor = UIColor.FromRGB(0.4f, 0.7f, 0.4f);
				mapCell.Hidden = _logger.Where(l => l.Latitude != "").Count() == 0;
				return mapCell;
			}
			
			// ELSE
			var cell = tableView.DequeueReusableCell("LoggTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Subtitle, "LoggTableCell");
			
			var currentLogs = _logger.Where(l => l.Dato.ToLocalDateString() == datelist.ElementAt(section));
			var logg = currentLogs.ElementAt(row);
			
			cell.TextLabel.Text = logg.Dato.ToLocalTimeString();
			var art = JaktLoggApp.instance.GetArt(logg.ArtId);
			if(art != null)
				cell.DetailTextLabel.Text =  logg.Treff + " " + art.Navn + "  " + logg.Skudd + " skudd. ";
			else if(logg.Skudd > 0)
				cell.DetailTextLabel.Text = logg.Skudd + " " + Utils.Translate("shots") + ". " +
											logg.Treff + " "+ Utils.Translate("hits") + " ";
			else
				cell.DetailTextLabel.Text = "";
			
			if(logg.JegerId > 0)
				cell.DetailTextLabel.Text += JaktLoggApp.instance.GetJeger(logg.JegerId).Fornavn;
			
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			
			var imgstr = Utils.GetPath("jaktlogg_"+logg.ID+".jpg");
			
			if(!File.Exists(imgstr)){
				imgstr = "Images/Icons/pictureframe.png";
				cell.ImageView.Image = new UIImage(imgstr);
			}
			else
				cell.ImageView.Image = new UIImage(Utils.GetPath(imgstr));
			
			cell.ImageView.Layer.MasksToBounds = true;
			cell.ImageView.Layer.CornerRadius = 5.0f;
			return cell;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			if(datelist.Distinct().Count() <= section)
				return "";
			
			return datelist.Distinct().ElementAt(section);
		}
		/*
		public override string TitleForFooter (UITableView tableView, int section)
		{
			if(datelist.Count() <= section)
				return "";
			
			var currentLogs = _logger.Where(l => l.Dato.ToLocalDateString() == datelist.ElementAt(section));
			
			var c = currentLogs.Count();
			if( c == 0)
				return "Legg til ny loggføring ved å trykke på \"+\"-knappen.";
			
			if(section == c+1)
				return "";
			
			return c + " " + ((c == 1) ? "loggføring" : "loggføringer") + ".";
		}*/
		
		public override int NumberOfSections (UITableView tableView)
		{	
			return datelist.Count() + 1;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			if(datelist.Count() <= section)
				return 1;
			
			var currentLogs = _logger.Where(l => l.Dato.ToLocalDateString() == datelist.ElementAt(section));
			
			return currentLogs.Count();
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var row = indexPath.Row;
			var section = indexPath.Section;
			
			if(datelist.Count() <= section)
			{
				var mapView = new StatsLoggMap(_logger);
				_controller.NavigationController.PushViewController(mapView, true);
			}
			else
			{
				var currentLogs = _logger.Where(l => l.Dato.ToLocalDateString() == datelist.ElementAt(section));
				var logg = currentLogs.ElementAt(row);
				var loggItemScreen = new LoggItemScreen(logg, screen => {
					_controller.TableView.ReloadData();
				});
				_controller.NavigationController.PushViewController(loggItemScreen, true);
			
			}
		}
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			return headers.ElementAt(section).View;
		}


		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			if(datelist.Count() <= section)
				return 0f;
			
			return 40.0f;
		}
		
		public override string TitleForDeleteConfirmation (UITableView tableView, NSIndexPath indexPath)
		{
			return Utils.Translate("delete");
		}
		
		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			
			var actionSheet = new UIActionSheet("") {Utils.Translate("delete"), Utils.Translate("cancel")};
			actionSheet.Title = Utils.Translate("log.deletewarning");
			actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 1;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object sender, UIButtonEventArgs e) {
				Console.WriteLine(e.ButtonIndex);
				switch (e.ButtonIndex)
				{
				case 0:
					//Slett
					var loggItem = _logger.ElementAt(indexPath.Row);
					JaktLoggApp.instance.DeleteLogg(loggItem);
					_controller.Refresh();
					break;
				case 1:
					//Avbryt
					
					break;
				}
			};
			
		}
	}
}

