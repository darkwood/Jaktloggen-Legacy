
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class DogItemTableSource : UITableViewSource 
	{
		private DogScreen _controller;
		private Dog _dog;
		private CellDeleteButton CellDelete;
		private UITableViewCell delcell;

		private List<SectionMapping> sections = new List<SectionMapping>();

		public DogItemTableSource(DogScreen controller, Dog j)
		{
			_controller = controller;
			_dog = j;

			//instanciate views
			CellDelete = new CellDeleteButton(HandleDeleteButtonTouchUpInside);
			NSBundle.MainBundle.LoadNib("CellDeleteButton", CellDelete, null);
			delcell = CellDelete.Cell;

			var sectionDog = new SectionMapping("", "");
			var sectionSlett = new SectionMapping("", "");
			
			sections.Add(sectionDog);
			sections.Add(sectionSlett);
			
			sectionDog.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("dog.name"),
				GetValue = () => {
					return _dog.Navn;
				},
				RowSelected = () => {
					var fieldScreen = new FieldStringScreen(Utils.Translate("dog.name"), screen => {
						_dog.Navn = Utils.UppercaseFirst(screen.Value);
						_controller.Refresh();
					}); 
					fieldScreen.Value = _dog.Navn;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				}
			});
			
			sectionDog.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("dog.breed"),
				GetValue = () => {
					return _dog.Rase;
				},
				RowSelected = () => {
					var fieldScreen = new FieldStringScreen(Utils.Translate("dog.breed"), screen => {
						_dog.Rase = Utils.UppercaseFirst(screen.Value);
						_controller.Refresh();
					}); 
					fieldScreen.Value = _dog.Rase;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				}
			});
			
			sectionDog.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("dog.image"),
				GetValue = () => {
					return _dog.ImagePath.Length > 0 ? Utils.Translate("picture.showimage") : Utils.Translate("picture.addimage");
				},
				RowSelected = () => {
					ShowImageView();
				},
				ImageFile = "Images/Icons/camera.png"
			});
			
			sectionDog.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("dog.licencenr"),
				GetValue = () => {
					return _dog.Lisensnummer;
				},
				RowSelected = () => {
					var fieldScreen = new FieldStringScreen(Utils.Translate("dog.licencenr"), screen => {
						_dog.Lisensnummer = screen.Value;
						_controller.Refresh();
					}); 
					fieldScreen.Value = _dog.Lisensnummer;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				}
			});
			
			
			
			
			if(!_controller.IsNewItem){
				sectionSlett.Rows.Add(new RowItemMapping {
					Label = Utils.Translate("dog.delete"),
					GetValue = () => {
						return "";
					}
				});
			}
		}
		
		private void ShowImageView(){
			if(_dog.ID == 0)
				_controller.Refresh();
			
			var filename = "dog_" + _dog.ID.ToString() + ".jpg";
			var fieldScreen = new FieldImagePickerScreen(Utils.Translate("dog.image"), filename, screen => {
				//_dog.ImagePath = screen.Value;
				_controller.Refresh();
			});
			//fieldScreen.Value = _dog.ImagePath;
			_controller.NavigationController.PushViewController(fieldScreen, true);
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("DogTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Value1, "DogTableCell");
			
			if(GetCellLabel(indexPath.Section, indexPath.Row) == Utils.Translate("dog.delete"))
			{
				tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
				CellDelete.ButtonText = GetCellLabel(indexPath.Section, indexPath.Row);
				CellDelete.LoadGUI();
				return delcell;
			}
			else
			{
				cell.TextLabel.Text = GetCellLabel(indexPath.Section, indexPath.Row);
				cell.DetailTextLabel.Text = GetCellValue(indexPath.Section, indexPath.Row);
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				
				string imgpath = GetImageFile(indexPath.Section, indexPath.Row);
				if(cell.TextLabel.Text == "Profilbilde"){
					var picturefile = Utils.GetPath("dog_"+_dog.ID+".jpg");
					if(File.Exists(picturefile))
						imgpath = picturefile;
				}
				cell.ImageView.Image = new UIImage(imgpath);
			}
			return cell;
		}
		
		void HandleDeleteButtonTouchUpInside ()
		{
			var actionSheet = new UIActionSheet("") {Utils.Translate("delete"), Utils.Translate("cancel")};
			actionSheet.Title = Utils.Translate("dog.deletewarning");
			actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 1;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object sender, UIButtonEventArgs e) {
				Console.WriteLine(e.ButtonIndex);
				switch (e.ButtonIndex)
				{
				case 0:
					//Slett
					_controller.Delete(_controller.dog);
					break;
				case 1:
					//Avbryt
					
					break;
				}
			};
			
		}
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			return tableView.TableHeaderView;
		}
		
		private void HandleButtonImageTouchUpInside (object sender, EventArgs e)
		{
			ShowImageView();
		}
		
		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			return 40.0f;
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

