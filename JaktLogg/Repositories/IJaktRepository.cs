using System;
using System.Collections.Generic;
namespace JaktLogg
{
	public interface IJaktRepository
	{
		List<Jakt> GetAllJaktItems();
		List<Jeger> GetAllJegerItems();
		List<Art> GetAllArtItems();
		
		List<Logg> GetAllLoggItems();
		
		void SaveJaktList(List<Jakt> jaktlist);
		void SaveJegerList(List<Jeger> jegerlist);
		void SaveLoggList(List<Logg> loggList);
		void SaveSelectedArtIdList(List<int> selectedArtIdList);
	}
}

