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

using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Fragments {
    /// <summary>
    /// Fragment for the items section
    /// </summary>
    public class ItemFragment : Fragment{

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
            itemsListView.ItemClick += (sender, e) => {
                var checkbox = e.View.FindViewById<CheckBox> (Resource.Id.itemCheckBox);
                checkbox.Checked = !checkbox.Checked;
                checkbox.Enabled = false;
                itemViewModel
                    .SaveAssignmentItem (Assignment, AssignmentItems.ElementAtOrDefault (checkbox.Tag.ToString ().ToInt ()))
                    .ContinueOnUIThread (_ => checkbox.Enabled = true);
            };
            return view;
        }

        /// <summary>
        /// Reloads the assignment items in the Listview
        /// </summary>
        private void ReloadAssignmentItems ()
        {
            if (AssignmentItems != null) {
                var adapter = new ItemsAdapter (Activity, Resource.Layout.ItemLayout, AssignmentItems);
                adapter.Fragment = this;
                itemsListView.Adapter = adapter;
            }
        }

        /// <summary>
        /// The currrent assignment
        /// </summary>
        public Assignment Assignment
        {
            get;
            set;
        }

        /// <summary>
        /// List of assignment items
        /// </summary>
        public List<AssignmentItem> AssignmentItems
        {
            get;
            set;
        }

        /// <summary>
        /// Deletes an AssignmentItem
        /// </summary>
        public void DeleteItem (AssignmentItem item)
        {
            itemViewModel.DeleteAssignmentItem (Assignment, item).ContinueOnUIThread (_ => {
                ReloadItems ();
            });
        }

        public void ReloadItems ()
        {
            itemViewModel.LoadAssignmentItems (Assignment).ContinueOnUIThread (_ => {
                AssignmentItems = itemViewModel.AssignmentItems;
                ReloadAssignmentItems ();
                var items = Activity.FindViewById<TextView> (Resource.Id.selectedAssignmentTotalItems);
                items.Text = string.Format ("({0}) Items", Assignment.TotalItems.ToString ());
            });
        }
    }
}