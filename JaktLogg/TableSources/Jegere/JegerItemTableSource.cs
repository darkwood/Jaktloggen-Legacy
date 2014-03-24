
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class JegerItemTableSource : UITableViewSource 
	{
		private JegerScreen _controller;
		private Jeger _jeger;
		
		private List<SectionMapping> sections = new List<SectionMapping>();
		private CellDeleteButton CellDelete;
		private UITableViewCell delcell;
		private HeaderJeger headerJegerView;
		public JegerItemTableSource(JegerScreen controller, Jeger j)
		{
			
			_controller = controller;
			_jeger = j;

			CellDelete = new CellDeleteButton(HandleDeleteButtonTouchUpInside);
			NSBundle.MainBundle.LoadNib("CellDeleteButton", CellDelete, null);
			delcell = CellDelete.Cell;
		
			headerJegerView = new HeaderJeger(_jeger);
			headerJegerView.HandleButtonImageTouchUpInside = HandleButtonImageTouchUpInside;

			var sectionJeger = new SectionMapping("", "");
			var sectionSlett = new SectionMapping("", "");
			
			sections.Add(sectionJeger);
			sections.Add(sectionSlett);
			
			sectionJeger.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("jeger.firstname"),
				GetValue = () => {
					return _jeger.Fornavn;
				},
				RowSelected = () => {
					var fieldScreen = new FieldStringScreen(Utils.Translate("jeger.firstname"), screen => {
						_jeger.Fornavn = Utils.UppercaseFirst(screen.Value);
						_controller.Refresh();
					}); 
					fieldScreen.Value = _jeger.Fornavn;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				}
			});
			
			sectionJeger.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("jeger.lastname"),
				GetValue = () => {
					return _jeger.Etternavn;
				},
				RowSelected = () => {
					var fieldScreen = new FieldStringScreen(Utils.Translate("jeger.lastname"), screen => {
						_jeger.Etternavn = Utils.UppercaseFirst(screen.Value);
						_controller.Refresh();
					}); 
					fieldScreen.Value = _jeger.Etternavn;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				}
			});
			
			
			/*
			sectionJeger.Rows.Add(new RowItemMapping {
				Label = "Telefon",
				GetValue = () => {
					return _jeger.Phone;
				},
				RowSelected = () => {
					var fieldScreen = new FieldStringScreen("Telefon", screen => {
						_jeger.Phone = screen.Value;
						_controller.Refresh();
					}); 
					fieldScreen.Value = _jeger.Phone;
					fieldScreen.KeyboardType = UIKeyboardType.PhonePad;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				}
			});*/
			
			sectionJeger.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("jeger.profilepicture"),
				GetValue = () => {
					return _jeger.ImagePath.Length > 0 ? Utils.Translate("picture.showimage") : Utils.Translate("picture.addimage");
				},
				RowSelected = () => {
					ShowImageView();
				},
				ImageFile = "Images/Icons/camera.png"
			});
			
			sectionJeger.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("jeger.email"),
				GetValue = () => {
					return _jeger.Email;
				},
				RowSelected = () => {
					var fieldScreen = new FieldStringScreen("E-post", screen => {
						_jeger.Email = screen.Value;
						_controller.Refresh();
					}); 
					fieldScreen.Value = _jeger.Email;
					fieldScreen.KeyboardType = UIKeyboardType.EmailAddress;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				}
			});
			
			if(!_controller.IsNewItem){
				sectionSlett.Rows.Add(new RowItemMapping {
					Label = Utils.Translate("jeger.delete"),
					GetValue = () => {
						return "";
					}
				});
			}
		}
		
		private void ShowImageView(){
			if(_jeger.ID == 0)
				_controller.Refresh();
			
			var filename = "jeger_" + _jeger.ID.ToString() + ".jpg";
			var fieldScreen = new FieldImagePickerScreen("Profilbilde", filename, screen => {
				//_jeger.ImagePath = screen.Value;
				_controller.Refresh();
			});
			//fieldScreen.Value = _jeger.ImagePath;
			_controller.NavigationController.PushViewController(fieldScreen, true);
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("JegerTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Value1, "JegerTableCell");
			
			if(GetCellLabel(indexPath.Section, indexPath.Row) == Utils.Translate("jeger.delete"))
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
					var picturefile = Utils.GetPath("jeger_"+_jeger.ID+".jpg");
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
			actionSheet.Title = "Jeger blir slettet permanent, og fjernet fra alle logger";
			actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 1;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object sender, UIButtonEventArgs e) {
				Console.WriteLine(e.ButtonIndex);
				switch (e.ButtonIndex)
				{
				case 0:
					//Slett
					_controller.Delete(_controller.jeger);
					break;
				case 1:
					//Avbryt
					
					break;
				}
			};
			
		}
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			if(section == 0 && _jeger.ID > 0){
				return headerJegerView.View;
			}
			
			return tableView.TableHeaderView;
		}
		
		private void HandleButtonImageTouchUpInside (object sender, EventArgs e)
		{
			ShowImageView();
		}
		
		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			if(section == 0 && _jeger.ID > 0)
				return 120.0f;
			
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

