using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using EmployeeDirectory.ViewModels;
using EmployeeDirectory.Data;

namespace EmployeeDirectory.Android
{
	[Activity (Label = "@string/app_name", MainLauncher = true, Theme = "@android:style/Theme.Holo.Light")]
	[MetaData ("android.app.default_searchable", Value = "employeedirectory.android.SearchActivity")]
	public class MainActivity : ListActivity
	{
		static IFavoritesRepository repo;

		public static IFavoritesRepository SharedFavoritesRepository {
			get { return repo; }
		}

		FavoritesViewModel viewModel;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//
			// Load the favorites
			//
			repo = XmlFavoritesRepository.Open ("Favorites.xml");

			//
			// Load the UI
			//
			SetContentView (Resource.Layout.MainActivity);
		}

		protected override void OnStart ()
		{
			base.OnStart ();

			viewModel = new FavoritesViewModel (repo, groupByLastName: false);

			ListAdapter = new PeopleGroupsAdapter () {
				ItemsSource = viewModel.Groups,
			};
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.AppActivityOptionsMenu, menu);
			var searchManager = (SearchManager)GetSystemService (Context.SearchService);
			var searchView = (SearchView)menu.FindItem (Resource.Id.MenuSearch).ActionView;
			var searchInfo = searchManager.GetSearchableInfo (ComponentName);
			searchView.SetSearchableInfo (searchInfo);
			return base.OnCreateOptionsMenu (menu);
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
