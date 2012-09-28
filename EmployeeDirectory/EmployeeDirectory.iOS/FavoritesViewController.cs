using System;
using MonoTouch.UIKit;
using EmployeeDirectory.Data;
using EmployeeDirectory.ViewModels;

namespace EmployeeDirectory.iOS
{
	public class FavoritesViewController : UITableViewController
	{
		FavoritesViewModel viewModel;

		public FavoritesViewController (IFavoritesRepository favoritesRepository)
		{
			Title = "Favorites";

			this.viewModel = new FavoritesViewModel (favoritesRepository, groupByLastName: true);
		}
	}
}

