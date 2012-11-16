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
//    limitations under the License.using System;

using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Dialogs;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Fragments {
    /// <summary>
    /// Fragment for the labor hours section
    /// </summary>
    public class LaborHourFragment : Fragment {
        ListView laborListView;
        LaborViewModel laborViewModel;
        AddLaborDialog laborDialog;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.LaborHoursLayout, null, true);

            laborListView = view.FindViewById<ListView> (Resource.Id.laborListViewFragment);

            ReloadLaborHours ();
            laborListView.ItemClick += (sender, e) => {
                var textView = e.View.FindViewById<TextView> (Resource.Id.laborHours);

                var labor = LaborHours.ElementAtOrDefault (textView.Tag.ToString ().ToInt ());

                laborDialog = new AddLaborDialog (Activity);
                laborDialog.Activity = Activity;
                laborDialog.Assignment = Assignment;
                laborDialog.CurrentLabor = labor;
                laborDialog.Show ();
            };
            return view;
        }

        /// <summary>
        /// Reload the view in the listview by itself without calling to reload the list.
        /// </summary>
        /// <param name="index">index of the list view item to reload</param>
        public void ReloadSingleListItem (int index)
        {
            if (laborListView.FirstVisiblePosition < index && index < laborListView.LastVisiblePosition) {
                var view = laborListView.GetChildAt (index);
                if (view != null) {
                    laborListView.Adapter.GetView (index, view, laborListView);
                }
            }
        }

        /// <summary>
        /// Reloads the labor hours in the ListView
        /// </summary>
        private void ReloadLaborHours ()
        {
            if (LaborHours != null) {
                laborListView.Adapter = new LaborHoursAdapter (Activity, Resource.Layout.LaborHoursListItemLayout, LaborHours);
            }
        }

        /// <summary>
        /// Reloads the labor hours from the view model
        /// </summary>
        public void ReloadHours ()
        {
            laborViewModel.LoadLaborHoursAsync (Assignment).ContinueOnUIThread (_ => {
                LaborHours = laborViewModel.LaborHours;
                ReloadLaborHours ();
                var items = Activity.FindViewById<TextView> (Resource.Id.selectedAssignmentTotalItems);
                items.Text = string.Format ("{0} hrs", Assignment.TotalHours.TotalHours.ToString ("0.0"));
            });
        }

        /// <summary>
        /// Dismiss any child dialogs
        /// </summary>
        public override void OnPause ()
        {
            base.OnPause ();
            if (laborDialog != null) {
                if (laborDialog.IsShowing) {
                    laborDialog.Dismiss ();
                }
            }
        }

        /// <summary>
        /// The current assignment
        /// </summary>
        public Assignment Assignment
        {
            get;
            set;
        }

        /// <summary>
        /// The list of labor hours
        /// </summary>
        public List<Labor> LaborHours
        {
            get;
            set;
        }        
    }
}