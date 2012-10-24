//
//  Copyright 2012  Xamarin Inc.
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
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;


namespace FieldService.Android.Fragments {
    public class ItemFragment : Fragment {

        ListView itemsListView;
        ItemViewModel itemViewModel;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.ItemsFragmentLayout, null, true);
            itemsListView = view.FindViewById<ListView> (Resource.Id.itemsListViewFragment);
            ReloadAssignmentItems ();
            return view;
        }

        public void ReloadAssignmentItems ()
        {
            if (AssignmentItems != null) {
                itemsListView.Adapter = new ItemsAdapter (this.Activity, Resource.Layout.ItemLayout, AssignmentItems);
            }
        }

        public Assignment Assignment
        {
            get;
            set;
        }

        public List<AssignmentItem> AssignmentItems
        {
            get;
            set;
        }
    }
}