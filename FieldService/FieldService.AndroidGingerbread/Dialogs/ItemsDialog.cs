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

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Fragments;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Dialogs {
    /// <summary>
    /// Dialog for searching through items
    /// </summary>
    public class ItemsDialog : BaseDialog {

        readonly SummaryActivity activity;
        readonly ItemViewModel itemViewModel;
        ListView itemsListView;
        ItemsSearchAdapter searchAdapter;

        public ItemsDialog (SummaryActivity activity)
            : base (activity)
        {
            this.activity = activity;
            itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();
        }

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            SetContentView (Resource.Layout.ItemsPopUpLayout);
            SetCancelable (true);

            var cancel = (Button)FindViewById (Resource.Id.itemsPopupCancelButton);
            cancel.Click += (sender, e) => Dismiss ();

            itemsListView = (ListView)FindViewById (Resource.Id.itemPopupItemsList);
            itemsListView.ItemClick += (sender, e) => {
                var item = ((ItemsSearchAdapter)itemsListView.Adapter).GetAssignmentItem (e.Position);
                itemViewModel.SaveAssignmentItemAsync (Assignment, new AssignmentItem {
                    ItemId = item.Id,
                    AssignmentId = Assignment.Id,
                })
                .ContinueWith (_ => {
                    activity.RunOnUiThread (() => {
                        var fragment = activity.FragmentManager.FindFragmentById<ItemFragment> (Resource.Id.contentFrame);
                        fragment.ReloadItems ();
                        Dismiss ();
                    });
                });
            };

            var searchText = (EditText)FindViewById (Resource.Id.itemsPopupSearchText);
            var clearText = (ImageButton)FindViewById (Resource.Id.itemsPopupSeachClear);

            itemViewModel.LoadItemsAsync ().ContinueWith (_ => {
                activity.RunOnUiThread (() => {
                    searchAdapter = new ItemsSearchAdapter (Context, Resource.Layout.ItemSearchListItemLayout, itemViewModel.Items);
                    itemsListView.Adapter = searchAdapter;
                });
            });
            searchText.TextChanged += (sender, e) => {
                searchAdapter.FilterItems (searchText.Text);
                searchAdapter.NotifyDataSetChanged ();
            };
            clearText.Click += (sender, e) => {
                searchText.Text = string.Empty;
                searchAdapter.NotifyDataSetChanged ();
            };
        }

        /// <summary>
        /// The selected assignment
        /// </summary>
        public Assignment Assignment
        {
            get;
            set;
        }
        
        public void OnClick (View v)
        {
            if (v.Id == Resource.Id.itemsPopupCancelButton) {
                Dismiss ();
            }
        }
    }
}