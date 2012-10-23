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

namespace FieldService.Android.Fragments {
    public class SummaryFragment : Fragment{
        TextView items,
            laborhours,
            expenses,
            description,
            descriptionHeader;
        
        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.SummaryLayout, null, true);

            items = view.FindViewById<TextView> (Resource.Id.summaryAssignmentItems);
            laborhours = view.FindViewById<TextView> (Resource.Id.summaryAssignmentLaborHours);
            expenses = view.FindViewById<TextView> (Resource.Id.summaryAssignmentExpenses);
            description = view.FindViewById<TextView> (Resource.Id.summaryAssignmentDescription);
            descriptionHeader = view.FindViewById<TextView> (Resource.Id.summaryAssignmentDescriptionHeader);

            if (Assignment != null) {
                description.Text = Assignment.Description;
                descriptionHeader.Text = Assignment.Title;
                items.Text = Assignment.TotalItems.ToString ();
                laborhours.Text = Assignment.TotalHours.ToString ();
                expenses.Text = Assignment.TotalExpenses.ToString ("$#.00");
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