using System;
using EmployeeDirectory.Data;
using System.Collections.ObjectModel;

namespace EmployeeDirectory.ViewModels
{
	public class FavoritesViewModel
	{
		public ObservableCollection<PeopleGroup> Groups { get; private set; }

		public FavoritesViewModel (IFavoritesRepository favoritesRepository, bool groupByLastName)
		{
			if (favoritesRepository == null) {
				throw new ArgumentNullException ("favoritesRepository");
			}

			Groups = PeopleGroup.CreateGroups (
				favoritesRepository.GetAll (),
				groupByLastName);
		}
	}
}

