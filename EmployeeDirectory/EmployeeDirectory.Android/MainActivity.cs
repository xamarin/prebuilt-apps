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

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using EmployeeDirectory.ViewModels;
using EmployeeDirectory.Data;
using System.IO;

namespace EmployeeDirectory.Android {
    [Activity (Label = "@string/app_name", MainLauncher = true, Theme = "@android:style/Theme.Holo.Light")]
    [MetaData ("android.app.default_searchable", Value = "employeedirectory.android.SearchActivity")]
    public class MainActivity : ListActivity {
        static IFavoritesRepository repo;

        public static IFavoritesRepository SharedFavoritesRepository
        {
            get { return repo; }
        }

        FavoritesViewModel viewModel;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            var filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "XamarinFavorites.xml");
            using (var stream = Assets.Open ("XamarinFavorites.xml")) {
                using (var filestream = File.Open (filePath, FileMode.Create)) {
                    stream.CopyTo (filestream);
                }
            }

            //
            // Load the favorites
            //
            repo = XmlFavoritesRepository.OpenFile (filePath);
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
