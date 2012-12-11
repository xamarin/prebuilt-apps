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
using Extensions = FieldService.Android.Utilities.AndroidExtensions;

namespace FieldService.Android {
    /// <summary>
    /// Activity for the list of assignments
    /// </summary>
    [Activity (Label = "Assignments", Theme = "@style/CustomHoloTheme")]
    public class AssignmentsActivity : BaseActivity {
        ListView assignmentsListView;
        LinearLayout assignmentActiveLayout,
            buttonLayout,
            timerLayout;
        RelativeLayout mapButton,
            phoneButton;
        ToggleButton timer;
        Spinner activeSpinner;
        Assignment assignment;
        ImageView spinnerImage;
        TextView number,
            name,
            job,
            phone,
            address,
            timerText;


        public AssignmentsActivity ()
        {
            AssignmentViewModel = new AssignmentViewModel ();
            AssignmentViewModel.HoursChanged += HoursChanged;
        }

        public AssignmentViewModel AssignmentViewModel
        {
            get;
            set;
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.AssignmentsLayout);

            assignmentsListView = FindViewById<ListView> (Resource.Id.assignmentsListView);
            assignmentActiveLayout = FindViewById<LinearLayout> (Resource.Id.assignmentSelectedItem);
            assignmentActiveLayout.Click += (sender, e) => AssignmentSelected (-1);

            assignmentsListView.ItemClick += (sender, e) => AssignmentSelected (e.Position);

