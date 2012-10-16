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
using FieldService.ViewModels;
using FieldService.Utilities;
using FieldService.Data;

namespace FieldService.Android {
    [Activity (Label = "Assignments", Theme = "@style/CustomHoloTheme")]
    public class AssignmentsActivity : Activity {
        readonly AssignmentViewModel assignmentViewModel;
        ListView assignmentsListView;
        RadioButton priority,
            mapView;
        RelativeLayout assignmentActiveLayout;

        public AssignmentsActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.AssignmentsLayout);

            assignmentsListView = FindViewById<ListView> (Resource.Id.assignmentsListView);
            priority = FindViewById<RadioButton> (Resource.Id.priorityButton);
            mapView = FindViewById<RadioButton> (Resource.Id.mapViewButton);
            assignmentActiveLayout = FindViewById<RelativeLayout> (Resource.Id.assignmentSelectedItem);

            priority.CheckedChange += (sender, e) => {
                if (priority.Checked) {
                    priority.SetCompoundDrawablesWithIntrinsicBounds (0, 0, 0, Resource.Drawable.blueRectangle);
                } else {
                    priority.SetCompoundDrawablesWithIntrinsicBounds (0, 0, 0, Resource.Drawable.smallBlueRectangle);
                }
            };
            mapView.CheckedChange += (sender, e) => {
                if (mapView.Checked) {
                    mapView.SetCompoundDrawablesWithIntrinsicBounds (0, 0, 0, Resource.Drawable.blueRectangle);
                } else {
                    mapView.SetCompoundDrawablesWithIntrinsicBounds (0, 0, 0, Resource.Drawable.smallBlueRectangle);
                }
            };
        }


        /// <summary>
        /// Overrides resume so we can refresh the list when the activity is loaded.
        /// </summary>
        protected override void OnResume ()
        {
            base.OnResume ();

            assignmentViewModel.LoadAssignmentsAsync ().ContinueOnUIThread (_ => {
                var adapter = new AssignmentsAdapter (this, Resource.Layout.AssignmentItemLayout, assignmentViewModel.Assignments);
                assignmentsListView.Adapter = adapter;
            });
        }

        /// <summary>
        /// manual refreshing assignments list
        /// </summary>
        public void ReloadAssignments ()
        {
            assignmentViewModel.LoadAssignmentsAsync ().ContinueOnUIThread (_ => {
                if (assignmentViewModel.ActiveAssignment == null) {
                    SetActiveAssignmentVisible (false);
                } else {
                    SetActiveAssignmentVisible (true);
                }
            });
        }

        private void SetActiveAssignmentVisible (bool visible)
        {
            if (visible) {
                View view = null;
                assignmentActiveLayout.Visibility = ViewStates.Visible;
                var assignment = assignmentViewModel.ActiveAssignment;
                //look at layouts children to get view
                if (assignmentActiveLayout.ChildCount > 0) {
                    view = assignmentActiveLayout.GetChildAt (0);

                } else {
                    view = new View (this);
                    LayoutInflater inflator = (LayoutInflater)GetSystemService (Context.LayoutInflaterService);
                    view = inflator.Inflate (Resource.Layout.AssignmentItemLayout, null);
                }

                view.SetBackgroundColor (Resources.GetColor (Resource.Color.assignmentoffwhite));
                var number = view.FindViewById<TextView> (Resource.Id.assignmentItemNumber);
                var job = view.FindViewById<TextView> (Resource.Id.assignmentJob);
                var name = view.FindViewById<TextView> (Resource.Id.assignmentName);
                var phone = view.FindViewById<TextView> (Resource.Id.assignmentPhone);
                var address = view.FindViewById<TextView> (Resource.Id.assignmentAddress);
                var buttonLayout = view.FindViewById<LinearLayout> (Resource.Id.assignmentButtonLayout);
                var timerLayout = view.FindViewById<LinearLayout> (Resource.Id.assignmentTimerLayout);
                var timerlinearLayout = view.FindViewById<LinearLayout> (Resource.Id.timerLinearLayout);
                var spinner = view.FindViewById<Spinner> (Resource.Id.assignmentStatus);
                var spinnerImage = view.FindViewById<ImageView> (Resource.Id.assignmentStatusImage);
                var timer = view.FindViewById<ToggleButton> (Resource.Id.assignmentTimer);
                var timerText = view.FindViewById<TextView> (Resource.Id.assignmentTimerText);

                buttonLayout.Visibility = ViewStates.Gone;
                timerLayout.Visibility = ViewStates.Visible;
                List<string> status = new List<string> ();
                foreach (var item in Enum.GetValues (typeof (AssignmentStatus))) {
                    status.Add (item.ToString ());
                }
                spinner.Adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerItem, status);
                spinner.ItemSelected += (sender, e) => {
                    var selected = status.ElementAtOrDefault (e.Position);
                    if (selected != null) {
                        switch (selected) {
                            case "Active":
                                break;
                            default:
                                view.SetBackgroundColor (Resources.GetColor (Resource.Color.assignmentgrey));
                                spinnerImage.SetImageResource (Resource.Drawable.HoldImage);
                                break;
                        }
                    }
                };
                spinner.SetSelection (status.IndexOf (assignment.Status.ToString ()));
                number.Text = assignment.Priority.ToString();
                job.Text = string.Format("#{0} {1}\n{2}", assignment.JobNumber,  assignment.StartDate.ToShortDateString(), assignment.Title);
                name.Text = assignment.ContactName;
                phone.Text = assignment.ContactPhone;
                address.Text = string.Format ("{0}\n{1}, {2} {3}", assignment.Address, assignment.City, assignment.State, assignment.Zip);
                timerText.Text = string.Format ("{0} hr {1} min\n{2}", "10", "59", "START");

            } else {
                assignmentActiveLayout.Visibility = ViewStates.Gone;
            }
        }
    }
}