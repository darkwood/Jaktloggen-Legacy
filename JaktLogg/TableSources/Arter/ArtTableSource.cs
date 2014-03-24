
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace JaktLogg
{
	public class ArtTableSource : UITableViewSource 
	{
		private ArtScreen _controller;
		private Art _art;
		private CellDeleteButton CellDelete;
		private UITableViewCell delcell;

		private List<SectionMapping> sections = new List<SectionMapping>();

		public ArtTableSource(ArtScreen controller, Art j)
		{
			_controller = controller;
			_art = j;

			CellDelete = new CellDeleteButton(HandleDeleteButtonTouchUpInside);
			NSBundle.MainBundle.LoadNib("CellDeleteButton", CellDelete, null);
			delcell = CellDelete.Cell;

			var sectionArt = new SectionMapping("", "");
			var sectionSlett = new SectionMapping("", "");
			
			sections.Add(sectionArt);
			sections.Add(sectionSlett);
			
			sectionArt.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("specie.name"),
				GetValue = () => {
					return _art.Navn;
				},
				RowSelected = () => {
					var fieldScreen = new FieldStringScreen(Utils.Translate("specie.name"), screen => {
						_art.Navn = Utils.UppercaseFirst(screen.Value);
						_controller.Refresh();
					}); 
					fieldScreen.Value = _art.Navn;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				}
			});
			
			sectionArt.Rows.Add(new RowItemMapping {
				Label = Utils.Translate("specie.wikiword"),
				GetValue = () => {
					return _art.Wikinavn;
				},
				RowSelected = () => {
					var fieldScreen = new FieldStringScreen(Utils.Translate("specie.wikiword"), screen => {
						_art.Wikinavn = screen.Value;
						_controller.Refresh();
					}); 
					fieldScreen.Value = _art.Wikinavn;
					_controller.NavigationController.PushViewController(fieldScreen, true);
				}
			});
			
			
			
			if(!_controller.IsNewItem){
				sectionSlett.Rows.Add(new RowItemMapping {
					Label = Utils.Translate("specie.delete"),
					GetValue = () => {
						return "";
					}
				});
			}
		}
	
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("ArtTableCell");
			if(cell == null)
				cell = new UIJaktTableViewCell(UITableViewCellStyle.Value1, "ArtTableCell");
			
			if(GetCellLabel(indexPath.Section, indexPath.Row) == Utils.Translate("specie.delete"))
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
			}
			return cell;
		}
		
		void HandleDeleteButtonTouchUpInside ()
		{
			var actionSheet = new UIActionSheet("") {Utils.Translate("delete"), Utils.Translate("cancel")};
			actionSheet.Title = Utils.Translate("specie.deletewarning");
			actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 1;
			actionSheet.ShowFromTabBar(JaktLoggApp.instance.TabBarController.TabBar);
			
			actionSheet.Clicked += delegate(object sender, UIButtonEventArgs e) {
				Console.WriteLine(e.ButtonIndex);
				switch (e.ButtonIndex)
				{
				case 0:
					//Slett
					_controller.Delete(_controller.art);
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

