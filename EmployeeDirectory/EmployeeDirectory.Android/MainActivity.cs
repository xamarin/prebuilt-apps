using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace EmployeeDirectory.Android
{
	[Activity (Label = "@string/app_name", MainLauncher = true, Theme = "@android:style/Theme.Holo.Light")]
	[MetaData ("android.app.default_searchable", Value = "employeedirectory.android.SearchActivity")]
	public class MainActivity : ListActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
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
	}
}
