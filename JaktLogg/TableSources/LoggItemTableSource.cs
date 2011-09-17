
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class LoggItemTableSource : UITableViewSource 
	{
		private LoggItemScreen _controller;
		public Logg loggItem;
		private List<SectionMapping> sections = new List<SectionMapping>();

		public LoggItemTableSource(LoggItemScreen controller)
		{
			var jeger = new Jeger();
			if(JaktLoggApp.instance.CurrentJakt.JegerIds.Count > 0)
				jeger = JaktLoggApp.instance.GetJeger(JaktLoggApp.instance.CurrentJakt.JegerIds[0]);
			
			_controller = controller;
			
			var section1 = new SectionMapping("", "");
			var section2 = new SectionMapping("", "");
			var section3 = new SectionMapping("Tilpassede felter", "Du kan legge til/fjerne felter under 'Felter' i menyen");
			var sectionSlett = new SectionMapping("", "");
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Felt/sett vilt",
				GetValue = () => {
					var lbl = "";
					if(loggItem.ArtId > 0)
						lbl = loggItem.Treff + " " + JaktLoggApp.instance.GetArt(loggItem.ArtId).Navn + " på " + loggItem.Skudd + " skudd";
					else 
						lbl = loggItem.Skudd + " skudd, " + loggItem.Treff + " treff";
					
					if(loggItem.Sett > 0)
						lbl += ", " + loggItem.Sett + " sett ";
					
					return lbl;
				},
				RowSelected = () => {
					var fieldScreen = new FieldLoggCatch("Felt/sett vilt", screen => {
						loggItem.ArtId = screen.CurrentArtId;
						loggItem.Treff = screen.CurrentHits;
						loggItem.Skudd = screen.CurrentShots;
						loggItem.Sett = screen.CurrentSeen;
						_controller.Refresh();
					});
					fieldScreen.CurrentArtId = loggItem.ArtId;
					fieldScreen.CurrentHits = loggItem.Treff;
					fieldScreen.CurrentShots = loggItem.Skudd;
					fieldScreen.CurrentSeen = loggItem.Sett;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/target.png"
			});
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Posisjon",
				GetValue = () => {
					return string.IsNullOrEmpty(loggItem.Latitude) ? "Ikke satt" : "Se i kart";
				},
				RowSelected = () => {
					var fieldScreen = new FieldLocationScreen("Posisjon", loggItem.Latitude, loggItem.Longitude, screen => {
						loggItem.Latitude = screen.Latitude;
						loggItem.Longitude = screen.Longitude;
						_controller.Refresh();
					});
					fieldScreen.Latitude = loggItem.Latitude;
					fieldScreen.Longitude = loggItem.Longitude;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/pin.png"
			});
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Jeger",
				GetValue = () => {
					
					if(loggItem.JegerId == 0 && JaktLoggApp.instance.CurrentJakt.JegerIds.Count == 1){
						loggItem.JegerId = jeger.ID;
						_controller.Refresh();
					}
					else if(loggItem.JegerId == 0)
						return string.Empty;
					
					else
						jeger = JaktLoggApp.instance.GetJeger(loggItem.JegerId);
					
					
					return jeger.Navn;
				},
				RowSelected = () => {
					var jegerlist = CreateSingleSelectJegerItems();
					var fieldScreen = new FieldSingleChoiceListScreen("Velg jeger", jegerlist, screen => {
						loggItem.JegerId = screen.SelectedKey == "" ? 0 : Int32.Parse(screen.SelectedKey);
						_controller.Refresh();
					});
					fieldScreen.SelectedKey = loggItem.JegerId.ToString();
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/user.png"
			});
			
			section2.Rows.Add(new RowItemMapping {
				Label = "Tidspunkt",
				GetValue = () => {
					return loggItem.Dato.Day + "/" + loggItem.Dato.Month + " " + loggItem.Dato.Hour +":"+loggItem.Dato.Minute;
				},
				RowSelected = () => {
					var fieldScreen = new FieldDatePickerScreen(screen => {
						loggItem.Dato = screen.Date;
						_controller.Refresh();
					}); 
					fieldScreen.Date = new DateTime(JaktLoggApp.instance.CurrentJakt.DatoFra.Year, 
					                                loggItem.Dato.Month, 
					                                loggItem.Dato.Day, 
					                                loggItem.Dato.Hour, 
					                                loggItem.Dato.Minute, 
					                                loggItem.Dato.Second);
					fieldScreen.Mode = UIDatePickerMode.DateAndTime;
					fieldScreen.Title = "Tidspunkt";
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/calendar.png"
			});
			
			section2.Rows.Add(new RowItemMapping {
				Label = "Notater",
				GetValue = () => {
					return loggItem.Notes;
				},
				RowSelected = () => {
					var fieldScreen = new FieldNotesScreen("Notater", screen => {
						loggItem.Notes = screen.Value;
						_controller.Refresh();
					});
					fieldScreen.Value = loggItem.Notes;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/notepad.png"
			});
			
			section2.Rows.Add(new RowItemMapping {
				Label = "Bilde",
				GetValue = () => {
					return loggItem != null && loggItem.ImagePath.Length > 0 ? "Vis / endre" : "Legg til";
				},
				RowSelected = () => {
					ShowImageView();
				},
				ImageFile = "Images/Icons/camera.png"
			});
			
			/*section3.Rows.Add(new RowItemMapping {
				Label = "Værforhold",
				GetValue = () => {
					return loggItem.Weather;
				},
				RowSelected = () => {
					var options = new List<SingleSelectRowItem>();
					options.Add(new SingleSelectRowItem(){ Key = "Sol", TextLabel = "Soll" });
					options.Add(new SingleSelectRowItem(){ Key = "Overskyet", TextLabel = "Overskyet" });
					options.Add(new SingleSelectRowItem(){ Key = "Regn", TextLabel = "Regn" });
					var fieldScreen = new FieldSingleChoiceListScreen("Værforhold", options, screen => {
						loggItem.Weather = screen.SelectedKey;
						_controller.Refresh();
					});
					_controller.NavigationController.PushViewController(fieldScreen, true);
				}
			});*/
			
			foreach(var selectedLoggTypeId in JaktLoggApp.instance.SelectedLoggTypeIds)
			{
				var item = JaktLoggApp.instance.LoggTypeList.SingleOrDefault(i => i.ID == selectedLoggTypeId);
				switch(item.Navn){
					case "Vær":
					
						section3.Rows.Add(new RowItemMapping {
						Label = "Værforhold",
						GetValue = () => {
							return loggItem.Weather;
						},
						RowSelected = () => 
							{
								var options = new List<SingleSelectRowItem>();
								options.Add(new SingleSelectRowItem(){ Key = "Sol", TextLabel = "Sol" });
								options.Add(new SingleSelectRowItem(){ Key = "Delvis skyet", TextLabel = "Delvis skyet" });
								options.Add(new SingleSelectRowItem(){ Key = "Overskyet", TextLabel = "Overskyet" });
								options.Add(new SingleSelectRowItem(){ Key = "Regn", TextLabel = "Regn" });
								options.Add(new SingleSelectRowItem(){ Key = "Snø", TextLabel = "Snø" });
								var fieldScreen = new FieldSingleChoiceListScreen("Værforhold", options, screen => {
									loggItem.Weather = screen.SelectedKey;
									_controller.Refresh();
								});
								_controller.NavigationController.PushViewController(fieldScreen, true);
							}
						
						});
					break;
					case "Kjønn":
					
						section3.Rows.Add(new RowItemMapping {
						Label = "Kjønn",
						GetValue = () => {
							return loggItem.Gender;
						},
						RowSelected = () => 
						{
							var options = new List<SingleSelectRowItem>();
							options.Add(new SingleSelectRowItem(){ Key = "Hannkjønn", TextLabel = "Hannkjønn" });
							options.Add(new SingleSelectRowItem(){ Key = "Hunnkjønn", TextLabel = "Hunnkjønn" });
							
							var fieldScreen = new FieldSingleChoiceListScreen("Kjønn", options, screen => {
								loggItem.Gender = screen.SelectedKey;
								_controller.Refresh();
							});
							fieldScreen.SelectedKey = loggItem.Gender;
							_controller.NavigationController.PushViewController(fieldScreen, true);
							}
						
						});
					break;
					
					case "Tagger":
					
						section3.Rows.Add(new RowItemMapping {
						Label = "Tagger på geviret",
						GetValue = () => {
							return loggItem.Tagger.ToString();
						},
						RowSelected = () => 
							{
								var fieldScreen = new FieldNumberPickerScreen(screen => {
									loggItem.Tagger = Int32.Parse(screen.Value);
									_controller.Refresh();
								});
							fieldScreen.Value = loggItem.Tagger.ToString();
							fieldScreen.Title = "Antall tagger på gevir";
							fieldScreen.LabelExtension = " tagger";
							
							_controller.NavigationController.PushViewController(fieldScreen, true);
							}
						
						});
					break;
					
					case "Alder":
					
						section3.Rows.Add(new RowItemMapping {
						Label = "Alder",
						GetValue = () => {
							return loggItem.Age;
						},
						RowSelected = () => 
							{
								var options = new List<SingleSelectRowItem>();
								options.Add(new SingleSelectRowItem(){ Key = "Kalv", TextLabel = "Kalv" });
								options.Add(new SingleSelectRowItem(){ Key = "1 1/2 år", TextLabel = "1 1/2 år" });
								options.Add(new SingleSelectRowItem(){ Key = "2 1/2 år og eldre", TextLabel = "2 1/2 år og eldre" });
								
								var fieldScreen = new FieldSingleChoiceListScreen("Alder", options, screen => {
								loggItem.Age = screen.SelectedKey;
								_controller.Refresh();
								});
							fieldScreen.SelectedKey = loggItem.Age;
							controller.NavigationController.PushViewController(fieldScreen, true);
							
							}
						});
					break;
					
					case "Vekt":
					
						section3.Rows.Add(new RowItemMapping {
						Label = "Vekt",
						GetValue = () => {
							return loggItem.Weight == 0 ? "" : loggItem.Weight + " kg";
						},
						RowSelected = () => 
							{
								var fieldScreen = new FieldNumberPickerScreen(screen => {
									loggItem.Weight = Int32.Parse(screen.Value);
									_controller.Refresh();
								});
							fieldScreen.Value = loggItem.Weight.ToString();
							fieldScreen.Title = "Vekt i kilo";
							fieldScreen.LabelExtension = " kg";
								_controller.NavigationController.PushViewController(fieldScreen, true);
							}
						
						});
					break;
				}
			}
			
			
			if(!_controller.IsNewItem){
				sectionSlett.Rows.Add(new RowItemMapping {
					Label = "Slett loggføring",
					GetValue = () => {
						return "";
					}
				});
			}
			
			sections.Add(section1);
			sections.Add(section2);
			
			if(section3.Rows.Count > 0)
				sections.Add(section3);
			
			sections.Add(sectionSlett);
			
		}
		
		private void ShowImageView()
		{
			if(loggItem.ID == 0)
				_controller.Refresh();
			
			var filename = "jaktlogg_" + loggItem.ID.ToString() + ".jpg";
			var fieldScreen = new FieldImagePickerScreen("Jaktbilde", filename, screen => {
				//loggItem.ImagePath = screen.Value;
				_controller.Refresh();
			});
			//fieldScreen.Value = loggItem.ImagePath;
			_controller.NavigationController.PushViewController(fieldScreen, true);
		}
		
		
		private List<SingleSelectRowItem> CreateSingleSelectJegerItems()
		{
			var jegerRowItems = new List<SingleSelectRowItem>();
			foreach(var i in JaktLoggApp.instance.CurrentJakt.JegerIds)
			{
				var j = JaktLoggApp.instance.GetJeger(i);
				var item = new SingleSelectRowItem();
				item.Key = j.ID.ToString();
				item.TextLabel = j.Navn;
				var imgpath = "Images/Icons/icon_placeholder_jeger.png";
				var picturefile = Utils.GetPath("jeger_"+j.ID+".jpg");
				if(File.Exists(picturefile))
					imgpath = picturefile;
				item.Image = new UIImage(imgpath);
				
				jegerRowItems.Add(item);
			}
			return jegerRowItems;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("LoggItemCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Value1, "LoggItemCell");
			
			var lbl = GetCellLabel(indexPath.Section, indexPath.Row) ;
			if(lbl == "Slett loggføring")
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
				if(lbl == "Felt/sett vilt"){
					cell = new UIJaktTableViewCell(UITableViewCellStyle.Subtitle, "LoggItemCellSubtitle");
				}
				cell.TextLabel.Text = lbl;
				cell.DetailTextLabel.Text = GetCellValue(indexPath.Section, indexPath.Row);
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				
				string imgpath = GetImageFile(indexPath.Section, indexPath.Row);
				if(cell.TextLabel.Text == "Bilde"){
					var picturefile = Utils.GetPath("jaktlogg_"+loggItem.ID+".jpg");
					if(File.Exists(picturefile))
						imgpath = picturefile;
				}
				cell.ImageView.Image = new UIImage(imgpath);
				
			}
			
			
			return cell;
		}
		
		void HandleCellArtSkuddTreffTouchUpInside ()
		{
			throw new NotImplementedException();
		}
		
		void HandleDeleteButtonTouchUpInside ()
		{
			var actionSheet = new UIActionSheet("") {"Slett", "Avbryt"};
			actionSheet.Title = "Loggen blir slettet permanent.";
			actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 1;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object sender, UIButtonEventArgs e) 
			{
				switch (e.ButtonIndex)
				{
				case 0:
					//Slett
					_controller.DeleteLogg();
					break;
				case 1:
					//Avbryt
					
					break;
				}
			};
			
		}
		
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			if(section > 0)
				return tableView.TableHeaderView;
			
			var headerView = new HeaderLoggItem(loggItem);
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
				return 40f;
			
			return 130f;
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
			
		private string GetImageFile(int section, int row)
		{
			return sections.ElementAt(section).Rows.ElementAt(row).ImageFile ?? "";
		}
	}
}

