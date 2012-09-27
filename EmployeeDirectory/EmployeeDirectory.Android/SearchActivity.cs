
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

			ListAdapter = new PeopleGroupsAdapter (this) {
				ItemsSource = searchViewModel.GroupedPeople,
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
			((PeopleGroupsAdapter)ListAdapter).ItemsSource = searchViewModel.GroupedPeople;
		}

		protected override void OnListItemClick (ListView l, View v, int position, long id)
		{
			Console.WriteLine (v);
		}
	}
}

