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
    public class ItemsDialog : BaseDialog, View.IOnClickListener, AdapterView.IOnItemClickListener {

        ListView itemsListView;
        ItemViewModel itemViewModel;

        public ItemsDialog (Context context)
            : base (context)
        {
            itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();
        }

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            SetContentView (Resource.Layout.ItemsPopUpLayout);
            SetCancelable (true);

            var cancel = (Button)FindViewById (Resource.Id.itemsPopupCancelButton);
            itemsListView = (ListView)FindViewById (Resource.Id.itemPopupItemsList);

            var searchText = (TextView)FindViewById (Resource.Id.itemsPopupSearchText);
            var clearText = (ImageButton)FindViewById (Resource.Id.itemsPopupSeachClear);

            itemViewModel.LoadItems ().ContinueOnUIThread (_ => {
                itemsListView.Adapter = new ItemsSearchAdapter (Context, Resource.Layout.ItemSearchListItemLayout, itemViewModel.Items);
            });

            cancel.SetOnClickListener (this);
            itemsListView.OnItemClickListener = this;
        }

        /// <summary>
        /// The selected assignment
        /// </summary>
        public Assignment Assignment
        {
            get;
            set;
        }

        /// <summary>
        /// The parent activity
        /// </summary>
        public SummaryActivity Activity
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

        /// <summary>
        /// When an item is clicked, add it to the assignment
        /// </summary>
        public void OnItemClick (AdapterView parent, View view, int position, long id)
        {
            var item = ((ItemsSearchAdapter)itemsListView.Adapter).GetAssignmentItem (position);
            itemViewModel.SaveAssignmentItem (Assignment, new AssignmentItem {
                Item = item.ID,
                Assignment = Assignment.ID,
            })
            .ContinueOnUIThread (_ => {
                var fragment = Activity.FragmentManager.FindFragmentById<ItemFragment> (Resource.Id.contentFrame);
                fragment.ReloadItems ();
                Dismiss ();
            });
        }
    }
}