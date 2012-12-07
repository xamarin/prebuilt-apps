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
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.Android.Fragments {
    /// <summary>
    /// Fragment for the summary screen
    /// </summary>
    public class SummaryFragment : Fragment{
        TextView items,
            laborhours,
            expenses,
            description,
            descriptionHeader;
        RelativeLayout itemsLayout,
            expensesLayout,
            laborHoursLayout;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.SummaryLayout, null, true);

            items = view.FindViewById<TextView> (Resource.Id.summaryAssignmentItems);
            laborhours = view.FindViewById<TextView> (Resource.Id.summaryAssignmentLaborHours);
            expenses = view.FindViewById<TextView> (Resource.Id.summaryAssignmentExpenses);
            description = view.FindViewById<TextView> (Resource.Id.summaryAssignmentDescription);
            descriptionHeader = view.FindViewById<TextView> (Resource.Id.summaryAssignmentDescriptionHeader);
            itemsLayout = view.FindViewById<RelativeLayout> (Resource.Id.summaryItemsLayout);
            laborHoursLayout = view.FindViewById<RelativeLayout> (Resource.Id.summaryLaborLayout);
            expensesLayout = view.FindViewById<RelativeLayout> (Resource.Id.summaryExpensesLayout);

            if (Assignment != null) {
                description.Text = Assignment.Description;
                descriptionHeader.Text = Assignment.CompanyName;
                items.Text = Assignment.TotalItems.ToString ();
                laborhours.Text = Assignment.TotalHours.TotalHours.ToString ("0.0");
                expenses.Text = Assignment.TotalExpenses.ToString ("$#.00");
            }

            if (Assignment != null && !Assignment.IsHistory) {
                itemsLayout.Click += (sender, e) => {
                    var index = Constants.Navigation.IndexOf ("Items");
                    SelectNavigation (index);
                };
                laborHoursLayout.Click += (sender, e) => {
                    var index = Constants.Navigation.IndexOf ("Labor Hours");
                    SelectNavigation (index);
                };
                expensesLayout.Click += (sender, e) => {
                    var index = Constants.Navigation.IndexOf ("Expenses");
                    SelectNavigation (index);
                };
            }

            return view;
        }

        /// <summary>
        /// Call to the navigation fragment to select the correct value in the list view to change fragments
        /// </summary>
        /// <param name="index"></param>
        private void SelectNavigation (int index)
        {
            var fragment = Activity.FragmentManager.FindFragmentById<NavigationFragment> (Resource.Id.navigationFragmentContainer);
            fragment.SetNavigation (index);
        }

        /// <summary>
        /// The selected assignment
        /// </summary>
        public Assignment Assignment
        {
            get;
            set;
        }
    }
}