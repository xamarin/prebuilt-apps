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
    public class SummaryFragment : Fragment{
        TextView number,
            name,
            phone,
            address,
            items,
            laborhours,
            expenses,
            description,
            descriptionHeader;
        
        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.SummaryLayout, container, true);

            number = view.FindViewById<TextView> (Resource.Id.summaryItemNumber);
            name = view.FindViewById<TextView> (Resource.Id.summaryContactName);
            phone = view.FindViewById<TextView> (Resource.Id.summaryPhoneNumber);
            address = view.FindViewById<TextView> (Resource.Id.summaryAddress);
            items = view.FindViewById<TextView> (Resource.Id.summaryAssignmentItems);
            laborhours = view.FindViewById<TextView> (Resource.Id.summaryAssignmentLaborHours);
            expenses = view.FindViewById<TextView> (Resource.Id.summaryAssignmentExpenses);
            description = view.FindViewById<TextView> (Resource.Id.summaryAssignmentDescription);
            descriptionHeader = view.FindViewById<TextView> (Resource.Id.summaryAssignmentDescriptionHeader);

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
                    description.Text = value.Description;
                    descriptionHeader.Text = value.Title;
                    items.Text = value.TotalItems.ToString();
                    laborhours.Text = value.TotalHours.ToString ();
                    expenses.Text = value.TotalExpenses.ToString ("$#.00");
                }
            }
        }
    }
}