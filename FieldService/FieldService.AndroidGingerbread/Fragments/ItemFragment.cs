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
        
        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();

            var view = inflater.Inflate (Resource.Layout.ItemsFragmentLayout, null, true);
            itemsListView = view.FindViewById<ListView> (Resource.Id.itemsListViewFragment);
            ReloadAssignmentItems ();
            itemsListView.Enabled = !Assignment.IsHistory;
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
            AlertDialog.Builder dialog = new AlertDialog.Builder (Activity)
                .SetTitle ("Delete")
                .SetMessage ("Are you sure you want to delete this item?")
                .SetPositiveButton ("Yes", (sender, e) => {
                    itemViewModel.DeleteAssignmentItemAsync (Assignment, item).ContinueWith (_ => Activity.RunOnUiThread (ReloadItems));
                })
                .SetNegativeButton ("No", (sender, e) => { });
            dialog.Show ();
        }

        public void ReloadItems ()
        {
            itemViewModel.LoadAssignmentItemsAsync (Assignment).ContinueWith (_ => {
                Activity.RunOnUiThread (() => {
                    AssignmentItems = itemViewModel.AssignmentItems;
                    ReloadAssignmentItems ();
                    var items = Activity.FindViewById<TextView> (Resource.Id.selectedAssignmentTotalItems);
                    items.Text = string.Format ("({0}) Items", Assignment.TotalItems.ToString ());
                });
            });
        }
    }
}