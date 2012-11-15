//
//  Copyright 2012, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using System;
using MonoTouch.UIKit;
using EmployeeDirectory.Data;
using EmployeeDirectory.ViewModels;
using System.Drawing;

namespace EmployeeDirectory.iOS
{
	public class FavoritesViewController : UITableViewController
	{
		FavoritesViewModel viewModel;
		SearchViewModel searchViewModel;

		UISearchBar searchBar;
		UISearchDisplayController searchController;

		public FavoritesViewController (IFavoritesRepository favoritesRepository, IDirectoryService service, Search savedSearch)
		{
			Title = "Favorites";

			this.viewModel = new FavoritesViewModel (favoritesRepository, groupByLastName: true);
			this.searchViewModel = new SearchViewModel (service, savedSearch);

			//
			// Configure this view
			//
			TableView.DataSource = new PeopleGroupsDataSource (viewModel.Groups);
			TableView.Delegate = new PeopleGroupsDelegate (this);

			//
			// Configure the search bar
			//
			searchBar = new UISearchBar (new RectangleF (0, 0, 320, 88)) {
				ShowsScopeBar = true,
				Text = savedSearch.Text,
			};
			searchBar.ScopeButtonTitles = new[] { "Name", "Title", "Dept", "All" };

			searchController = new UISearchDisplayController (searchBar, this) {
				SearchResultsDataSource = new PeopleGroupsDataSource (searchViewModel.Groups),
			};

			TableView.TableHeaderView = searchBar;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			//
			// Deselect all cells when appearing
			//
			var sels = TableView.IndexPathsForSelectedRows;
			if (sels != null) {
				foreach (var sel in sels) {
					TableView.DeselectRow (sel, true);
				}
			}
		}
	}
}

