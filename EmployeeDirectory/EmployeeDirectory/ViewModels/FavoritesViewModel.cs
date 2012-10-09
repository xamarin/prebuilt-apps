using System;
using EmployeeDirectory.Data;
using System.Collections.ObjectModel;

namespace EmployeeDirectory.ViewModels
{
	public class FavoritesViewModel : ViewModel
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

		public bool IsEmpty
		{
			get
			{
				return Groups.Count == 0;
			}
		}
	}
}

