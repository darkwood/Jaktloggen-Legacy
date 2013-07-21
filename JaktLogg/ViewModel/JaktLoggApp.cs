using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using System.IO;
namespace JaktLogg
{
	public class JaktLoggApp
	{

		public static JaktLoggApp instance = new JaktLoggApp();
		public UITabBarController TabBarController = new UITabBarController();
		public bool ShouldStartNewJakt = false;
		public Language CurrentLanguage = Language.English;
		//private MockJaktRepository _repository;
		private FileJaktRepository _repository;
		public List<Jakt> JaktList = new List<Jakt>();
		public List<Jeger> JegerList = new List<Jeger>();
		public List<Dog> DogList = new List<Dog>();
		public List<Art> ArtList = new List<Art>();
		public List<ArtGroup> ArtGroupList = new List<ArtGroup>();
		public List<Logg> LoggList = new List<Logg>();
		public List<int> SelectedArtIds = new List<int>();
		public List<string> SelectedLoggTypeIds = new List<string>();
		public List<LoggType> LoggTypeList = new List<LoggType>();
		public List<LoggTypeGroup> LoggTypeGroupList = new List<LoggTypeGroup>();
		
		public Jakt CurrentJakt = new Jakt();
		
		private JaktLoggApp ()
		{
			//_repository = new MockJaktRepository();
			_repository = new FileJaktRepository();
		}
		
		// SAVE METHODS
		public void SaveJaktItem(Jakt item){
			
			if(item.Sted == "")
				item.Sted = Utils.Translate("jakt.jakt") + " " + item.DatoFra.ToLocalDateString();
			
			JaktList = JaktList.OrderBy(o => o.ID).ToList<Jakt>();
			var itemToUpdate = JaktList.Where(j=>j.ID == item.ID).FirstOrDefault();
			if(itemToUpdate == null)
			{
				item.ID = JaktList.Count == 0 ? 1 : JaktList.Last().ID + 1;
				JaktList.Add(item);
			}
			else
				itemToUpdate = item;
			
			_repository.SaveJaktList(JaktList);
		}
		public void SaveJegerItem(Jeger item){
			
			JegerList = JegerList.OrderBy(o => o.ID).ToList<Jeger>();
			var itemToUpdate = JegerList.Where(j=>j.ID == item.ID).FirstOrDefault();
			if(itemToUpdate == null)
			{
				item.ID = JegerList.Count == 0 ? 1 : JegerList.Last().ID + 1;
				JegerList.Add(item);
			}
			else
				itemToUpdate = item;
			
			_repository.SaveJegerList(JegerList);
		}
		
		public void SaveDogItem(Dog item){
			
			DogList = DogList.OrderBy(o => o.ID).ToList<Dog>();
			var itemToUpdate = DogList.Where(j=>j.ID == item.ID).FirstOrDefault();
			if(itemToUpdate == null)
			{
				item.ID = DogList.Count == 0 ? 1 : DogList.Last().ID + 1;
				DogList.Add(item);
			}
			else
				itemToUpdate = item;
			
			_repository.SaveDogList(DogList);
		}

		public void SaveArtItem(Art item){
			
			ArtList = ArtList.OrderBy(o => o.ID).ToList<Art>();
			var itemToUpdate = ArtList.Where(j=>j.ID == item.ID).FirstOrDefault();
			if(itemToUpdate == null)
			{
				item.ID = ArtList.Count == 0 ? 1 : ArtList.Last().ID + 1;

				ArtList.Add(item);
			}
			else
				itemToUpdate = item;
			
			_repository.SaveArtList(ArtList.Where(w => w.GroupId == 100).ToList());
		}

		public void SaveLoggItem(Logg item){
			LoggList = LoggList.OrderBy(o => o.ID).ToList<Logg>();
			var itemToUpdate = LoggList.Where(j=>j.ID == item.ID).FirstOrDefault();
			if(itemToUpdate == null)
			{
				item.ID = LoggList.Count == 0 ? 1 : LoggList.Last().ID + 1;
				LoggList.Add(item);
			}
			else
				itemToUpdate = item;
			
			_repository.SaveLoggList(LoggList);
		}
		
		public void SaveSelectedArtIds(){
			_repository.SaveSelectedArtIdList(SelectedArtIds);
		}
		
		public void SaveSelectedLoggTypeIds(){
			_repository.SaveSelectedLoggTypeIdList(SelectedLoggTypeIds);
		}
		
		
		//GET METHODS
		
		public void InitializeAllData(Action callback)
		{
			ThreadPool.QueueUserWorkItem(a => {
				
				//LoadingView.Show("Henter loggføringer...");
				JaktList = _repository.GetAllJaktItems();
				LoggList = _repository.GetAllLoggItems();
				//LoadingView.Hide();
				
				//LoadingView.Show("Henter jegere...");
				SelectedArtIds = _repository.GetSelectedArtIdList();
				SelectedLoggTypeIds = _repository.GetSelectedLoggTypeIdList();
				JegerList = _repository.GetAllJegerItems();
				DogList = _repository.GetAllDogItems();
				//LoadingView.Hide();
				
				//LoadingView.Show("Henter arter...");
				ArtList = _repository.GetAllArtItems();
				ArtGroupList = _repository.GetAllArtGroupItems();
				LoggTypeGroupList = _repository.GetAllLoggtypeGroupItems();
				LoggTypeList = _repository.GetAllLoggTypeItems();
				
				//LoadingView.Hide();
				callback();
			});
		}
		
