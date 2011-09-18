using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class JaktItemTableSource : UITableViewSource 
	{
		private JaktItemScreen _controller;
		public Jakt jakt;
		private List<SectionMapping> sections = new List<SectionMapping>();

		public JaktItemTableSource(JaktItemScreen controller, Jakt j)
		{
			_controller = controller;
			jakt = j;
			
			var sectionJakt = new SectionMapping("", "");
			var sectionLogg = new SectionMapping("", " ");
			var sectionDetaljer = new SectionMapping("", "");
			var sectionSlett = new SectionMapping("", "");
			
			
			sectionJakt.Rows.Add(new RowItemMapping {
				Label = "Jaktsted",
				GetValue = () => {
					return jakt.Sted;
				},
				RowSelected = () => {
					var fieldScreen = new FieldStringScreen("Jaktsted", screen => {
						jakt.Sted = screen.Value;
						_controller.Refresh();
					});
					fieldScreen.Placeholder = "Skriv inn jaktsted";
					fieldScreen.Value = jakt.Sted;
					//autosuggest:
					var steder = (from x in JaktLoggApp.instance.JaktList
					              where x.Sted != string.Empty
								  select x.Sted.ToUpper()).Distinct();
					var autosuggests = new List<ItemCount>();
					foreach(var sted in steder){
						autosuggests.Add(new ItemCount{
							Name = sted,
							Count = JaktLoggApp.instance.JaktList.Where(y => y.Sted.ToUpper() == sted).Count()
						});
					}
					fieldScreen.AutoSuggestions = autosuggests.OrderByDescending( o => o.Count ).ToList();          
					
					
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/signpost.png"
			});
			
			
			sectionJakt.Rows.Add(new RowItemMapping {
				Label = "Jegere",
				GetValue = () => {
					var c = jakt.JegerIds.Count();
					return  c == 1 ? c + " jeger" : c + " jegere";
				},
				RowSelected = () => {
					var jegerScreen = new JegerPickerScreen(jakt.JegerIds, screen => {
						jakt.JegerIds = screen.jegerIds;
						_controller.Refresh();
					});
					_controller.NavigationController.PushViewController(jegerScreen, true);
				},
				ImageFile = "Images/Icons/Tabs/Jegere.png"
			});
			
			sectionLogg.Rows.Add(new RowItemMapping {
				Label = "Loggføringer",
				GetValue = () => {
					var c = JaktLoggApp.instance.LoggList.Where(l => l.JaktId == jakt.ID).Count();
					return c == 1 ? c + " loggføring" : c + " loggføringer";
				},
				RowSelected = () => {
					_controller.Refresh();
					var loggerScreen = new LoggerScreen(jakt.ID); 
					_controller.NavigationController.PushViewController(loggerScreen, true);
				},
				ImageFile = "Images/Icons/Tabs/Jaktloggen.png"
			});
			
			sectionDetaljer.Rows.Add(new RowItemMapping {
				Label = "Dato fra",
				GetValue = () => {
					return jakt.DatoFra.ToNorwegianDateAndYearString();
				},
				RowSelected = () => {
					var fieldScreen = new FieldDatePickerScreen(screen => {
						jakt.DatoFra = screen.Date;
						
						if(jakt.DatoTil < jakt.DatoFra)
							jakt.DatoTil = jakt.DatoFra;
						_controller.Refresh();
					});
					fieldScreen.Date = jakt.DatoFra;
					fieldScreen.Mode = UIDatePickerMode.Date;
					fieldScreen.Title = "Dato";
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/calendar.png"
			}); 
			
			sectionDetaljer.Rows.Add(new RowItemMapping {
				Label = "Dato til",
				GetValue = () => {
					return jakt.DatoTil.ToNorwegianDateAndYearString();
				},
				RowSelected = () => {
					var fieldScreen = new FieldDatePickerScreen(screen => {
						jakt.DatoTil = screen.Date;
						
						if(jakt.DatoFra > jakt.DatoTil)
							jakt.DatoFra = jakt.DatoTil;
						
						_controller.Refresh();
					});
					fieldScreen.Date = jakt.DatoTil;
					fieldScreen.Mode = UIDatePickerMode.Date;
					fieldScreen.Title = "Dato til";
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/calendar.png"
			});
			
			sectionDetaljer.Rows.Add(new RowItemMapping {
				Label = "Jaktbilde",
				GetValue = () => {
					return jakt.ImagePath.Length > 0 ? "Vis / endre" : "Legg til";
				},
				RowSelected = () => {
					ShowImageView();
				},
				ImageFile = "Images/Icons/camera.png"
			});
			
			sectionDetaljer.Rows.Add(new RowItemMapping {
				Label = "Notater",
				GetValue = () => {
					return jakt.Notes;
				},
				RowSelected = () => {
					var fieldScreen = new FieldNotesScreen("Notater", screen => {
						jakt.Notes = screen.Value;
						_controller.Refresh();
					});
					fieldScreen.Value = jakt.Notes;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/notepad.png"
			});
			
			
			if(jakt.ID > 0){
				sectionSlett.Rows.Add(new RowItemMapping {
					Label = "Slett jakt",
					GetValue = () => {
						return "";
					}
				});
			}
			if(sectionJakt.Rows.Count > 0)
				sections.Add(sectionJakt);
			
			if(sectionLogg.Rows.Count > 0)
				sections.Add(sectionLogg);
			
			if(sectionDetaljer.Rows.Count > 0)
				sections.Add(sectionDetaljer);
			
			if(sectionSlett.Rows.Count > 0)
				sections.Add(sectionSlett);
			
		}
		
		private void ShowImageView(){
			if(jakt.ID == 0)
				_controller.Refresh();
				
			var filename = "jakt_" + jakt.ID.ToString() + ".jpg";
			var fieldScreen = new FieldImagePickerScreen("Jaktbilde", filename, screen => {
				//jakt.ImagePath = screen.Value;
				_controller.Refresh();
			});
			
			//fieldScreen.Value = jakt.ImagePath;
			_controller.NavigationController.PushViewController(fieldScreen, true);
		}
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			if(section > 0)
				return new HeaderTableSection(TitleForHeader(tableView, section)).View;
			
			var headerView = new HeaderJaktItem(jakt);
			headerView.HandleButtonImageTouchUpInside = HandleButtonImageTouchUpInside;
			return headerView.View;
		}
		
		private void HandleButtonImageTouchUpInside (object sender, EventArgs e)
		{
			ShowImageView();
		}

		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			if(section > 0)
				return 0.0f;
			
			return 110f;
		}
		
		public override float GetHeightForFooter (UITableView tableView, int section)
		{
			return 0.0f;
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
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("JaktItemCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Value1, "JaktItemCell");
			
			if(GetCellLabel(indexPath.Section, indexPath.Row) == "Slett jakt")
			{
				var CellDelete = new CellDeleteButton(HandleDeleteButtonTouchUpInside);
				cell = tableView.DequeueReusableCell("CellDeleteButton");
				
				if(cell == null)
				{	
					NSBundle.MainBundle.LoadNib("CellDeleteButton", CellDelete, null);
					CellDelete.ButtonText = GetCellLabel(indexPath.Section, indexPath.Row);
					CellDelete.LoadGUI();
					cell = CellDelete.Cell;
				}
				
				tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			}
			else
			{
				cell.TextLabel.Text = GetCellLabel(indexPath.Section, indexPath.Row);
				cell.DetailTextLabel.Text = GetCellValue(indexPath.Section, indexPath.Row);
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				
				string imgpath = GetImageFile(indexPath.Section, indexPath.Row);
				if(cell.TextLabel.Text == "Jaktbilde"){
					var picturefile = Utils.GetPath("jakt_"+jakt.ID+".jpg");
					if(File.Exists(picturefile))
						imgpath = picturefile;
				}
				cell.ImageView.Image = new UIImage(imgpath);
				
			}
			return cell;
		}
		
		void HandleDeleteButtonTouchUpInside ()
		{
			var actionSheet = new UIActionSheet("") {"Slett", "Avbryt"};
			actionSheet.Title = "Jakta blir slettet permanent.";
			actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 1;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object sender, UIButtonEventArgs e) 
			{	
				switch (e.ButtonIndex)
				{
				case 0:
					//Slett
					_controller.DeleteJakt();
					break;
				case 1:
					//Avbryt
					
					break;
				}
			};
			
		}
		
		
		private string GetCellLabel(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).Label;
		}
		
		private string GetCellValue(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).GetValue();
		}
			
		private string GetImageFile(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).ImageFile ?? "";
		}
	}
}

