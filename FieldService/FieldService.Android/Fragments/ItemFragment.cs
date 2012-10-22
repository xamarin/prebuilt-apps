using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
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
        Assignment assignment;
        ItemViewModel itemViewModel;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.ItemsFragmentLayout, container, true);

            number = view.FindViewById<TextView> (Resource.Id.itemFragmentNumber);
            name = view.FindViewById<TextView> (Resource.Id.itemFragmentContactName);
            phone = view.FindViewById<TextView> (Resource.Id.itemFragmentPhoneNumber);
            address = view.FindViewById<TextView> (Resource.Id.itemFragmentAddress);
            items = view.FindViewById<TextView> (Resource.Id.itemFragmentTotalItems);
            addItems = view.FindViewById<Button> (Resource.Id.itemFragmentAddItem);
            itemsListView = view.FindViewById<ListView> (Resource.Id.itemsListViewFragment);
            itemViewModel.LoadAssignmentItems (assignment).ContinueOnUIThread (_ => {
                itemsListView.Adapter = new ItemsAdapter (this.Activity, Resource.Layout.ItemLayout, itemViewModel.AssignmentItems);
            });

            return view;
        }

        public Assignment Assignment
        {
            set
            {
                if (value != null) {
                    number.Text = value.Priority.ToString ();
                    name.Text = value.ContactName;
                    phone.Text = value.ContactPhone;
                    address.Text = string.Format ("{0}\n{1}, {2} {3}", value.Address, value.City, value.State, value.Zip);
                    items.Text = string.Format ("({0}) Items", value.TotalItems.ToString ());
                }
                assignment = value;
            }
        }
    }
}