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

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using EmployeeDirectory.ViewModels;

namespace EmployeeDirectory.Android {
    [Activity (Label = "@string/app_name")]
    [MetaData ("android.app.default_searchable", Value = "employeedirectory.android.SearchActivity")]
    public class MainActivity : BaseListActivity {

        FavoritesViewModel viewModel;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            //
            // Load the UI
            //
            SetContentView (Resource.Layout.MainActivity);
        }

        protected override void OnStart ()
        {
            base.OnStart ();

            viewModel = new FavoritesViewModel (Android.Application.SharedFavoritesRepository, groupByLastName: false);

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
