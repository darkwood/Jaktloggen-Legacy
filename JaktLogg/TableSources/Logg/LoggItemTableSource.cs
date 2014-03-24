
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
		private HeaderLoggItem headerLoggItemView;
		private List<SectionMapping> sections = new List<SectionMapping>();
		private List<HeaderTableSection> headers = new List<HeaderTableSection>();

		private CellDeleteButton CellDelete;
		private UITableViewCell delcell;

		public LoggItemTableSource(LoggItemScreen controller, Logg logg)
		{
			loggItem = logg;
			_controller = controller;

			//views
			CellDelete = new CellDeleteButton(HandleDeleteButtonTouchUpInside);
			NSBundle.MainBundle.LoadNib("CellDeleteButton", CellDelete, null);
			delcell = CellDelete.Cell;

			headerLoggItemView = new HeaderLoggItem(loggItem);
			headerLoggItemView.HandleButtonImageTouchUpInside = HandleButtonImageTouchUpInside;
			headerLoggItemView.HandleButtonDogTouchUpInside = HandleButtonDogTouchUpInside;
			headerLoggItemView.HandleButtonJegerTouchUpInside = HandleButtonJegerTouchUpInside;

			//Sections and cells
			var section1 = new SectionMapping("", "");
			var section2 = new SectionMapping("", "");
			var section3 = new SectionMapping("", "");
			var sectionSlett = new SectionMapping("", "");
#region fields

			section1.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("log.header.results"),
				GetValue = () => {
					var lbl = "";
					var art = JaktLoggApp.instance.GetArt(loggItem.ArtId);
					if(art != null)
						lbl = loggItem.Treff + " " + art.Navn + " / " + loggItem.Skudd + " " + Utils.Translate("shots");
					else 
						lbl = loggItem.Skudd + " "+ Utils.Translate("shots") +", " + loggItem.Treff + " "+ Utils.Translate("hits");
					
					if(loggItem.Sett > 0)
						lbl += ", " + loggItem.Sett + " "+ Utils.Translate("seen") +" ";
					
					return lbl;
				},
				RowSelected = () => {
					var fieldScreen = new FieldLoggCatch(Utils.Translate("log.header.results"), screen => {
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
				Label = Utils.Translate("log.header.position"),
				GetValue = () => {
					return string.IsNullOrEmpty(loggItem.Latitude) ? "-" : Utils.Translate("log.viewinmap");
				},
				RowSelected = () => {
					var fieldScreen = new FieldLocationScreen(loggItem, screen => {
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
			
			/*
			 * 
			var jeger = new Jeger();
			if(JaktLoggApp.instance.CurrentJakt.JegerIds.Count > 0)
				jeger = JaktLoggApp.instance.GetJeger(JaktLoggApp.instance.CurrentJakt.JegerIds[0]);

			 * section1.Rows.Add(new RowItemMapping {
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
					ShowJegerView();
				},
				ImageFile = "Images/Icons/user.png"
			});
			
			section1.Rows.Add(new RowItemMapping {
				Label = "Hund",
				GetValue = () => {
					
					if(loggItem.DogId == 0)
						return string.Empty;

					return JaktLoggApp.instance.GetDog(loggItem.DogId).Navn;
					
				},
				RowSelected = () => {
					ShowDogView();
				},
				ImageFile =  "Images/Icons/Tabs/dog-paw.png"
			});
			*/
			section2.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("log.header.time"),
				GetValue = () => {
					return loggItem.Dato.ToLocalDateAndYearString();
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
					fieldScreen.Title = Utils.Translate("log.header.time");
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/calendar.png"
			});
			
			section2.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("log.header.notes"),
				GetValue = () => {
					return loggItem.Notes;
				},
				RowSelected = () => {
					var fieldScreen = new FieldNotesScreen(Utils.Translate("log.header.notes"), screen => {
						loggItem.Notes = screen.Value;
						_controller.Refresh();
					});
					fieldScreen.Value = loggItem.Notes;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				},
				ImageFile = "Images/Icons/notepad.png"
			});
			/*
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
			*/
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
				var item = JaktLoggApp.instance.LoggTypeList.SingleOrDefault(i => i.Key == selectedLoggTypeId);
				if(item == null)
					continue;
				
				switch(item.Key){
					case "Weather":
					
						section3.Rows.Add(new RowItemMapping {
						Label = Utils.Translate("log.header.weather"),
						GetValue = () => {
							return loggItem.Weather;
						},
						RowSelected = () => 
							{
								var fieldScreen = new FieldStringScreen(Utils.Translate("log.header.weather"), screen => {
									loggItem.Weather = screen.Value;
									_controller.Refresh();
								});
								fieldScreen.Placeholder = Utils.Translate("log.header.weather");
								fieldScreen.Value = loggItem.Weather;
								//autosuggest:
								var autoitems = (from x in JaktLoggApp.instance.LoggList
								              where x.Weather != string.Empty
											  select x.Weather.ToUpper()).Distinct();
								var autosuggests = new List<ItemCount>();
								foreach(var autoitem in autoitems){
									autosuggests.Add(new ItemCount{
										Name = autoitem,
										Count = JaktLoggApp.instance.LoggList.Where(y => y.Weather.ToUpper() == autoitem).Count()
									});
								}
								fieldScreen.AutoSuggestions = autosuggests.OrderByDescending( o => o.Count ).Take(15).ToList();          
								_controller.NavigationController.PushViewController(fieldScreen, true);
							}
						
						});
					break;
					case "Gender":
					
						section3.Rows.Add(new RowItemMapping {
						Label = Utils.Translate("log.header.gender"),
						GetValue = () => {
							return loggItem.Gender;
						},
						RowSelected = () => 
						{
							var options = new List<SingleSelectRowItem>();
							options.Add(new SingleSelectRowItem(){ Key = Utils.Translate("log.gender.male"), TextLabel = Utils.Translate("log.gender.male") });
							options.Add(new SingleSelectRowItem(){ Key = Utils.Translate("log.gender.female"), TextLabel = Utils.Translate("log.gender.female") });
							
							var fieldScreen = new FieldSingleChoiceListScreen(Utils.Translate("log.header.gender"), options, screen => {
								loggItem.Gender = screen.SelectedKey;
								_controller.Refresh();
							});
							fieldScreen.SelectedKey = loggItem.Gender;
							_controller.NavigationController.PushViewController(fieldScreen, true);
							}
						
						});
					break;
					
					case "Tags":
					
						section3.Rows.Add(new RowItemMapping {
						Label = Utils.Translate("log.header.tags"),
						GetValue = () => {
							return loggItem.Tags == 0 ? "" : loggItem.Tags.ToString();
						},
						RowSelected = () => 
							{
								var fieldScreen = new FieldNumberPickerScreen(screen => {
									loggItem.Tags = Int32.Parse(screen.Value);
									_controller.Refresh();
								});
							fieldScreen.Value = loggItem.Tags.ToString();
							fieldScreen.Title = Utils.Translate("log.header.tags");
							fieldScreen.LabelExtension = " " + Utils.Translate("log.tags");
							
							_controller.NavigationController.PushViewController(fieldScreen, true);
							}
						
						});
					break;
					
					case "Age":
					
						section3.Rows.Add(new RowItemMapping {
						Label = Utils.Translate("log.header.largeage"),
						GetValue = () => {
							return loggItem.Age;
						},
						RowSelected = () => 
							{
								var options = new List<SingleSelectRowItem>();
								options.Add(new SingleSelectRowItem(){ Key = Utils.Translate("log.kalv.name"), TextLabel = Utils.Translate("log.kalv.name") });
								options.Add(new SingleSelectRowItem(){ Key = Utils.Translate("log.kalv.age1"), TextLabel = Utils.Translate("log.kalv.age1") });
								options.Add(new SingleSelectRowItem(){ Key = Utils.Translate("log.kalv.age2"), TextLabel = Utils.Translate("log.kalv.age2") });
								
								var fieldScreen = new FieldSingleChoiceListScreen(Utils.Translate("log.header.largeage"), options, screen => {
								loggItem.Age = screen.SelectedKey;
								_controller.Refresh();
								});
							fieldScreen.SelectedKey = loggItem.Age;
							controller.NavigationController.PushViewController(fieldScreen, true);
							
							}
						});
					break;
					
					case "Weight":
					
						section3.Rows.Add(new RowItemMapping {
						Label = Utils.Translate("log.header.weight"),
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
							fieldScreen.Title = Utils.Translate("log.header.weight");
							fieldScreen.LabelExtension = " kg";
							_controller.NavigationController.PushViewController(fieldScreen, true);
							}
						
						});
					break;
					
					case "ButchWeight":
					
						section3.Rows.Add(new RowItemMapping {
						Label = Utils.Translate("log.header.butchweight"),
						GetValue = () => {
							return loggItem.ButchWeight == 0 ? "" : loggItem.ButchWeight + " kg";
						},
						RowSelected = () => 
							{
								var fieldScreen = new FieldNumberPickerScreen(screen => {
									loggItem.ButchWeight = Int32.Parse(screen.Value);
									_controller.Refresh();
								});
							fieldScreen.Value = loggItem.ButchWeight.ToString();
							fieldScreen.Title = Utils.Translate("log.header.butchweight");
							fieldScreen.LabelExtension = " kg";
							_controller.NavigationController.PushViewController(fieldScreen, true);
							}
						
						});
					break;
					
					case "WeaponType":
					
						section3.Rows.Add(new RowItemMapping {
							Label = Utils.Translate("log.header.weapontype"),
							GetValue = () => {
								return loggItem.WeaponType;
							},
							RowSelected = () => {
								var fieldScreen = new FieldStringScreen(Utils.Translate("log.header.weapontype"), screen => {
									loggItem.WeaponType = screen.Value;
									_controller.Refresh();
								});
								fieldScreen.Placeholder = Utils.Translate("log.writeweapontype");
								fieldScreen.Value = loggItem.WeaponType;
								//autosuggest:
								var autoitems = (from x in JaktLoggApp.instance.LoggList
								              where x.WeaponType != string.Empty
											  select x.WeaponType.ToUpper()).Distinct();
								var autosuggests = new List<ItemCount>();
								foreach(var autoitem in autoitems){
									autosuggests.Add(new ItemCount{
										Name = autoitem,
										Count = JaktLoggApp.instance.LoggList.Where(y => y.WeaponType.ToUpper() == autoitem).Count()
									});
								}
								fieldScreen.AutoSuggestions = autosuggests.OrderByDescending( o => o.Count ).ToList();          
								_controller.NavigationController.PushViewController(fieldScreen, true);
							},
							ImageFile = ""
						});
					
					break;
				}
			}
#endregion

			
			if(!_controller.IsNewItem){
				sectionSlett.Rows.Add(new RowItemMapping {
					Label = Utils.Translate("log.delete"),
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

			foreach(var s in sections){
				headers.Add(new HeaderTableSection(s.Header));
			}
			
		}
		
		private void ShowImageView()
		{
			if(loggItem.ID == 0)
				_controller.Refresh();
			
			var filename = "jaktlogg_" + loggItem.ID.ToString() + ".jpg";
			var fieldScreen = new FieldImagePickerScreen(Utils.Translate("jakt.image"), filename, screen => {
				_controller.Refresh();
			});
			_controller.NavigationController.PushViewController(fieldScreen, true);
		}
		private void ShowJegerView()
		{
			var jegerlist = CreateSingleSelectJegerItems();
			if(jegerlist.Count == 0){
				MessageBox.Show(Utils.Translate("log.nohunters.header"), Utils.Translate("log.nohunters.message"));
				return;
			}
			var fieldScreen = new FieldSingleChoiceListScreen(Utils.Translate("log.jeger.choose"), jegerlist, screen => {
				loggItem.JegerId = screen.SelectedKey == "" ? 0 : Int32.Parse(screen.SelectedKey);
				_controller.Refresh();
			});
			
			fieldScreen.SelectedKey = loggItem.JegerId.ToString();
			_controller.NavigationController.PushViewController(fieldScreen, true);
		}
		private void ShowDogView()
		{	
			var doglist = CreateSingleSelectDogItems();
			if(doglist.Count == 0){
				MessageBox.Show(Utils.Translate("log.nodogs.header"), Utils.Translate("log.nodogs.message"));
				return;
			}
			var fieldScreen = new FieldSingleChoiceListScreen(Utils.Translate("dogs.choose"), doglist, screen => {
				loggItem.DogId = screen.SelectedKey == "" ? 0 : Int32.Parse(screen.SelectedKey);
				_controller.Refresh();
			});
			fieldScreen.SelectedKey = loggItem.DogId.ToString();
			_controller.NavigationController.PushViewController(fieldScreen, true);
			
		}
		
		private List<SingleSelectRowItem> CreateSingleSelectJegerItems()
		{
			var jegerRowItems = new List<SingleSelectRowItem>();
			foreach(var i in JaktLoggApp.instance.CurrentJakt.JegerIds)
			{
				var j = JaktLoggApp.instance.GetJeger(i);
				if(j == null)
					continue;
				
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
		
		private List<SingleSelectRowItem> CreateSingleSelectDogItems()
		{
			var dogRowItems = new List<SingleSelectRowItem>();
			foreach(var i in JaktLoggApp.instance.CurrentJakt.DogIds)
			{
				var j = JaktLoggApp.instance.GetDog(i);
				if(j == null)
					continue;
				
				var item = new SingleSelectRowItem();
				item.Key = j.ID.ToString();
				item.TextLabel = j.Navn;
				var imgpath = "Images/Icons/Tabs/dog-paw.png";
				var picturefile = Utils.GetPath("dog_"+j.ID+".jpg");
				if(File.Exists(picturefile))
					imgpath = picturefile;
				item.Image = new UIImage(imgpath);
				
				dogRowItems.Add(item);
			}
			return dogRowItems;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("LoggItemCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Value1, "LoggItemCell");
			
			var lbl = GetCellLabel(indexPath.Section, indexPath.Row) ;
			if(lbl == Utils.Translate("log.delete"))
			{
				tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
				CellDelete.ButtonText = GetCellLabel(indexPath.Section, indexPath.Row);
				CellDelete.LoadGUI();
				return delcell;
			}
			else
			{
				if(lbl == Utils.Translate("log.header.results")){
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
			var actionSheet = new UIActionSheet("") {Utils.Translate("delete"), Utils.Translate("cancel")};
			actionSheet.Title = Utils.Translate("log.deletewarning");
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
				return headers.ElementAt(section).View;

			return headerLoggItemView.View;
		}
		
		private void HandleButtonImageTouchUpInside (object sender, EventArgs e)
		{
			ShowImageView();
		}
		private void HandleButtonDogTouchUpInside (object sender, EventArgs e)
		{
			ShowDogView();
		}
		private void HandleButtonJegerTouchUpInside (object sender, EventArgs e)
		{
			ShowJegerView();
		}
		
		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			if(section > 0 && TitleForHeader(tableView, section).Length > 0)
				return 40f;
			else if(section > 0)
				return 0f;
			
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

