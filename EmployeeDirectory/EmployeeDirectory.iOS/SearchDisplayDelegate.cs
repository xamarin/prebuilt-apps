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

using EmployeeDirectory.ViewModels;

namespace EmployeeDirectory.iOS
{
	public class SearchDisplayDelegate : UISearchDisplayDelegate
	{
		SearchViewModel searchViewModel;

		string lastSearchText = "";
		SearchProperty lastSearchProperty = SearchProperty.All;
		UITableView lastTableView;

		public SearchDisplayDelegate (SearchViewModel searchViewModel)
		{
			this.searchViewModel = searchViewModel;

			searchViewModel.SearchCompleted += HandleSearchCompleted;
		}

		void HandleSearchCompleted (object sender, SearchCompletedEventArgs e)
		{
			if (e.SearchText == lastSearchText && e.SearchProperty == lastSearchProperty) {
				if (lastTableView != null) {
					var data = (PeopleGroupsDataSource)lastTableView.DataSource;
					data.Groups = searchViewModel.Groups;
					lastTableView.ReloadData ();
				}
			}
		}

		public override bool ShouldReloadForSearchString (UISearchDisplayController controller, string forSearchString)
		{
			if (string.IsNullOrWhiteSpace (forSearchString)) {
				return true;
			} else {
				searchViewModel.SearchText = forSearchString;

				lastTableView = controller.SearchResultsTableView;
				BeginSearch ();
				return false; // We'll search asynchronously
			}
		}

		public override bool ShouldReloadForSearchScope (UISearchDisplayController controller, int forSearchOption)
		{
			switch (forSearchOption) {
			case 0:
				searchViewModel.SearchProperty = SearchProperty.Name;
				break;
			case 1:
				searchViewModel.SearchProperty = SearchProperty.Title;
				break;
			case 2:
				searchViewModel.SearchProperty = SearchProperty.Department;
				break;
			case 3:
				searchViewModel.SearchProperty = SearchProperty.All;
				break;
			}

			lastTableView = controller.SearchResultsTableView;
			BeginSearch ();
			return false; // We'll search asynchronously
		}

		void BeginSearch ()
		{
			lastSearchText = searchViewModel.SearchText;
			lastSearchProperty = searchViewModel.SearchProperty;
			searchViewModel.Search ();
		}
	}
}