            //View containing the active assignment
            var view = new View (this);
            LayoutInflater inflator = (LayoutInflater)GetSystemService (Context.LayoutInflaterService);
            view = inflator.Inflate (Resource.Layout.AssignmentItemLayout, null);
            assignmentActiveLayout.AddView (view);
            view.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);
            view.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.active_assignment_selector));
            number = view.FindViewById<TextView> (Resource.Id.assignmentItemNumber);
            job = view.FindViewById<TextView> (Resource.Id.assignmentJob);
            name = view.FindViewById<TextView> (Resource.Id.assignmentName);
            phone = view.FindViewById<TextView> (Resource.Id.assignmentPhone);
            address = view.FindViewById<TextView> (Resource.Id.assignmentAddress);
            buttonLayout = view.FindViewById<LinearLayout> (Resource.Id.assignmentButtonLayout);
            timerLayout = view.FindViewById<LinearLayout> (Resource.Id.assignmentTimerLayout);
            activeSpinner = view.FindViewById<Spinner> (Resource.Id.assignmentStatus);
            spinnerImage = view.FindViewById<ImageView> (Resource.Id.assignmentStatusImage);
            timer = view.FindViewById<ToggleButton> (Resource.Id.assignmentTimer);
            timerText = view.FindViewById<TextView> (Resource.Id.assignmentTimerText);
            phoneButton = view.FindViewById<RelativeLayout> (Resource.Id.assignmentPhoneLayout);
            mapButton = view.FindViewById<RelativeLayout> (Resource.Id.assignmentAddressLayout);

            timer.CheckedChange += (sender, e) => {
                if (e.IsChecked != AssignmentViewModel.Recording) {
                    if (AssignmentViewModel.Recording) {
                        AssignmentViewModel.PauseAsync ();
                    } else {
                        AssignmentViewModel.RecordAsync ();
                    }
                }
            };

            activeSpinner.ItemSelected += (sender, e) => {
                if (assignment != null) {
                    var selected = AssignmentViewModel.AvailableStatuses.ElementAtOrDefault (e.Position);
                    if (selected != assignment.Status) {
                        switch (selected) {
                            case AssignmentStatus.Hold:
                                assignment.Status = selected;
                                AssignmentViewModel.SaveAssignmentAsync (assignment).ContinueOnUIThread (_ => {
                                    ReloadAssignments ();
                                });
                                break;
                            case AssignmentStatus.Complete:
                                //go to confirmations, this is getting called multiple times.

                                var intent = new Intent (this, typeof (SummaryActivity));
                                intent.PutExtra (Constants.BundleIndex, -1);
                                intent.PutExtra (Constants.FragmentIndex, Constants.Navigation.IndexOf (Constants.Confirmations));
                                StartActivity (intent);
                                break;
                            default:
                                break;
                        }
                    }
                }
            };

            mapButton.Click += (sender, e) => {
                var activity = ServiceContainer.Resolve<AssignmentTabActivity> ();
                var intent = new Intent (activity, typeof (SummaryActivity));
                intent.PutExtra (Constants.FragmentIndex, Constants.Navigation.IndexOf ("Map"));
                activity.SelectedAssignment = AssignmentViewModel.ActiveAssignment;
                activity.StartActivity (intent);
            };

            phoneButton.Click += (sender, e) => {
                Extensions.MakePhoneCall (this, phone.Text);
            };

            ServiceContainer.Register<AssignmentsActivity> (this);
        }

        /// <summary>
        /// Overrides resume so we can refresh the list when the activity is loaded.
        /// </summary>
        protected override void OnResume ()
        {
            base.OnResume ();

            AssignmentViewModel.LoadAssignmentsAsync ().ContinueOnUIThread (_ => {
                if (AssignmentViewModel.ActiveAssignment != null) {
                    SetActiveAssignmentVisible (true);
                } else {
                    SetActiveAssignmentVisible (false);
                }
                var adapter = new AssignmentsAdapter (this, Resource.Layout.AssignmentItemLayout, AssignmentViewModel.Assignments);
                adapter.AssignmentViewModel = AssignmentViewModel;
                assignmentsListView.Adapter = adapter;
                var activity = ServiceContainer.Resolve<AssignmentTabActivity> ();
                activity.AssignmentViewModel = AssignmentViewModel;
            });
        }

        private void HoursChanged (object sender, EventArgs e)
        {
            if (timerText != null) {
                RunOnUiThread (() => {
                    timerText.Text = AssignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
                });
            }
        }
        
        /// <summary>
        /// manual refreshing assignments list
        /// </summary>
        public void ReloadAssignments ()
        {
            AssignmentViewModel.LoadAssignmentsAsync ().ContinueOnUIThread (_ => {
                if (AssignmentViewModel.ActiveAssignment != null) {
                    SetActiveAssignmentVisible (true);
                } else {
                    SetActiveAssignmentVisible (false);
                }
                var adapter = new AssignmentsAdapter (this, Resource.Layout.AssignmentItemLayout, AssignmentViewModel.Assignments);
                adapter.AssignmentViewModel = AssignmentViewModel;
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
                assignmentActiveLayout.Visibility = ViewStates.Visible;
                assignment = AssignmentViewModel.ActiveAssignment;

                AssignmentViewModel.LoadTimerEntryAsync ().ContinueOnUIThread (_ => {
                    if (AssignmentViewModel.Recording) {
                        timer.Checked = true;
                    } else {
                        timer.Checked = false;
                    }
                });

                buttonLayout.Visibility = ViewStates.Gone;
                timerLayout.Visibility = ViewStates.Visible;

                var adapter = new SpinnerAdapter<AssignmentStatus> (AssignmentViewModel.AvailableStatuses, this, Resource.Layout.SimpleSpinnerItem);
                adapter.TextColor = Resources.GetColor (Resource.Color.greyspinnertext);
                adapter.Background = Resources.GetColor (Resource.Color.assignmentblue);
                activeSpinner.Adapter = adapter;
                activeSpinner.SetSelection (AssignmentViewModel.AvailableStatuses.ToList ().IndexOf (assignment.Status));
                activeSpinner.SetBackgroundResource (Resource.Drawable.triangleblue);
                spinnerImage.SetImageResource (Resource.Drawable.EnrouteImage);

                number.Text = assignment.Priority.ToString ();
                job.Text = string.Format ("#{0} {1}\n{2}", assignment.JobNumber, assignment.StartDate.ToShortDateString (), assignment.CompanyName);
                name.Text = assignment.ContactName;
                phone.Text = assignment.ContactPhone;
                address.Text = string.Format ("{0}\n{1}, {2} {3}", assignment.Address, assignment.City, assignment.State, assignment.Zip);
                timerText.Text = AssignmentViewModel.Hours.ToString (@"hh\:mm\:ss");

            } else {
                assignmentActiveLayout.Visibility = ViewStates.Gone;
            }
        }

        /// <summary>
        /// Calls new intent to go to navigation
        /// </summary>
        /// <param name="index"></param>
        private void AssignmentSelected (int index)
        {
            var intent = new Intent (this, typeof (SummaryActivity));
            var activity = ServiceContainer.Resolve<AssignmentTabActivity> ();
            activity.MapData = null;
            activity.AssignmentViewModel = AssignmentViewModel;
            if (index != -1) {
                activity.SelectedAssignment = AssignmentViewModel.Assignments [index];
            } else {
                activity.SelectedAssignment = AssignmentViewModel.ActiveAssignment;
            }
            intent.PutExtra (Constants.FragmentIndex, 0);
            StartActivity (intent);
        }
    }
}