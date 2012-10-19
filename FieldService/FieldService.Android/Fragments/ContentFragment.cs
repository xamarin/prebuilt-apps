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
        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.ContentLayout, container, true);

            var number = view.FindViewById<TextView> (Resource.Id.summaryItemNumber);
            var name = view.FindViewById<TextView> (Resource.Id.summaryContactName);
            var phone = view.FindViewById<TextView> (Resource.Id.summaryPhoneNumber);
            var address = view.FindViewById<TextView> (Resource.Id.summaryAddress);

            if (Assignment != null && Assignment.Status == AssignmentStatus.Active) {
                number.Text = Assignment.Priority.ToString();
                name.Text = Assignment.ContactName;
                phone.Text = Assignment.ContactPhone;
                address.Text = string.Format ("{0}\n{1}, {2} {3}", Assignment.Address, Assignment.City, Assignment.State, Assignment.Zip);
            }
            return view;
        }

        public Assignment Assignment
        {
            get;
            set;
        }
    }
}