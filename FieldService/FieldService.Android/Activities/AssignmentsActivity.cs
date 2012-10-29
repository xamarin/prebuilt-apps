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
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android {
    /// <summary>
    /// Activity for the list of assignments
    /// </summary>
    [Activity (Label = "Assignments", Theme = "@style/CustomHoloTheme")]
    public class AssignmentsActivity : Activity {
        readonly AssignmentViewModel assignmentViewModel;
        ListView assignmentsListView;
        LinearLayout assignmentActiveLayout;
        ToggleButton timer;
        TextView timerText;

        public AssignmentsActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            assignmentViewModel.HoursChanged += HoursChanged;
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.AssignmentsLayout);

            assignmentsListView = FindViewById<ListView> (Resource.Id.assignmentsListView);
            assignmentActiveLayout = FindViewById<LinearLayout> (Resource.Id.assignmentSelectedItem);
            assignmentActiveLayout.Click += (sender, e) => AssignmentSelected (-1);

            assignmentsListView.ItemClick += (sender, e) => AssignmentSelected (e.Position);
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

        private void HoursChanged (object sender, EventArgs e)
        {
            if (timerText != null) {
                RunOnUiThread (() => {
                    timerText.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
                });
            }
        }
        
        protected override void OnPause ()
        {
            base.OnPause ();
            timer = null;
            timerText = null;
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
        /// <param name="index">index of the list view item to reload</param>
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
                view.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.active_assignment_selector));
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
                timer = view.FindViewById<ToggleButton> (Resource.Id.assignmentTimer);
                timerText = view.FindViewById<TextView> (Resource.Id.assignmentTimerText);

                assignmentViewModel.LoadTimerEntry ().ContinueOnUIThread (_ => {
                    if (assignmentViewModel.Recording) {
                        timer.Checked = true;
                    } else {
                        timer.Checked = false;
                    }
                });

                timer.CheckedChange += (sender, e) => {
                    if (e.IsChecked != assignmentViewModel.Recording) {
                        if (assignmentViewModel.Recording) {
                            assignmentViewModel.Pause ();
                        } else {
                            assignmentViewModel.Record ();
                        }
                    }
                };

                buttonLayout.Visibility = ViewStates.Gone;
                timerLayout.Visibility = ViewStates.Visible;

                var adapter = new SpinnerAdapter (assignmentViewModel.AvailableStatuses, this, Resource.Layout.SimpleSpinnerItem);
                adapter.TextColor = Resources.GetColor (Resource.Color.greyspinnertext);
                adapter.Background = Resources.GetColor (Resource.Color.assignmentblue);
                spinner.Adapter = adapter;
                spinner.SetSelection (assignmentViewModel.AvailableStatuses.ToList ().IndexOf (assignment.Status));
                spinner.SetBackgroundResource (Resource.Drawable.triangleblue);
                spinnerImage.SetImageResource (Resource.Drawable.EnrouteImage);

                spinner.ItemSelected += (sender, e) => {
                    var selected = assignmentViewModel.AvailableStatuses.ElementAtOrDefault (e.Position);
                    if (selected != assignment.Status) {
                        switch (selected) {
                            case AssignmentStatus.Hold:
                                assignment.Status = selected;
                                assignmentViewModel.SaveAssignment (assignment).ContinueOnUIThread (_ => {
                                    ReloadAssignments ();
                                });
                                break;
                            case AssignmentStatus.Complete:
                                //go to confirmations
                                assignment.Status = selected;
                                var intent = new Intent (this, typeof (SummaryActivity));
                                intent.PutExtra (Constants.BundleIndex, -1);
                                intent.PutExtra (Constants.FragmentIndex, Constants.Navigation.IndexOf(Constants.Confirmations));
                                StartActivity (intent);
                                break;
                            default:                                
                                break;
                        }
                    }
                };
                number.Text = assignment.Priority.ToString ();
                job.Text = string.Format ("#{0} {1}\n{2}", assignment.JobNumber, assignment.StartDate.ToShortDateString (), assignment.Title);
                name.Text = assignment.ContactName;
                phone.Text = assignment.ContactPhone;
                address.Text = string.Format ("{0}\n{1}, {2} {3}", assignment.Address, assignment.City, assignment.State, assignment.Zip);
                timerText.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");

            } else {
                assignmentActiveLayout.Visibility = ViewStates.Gone;
            }
        }

        private void AssignmentSelected (int index)
        {
            var intent = new Intent (this, typeof (SummaryActivity));
            intent.PutExtra (Constants.BundleIndex, index);
            StartActivity (intent);
        }
    }
}