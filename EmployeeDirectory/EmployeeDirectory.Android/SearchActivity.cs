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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using EmployeeDirectory.ViewModels;
using EmployeeDirectory.Data;

namespace EmployeeDirectory.Android
{
	[Activity (Label = "Search", Theme = "@android:style/Theme.Holo.Light")]
	[IntentFilter (new[] { "android.intent.action.SEARCH" })]
	[MetaData ("android.app.searchable", Resource = "@xml/searchable")]
	public class SearchActivity : ListActivity
	{
		SearchViewModel searchViewModel;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//
			// Initialize the service
			//
			var service = new LdapDirectoryService {
				Host = "ldap.mit.edu",
				SearchBase = "dc=mit,dc=edu",
			};

			searchViewModel = new SearchViewModel (service, new Search ("Default")) {
				GroupByLastName = false,
			};
			searchViewModel.SearchCompleted += HandleSearchCompleted;

			//
			// Construct the UI
			//

			SetContentView (Resource.Layout.SearchActivity);

			ListAdapter = new PeopleGroupsAdapter () {
				ItemsSource = searchViewModel.Groups,
			};

			//
			// Start the search
			//
			var intent = Intent;
			if (Intent.ActionSearch.Equals (intent.Action)) {
				var query = intent.GetStringExtra (SearchManager.Query);

				searchViewModel.SearchText = query;
				searchViewModel.SearchProperty = SearchProperty.Name;

				searchViewModel.Search ();
			}
		}

		void HandleSearchCompleted (object sender, EventArgs e)
		{
			((PeopleGroupsAdapter)ListAdapter).ItemsSource = searchViewModel.Groups;
		}

		protected override void OnListItemClick (ListView l, View v, int position, long id)
		{
			var person = ((PeopleGroupsAdapter)ListAdapter).GetPerson (position);
			if (person != null) {
				StartActivity (PersonActivity.CreateIntent (this, person));
			}
		}
	}
}

