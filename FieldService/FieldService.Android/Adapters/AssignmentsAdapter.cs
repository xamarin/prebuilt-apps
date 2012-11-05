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

using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using Orientation = Android.Content.Res.Orientation;

namespace FieldService.Android {
    /// <summary>
    /// Adapter for a list of assignments
    /// </summary>
    public class AssignmentsAdapter : ArrayAdapter<Assignment> {

        AssignmentViewModel assignmentViewModel;
        IList<Assignment> assignments;
        int resourceId;

        public AssignmentsAdapter (Context context, int resourceId, IList<Assignment> assignments)
            : base (context, resourceId, assignments)
        {
            this.assignments = assignments;
            this.resourceId = resourceId;
            this.assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            Assignment assignment = null;
            var view = convertView;
            if (view == null) {
                LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
                view = inflator.Inflate (resourceId, null);
            }

            if (assignments.Count > position) {
                assignment = assignments [position];
            }

            if (assignment == null) {
                return view;
            }

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
            var accept = view.FindViewById<Button> (Resource.Id.assignmentAccept);
            accept.Click += (sender, e) => {
                var activeAssignment = GetItem (position);
                activeAssignment.Status = AssignmentStatus.Hold;
                SaveAssignment (activeAssignment, position);
            };

            var decline = view.FindViewById<Button> (Resource.Id.assignmentDecline);
            decline.Click += (sender, e) => {
                var activeAssignment = GetItem (position);
                activeAssignment.Status = AssignmentStatus.Declined;
                SaveAssignment (activeAssignment, position);
            };

            if (assignment.Status == AssignmentStatus.New) {
                buttonLayout.Visibility = ViewStates.Visible;
                timerLayout.Visibility = ViewStates.Gone;

            } else {
                buttonLayout.Visibility = ViewStates.Gone;
                timerLayout.Visibility = ViewStates.Visible;
                timerlinearLayout.Visibility = Context.Resources.Configuration.Orientation == Orientation.Landscape ? ViewStates.Invisible : ViewStates.Gone;

                spinner.Focusable = false;
                spinner.Tag = position;
                var adapter = new SpinnerAdapter<AssignmentStatus> (assignmentViewModel.AvailableStatuses, ServiceContainer.Resolve<AssignmentsActivity> (), Resource.Layout.SimpleSpinnerItem);
                adapter.TextColor = Context.Resources.GetColor (Resource.Color.greyspinnertext);
                adapter.Background = Color.White;
                spinner.Adapter = adapter;

                spinner.SetSelection (assignmentViewModel.AvailableStatuses.ToList ().IndexOf (assignment.Status));
                spinner.SetBackgroundResource (Resource.Drawable.trianglewhite);
                spinnerImage.SetImageResource (Resource.Drawable.HoldImage);

                spinner.ItemSelected += (sender, e) => {
                    var selected = assignmentViewModel.AvailableStatuses.ElementAtOrDefault (e.Position);
                    var index = int.Parse (e.Parent.Tag.ToString ());
                    var activeAssignment = GetItem (index);
                    if (activeAssignment.Status != selected) {
                        switch (selected) {
                            case AssignmentStatus.Active: {
                                    spinnerImage.SetImageResource (Resource.Drawable.EnrouteImage);
                                    spinnerImage.InvalidateDrawable (spinnerImage.Drawable);
                                    activeAssignment.Status = AssignmentStatus.Active;
                                    SaveAssignment (activeAssignment, index);
                                }
                                break;
                            case AssignmentStatus.Complete: {
                                    //go to confirmations
                                    activeAssignment.Status = selected;
                                    var activity = ServiceContainer.Resolve<AssignmentsActivity> ();
                                    var intent = new Intent (activity, typeof (SummaryActivity));
                                    intent.PutExtra (Constants.BundleIndex, index);
                                    intent.PutExtra (Constants.FragmentIndex, Constants.Navigation.IndexOf(Constants.Confirmations));
                                    activity.StartActivity (intent);
                                }
                                break;
                            default: {
                                    spinnerImage.SetImageResource (Resource.Drawable.HoldImage);
                                    spinnerImage.InvalidateDrawable (spinnerImage.Drawable);
                                    spinner.SetBackgroundResource (Resource.Drawable.trianglewhite);
                                    activeAssignment.Status = selected;
                                    SaveAssignment (activeAssignment, index);
                                }
                                break;
                        }
                    }
                };
            }

            number.Text = assignment.Priority.ToString ();
            job.Text = string.Format ("#{0} {1}\n{2}", assignment.JobNumber, assignment.StartDate.ToShortDateString (), assignment.Title);
            name.Text = assignment.ContactName;
            phone.Text = assignment.ContactPhone;
            address.Text = string.Format ("{0}\n{1}, {2} {3}", assignment.Address, assignment.City, assignment.State, assignment.Zip);

            return view;
        }

        /// <summary>
        /// Save assignment to the view model.
        /// </summary>
        private void SaveAssignment (Assignment assignment, int index)
        {
            assignmentViewModel.SaveAssignment (assignment).ContinueOnUIThread (_ => {
                var activity = ServiceContainer.Resolve<AssignmentsActivity> ();
                if (assignment.Status == AssignmentStatus.Active || assignment.Status == AssignmentStatus.Declined) {
                    activity.ReloadAssignments ();
                } else {
                    activity.ReloadSingleListItem (index);
                }
            });
        }
    }
}