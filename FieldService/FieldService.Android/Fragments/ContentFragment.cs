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

namespace FieldService.Android.Fragments {
    public class ContentFragment : Fragment{
        TextView number,
            name,
            phone,
            address;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            SetHasOptionsMenu (true);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.ContentLayout, container, true);

            number = view.FindViewById<TextView> (Resource.Id.summaryItemNumber);
            name = view.FindViewById<TextView> (Resource.Id.summaryContactName);
            phone = view.FindViewById<TextView> (Resource.Id.summaryPhoneNumber);
            address = view.FindViewById<TextView> (Resource.Id.summaryAddress);

            return view;
        }

        public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate (Resource.Menu.menu, menu);   
            base.OnCreateOptionsMenu (menu, inflater);
        }

        public Assignment Assignment
        {
            set
            {
                if (value != null && value.Status == AssignmentStatus.Active) {
                    number.Text = value.Priority.ToString ();
                    name.Text = value.ContactName;
                    phone.Text = value.ContactPhone;
                    address.Text = string.Format ("{0}\n{1}, {2} {3}", value.Address, value.City, value.State, value.Zip);
                }
            }
        }
    }
}