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
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Fragments {
    public class ItemFragment : Fragment {
        TextView number,
            name,
            phone,
            address,
            items;
        Button addItems;
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

            number = view.FindViewById<TextView> (Resource.Id.itemFragmentNumber);
            name = view.FindViewById<TextView> (Resource.Id.itemFragmentContactName);
            phone = view.FindViewById<TextView> (Resource.Id.itemFragmentPhoneNumber);
            address = view.FindViewById<TextView> (Resource.Id.itemFragmentAddress);
            items = view.FindViewById<TextView> (Resource.Id.itemFragmentTotalItems);
            addItems = view.FindViewById<Button> (Resource.Id.itemFragmentAddItem);
            itemsListView = view.FindViewById<ListView> (Resource.Id.itemsListViewFragment);
            itemViewModel.LoadAssignmentItems (Assignment).ContinueOnUIThread (_ => {
                itemsListView.Adapter = new ItemsAdapter (this.Activity, Resource.Layout.ItemLayout, itemViewModel.AssignmentItems);
            });

            number.Text = Assignment.Priority.ToString ();
            name.Text = Assignment.ContactName;
            phone.Text = Assignment.ContactPhone;
            address.Text = string.Format ("{0}\n{1}, {2} {3}", Assignment.Address, Assignment.City, Assignment.State, Assignment.Zip);
            items.Text = string.Format ("({0}) Items", Assignment.TotalItems.ToString ());

            return view;
        }

        public Assignment Assignment
        {
            get;
            set;
        }
    }
}