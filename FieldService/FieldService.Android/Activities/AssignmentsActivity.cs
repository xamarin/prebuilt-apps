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
        LinearLayout assignmentActiveLayout;

        public AssignmentsActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.AssignmentsLayout);

            assignmentsListView = FindViewById<ListView> (Resource.Id.assignmentsListView);
            assignmentActiveLayout = FindViewById<LinearLayout> (Resource.Id.assignmentSelectedItem);

            ServiceContainer.Register<AssignmentsActivity> (this);
        }


        /// <summary>
        /// Overrides resume so we can refresh the list when the activity is loaded.
        /// </summary>
        protected override void OnResume ()
        {
            base.OnResume ();

            assignmentViewModel.LoadAssignments ().ContinueOnUIThread (_ => {
                if (assignmentViewModel.ActiveAssignment != null) {
                    SetActiveAssignmentVisible (true);
                } else {
                    SetActiveAssignmentVisible (false);
                }
                var adapter = new AssignmentsAdapter (this, Resource.Layout.AssignmentItemLayout, assignmentViewModel.Assignments);
                assignmentsListView.Adapter = adapter;
            });
        }

        /// <summary>
        /// manual refreshing assignments list
        /// </summary>
        public void ReloadAssignments ()
        {
            assignmentViewModel.LoadAssignments ().ContinueOnUIThread (_ => {
                if (assignmentViewModel.ActiveAssignment != null) {
                    SetActiveAssignmentVisible (true);
                } else {
                    SetActiveAssignmentVisible (false);
                }
                var adapter = new AssignmentsAdapter (this, Resource.Layout.AssignmentItemLayout, assignmentViewModel.Assignments);
                assignmentsListView.Adapter = adapter;
            });
        }

        /// <summary>
        /// Reload the view in the listview by itself without calling to reload the list.
        /// </summary>
        /// <param name="index"></param>
        public void ReloadSingleListItem (int index)
        {
            if (assignmentsListView.FirstVisiblePosition < index && index < assignmentsListView.LastVisiblePosition) {
                var view = assignmentsListView.GetChildAt (index);
                if (view != null) {
                    assignmentsListView.Adapter.GetView (index, view, assignmentsListView);
                }
            }
        }

        /// <summary>
        /// Activate the timer for keeping track of the active assignments time.
        /// </summary>
        /// <param name="timerChanged"></param>
        private void TimerCheckedChanged (bool timerChanged)
        {

        }

        /// <summary>
        /// Set the active assignment at the top of the list.
        /// </summary>
        /// <param name="visible">boolean to set the layout visible or not</param>
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
                    assignmentActiveLayout.AddView (view);                    
                }
                view.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);
                view.SetBackgroundColor (Resources.GetColor (Resource.Color.assignmentblue));
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

                spinner.Adapter = new SpinnerAdapter (Assignment.AvailableStatuses, this);
                spinner.SetSelection (Assignment.AvailableStatuses.ToList().IndexOf (assignment.Status));
                spinner.SetBackgroundColor (Resources.GetColor (Resource.Color.assignmentblue));
                spinnerImage.SetImageResource (Resource.Drawable.EnrouteImage);

                spinner.ItemSelected += (sender, e) => {
                        var selected = Assignment.AvailableStatuses.ElementAtOrDefault (e.Position);
                        if (selected != assignment.Status) {
                            switch (selected) {
                                case AssignmentStatus.Active:
                                    break;
                                default:
                                    assignment.Status = selected;
                                    assignmentViewModel.SaveAssignment (assignment).ContinueOnUIThread (_ => {
                                        ReloadAssignments ();
                                    });
                                    break;
                            }
                        }
                };
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