		/*public void GetAllJaktItems(Action callback)
		{
			ThreadPool.QueueUserWorkItem(a => {
				
				JaktList = _repository.GetAllJaktItems();
				callback();
			});
		}
		
		public void GetAllJegerItems(Action callback)
		{
			ThreadPool.QueueUserWorkItem(a => {
				JegerList = _repository.GetAllJegerItems();
				callback();
			});
		}
		
		public void GetAllArtItems(Action callback)
		{
			ThreadPool.QueueUserWorkItem(a => {
				ArtList = _repository.GetAllArtItems();
				callback();
			});
		}
		
		public void GetLoggItems(Action callback)
		{
			ThreadPool.QueueUserWorkItem(a => {
				LoggList = _repository.GetAllLoggItems();
				callback();
			});
		}
		
		public void GetSelectedJaktIds(Action callback)
		{
			ThreadPool.QueueUserWorkItem(a => {
				SelectedArtIds = _repository.GetSelectedArtIdList();
				callback();
			});
		}
		
		public void GetSelectedLoggTypeIds(Action callback)
		{
			ThreadPool.QueueUserWorkItem(a => {
				SelectedLoggTypeIds = _repository.GetSelectedLoggTypeIdList();
				callback();
			});
		}
		*/
		public Art GetArt(int artId)
		{
			var item = ArtList.Where(a => a.ID == artId).FirstOrDefault();
			return item;
		}
		
		public Jeger GetJeger(int jegerId)
		{
			var item = JegerList.Where(j => j.ID == jegerId).FirstOrDefault();
			return item;
		}
		
		public Dog GetDog(int dogId)
		{
			var item = DogList.Where(j => j.ID == dogId).FirstOrDefault();
			return item;
		}
		
		public void DeleteJakt(Jakt item){
			var logglist = LoggList.Where(x => x.JaktId == item.ID).ToList<Logg>();
			foreach(var logg in logglist){
				DeleteLogg(logg);
			}
			
			DeleteFile(Utils.GetPath("jakt_" + item.ID + ".jpg"));
			
			JaktList.Remove(item);
			_repository.SaveJaktList(JaktList);
		}
		
			
		public void DeleteLogg(Logg item)
		{
			DeleteFile(Utils.GetPath("jaktlogg_" + item.ID + ".jpg"));
			LoggList.Remove(item);
			_repository.SaveLoggList(LoggList);
		}
		
		public void DeleteJeger(Jeger item){
			//remove jeger from loggs
			var logger = LoggList.Where(x => x.JegerId == item.ID);
			foreach(var logg in logger){
				logg.JegerId = 0;
			}
			_repository.SaveLoggList(LoggList);
			
			//remove jeger from jakts
			var jakts = JaktList.Where(x => x.JegerIds.Contains(item.ID));
			foreach(var jakt in jakts){
				jakt.JegerIds.Remove(item.ID);
			}
			
			//remove jeger images
			DeleteFile(Utils.GetPath("jeger_" + item.ID + ".jpg"));
			               
			//remove jeger from jegerlist
			JegerList.Remove(item);
			_repository.SaveJegerList(JegerList);
		}
		
		public void DeleteDog(Dog item){
			//remove dog from loggs
			var logger = LoggList.Where(x => x.DogId == item.ID);
			foreach(var logg in logger){
				logg.DogId = 0;
			}
			_repository.SaveLoggList(LoggList);
			
			//remove dog from jakts
			var jakts = JaktList.Where(x => x.DogIds.Contains(item.ID));
			foreach(var jakt in jakts){
				jakt.DogIds.Remove(item.ID);
			}
			
			//remove dog images
			DeleteFile(Utils.GetPath("dog_" + item.ID + ".jpg"));
			               
			//remove dog from doglist
			DogList.Remove(item);
			_repository.SaveDogList(DogList);
		}

		public void DeleteArt(Art item){
			//remove art from loggs
			var logger = LoggList.Where(x => x.ArtId == item.ID);
			foreach(var logg in logger){
				logg.ArtId = 0;
			}
			_repository.SaveLoggList(LoggList);

			//remove art from selected artlist
			if(SelectedArtIds.Contains(item.ID))
				SelectedArtIds.Remove(item.ID);

			//remove art from artlist
			ArtList.Remove(item);
			_repository.SaveArtList(ArtList);
		}
		
		public void DeleteFile(string filepath){
			if(File.Exists(filepath)){
				File.Delete(filepath);
			}
		}
			
		/*public void DeleteArt(Art item){
			//remove art from loggs
			var logger = LoggList.Where(x => x.ArtId == item.ID);
			foreach(var logg in logger){
				logg.ArtId = 0;
			}
			
			ArtList.Remove(item);
			_repository.SaveArtList(ArtList);
		}*/
	}
}

