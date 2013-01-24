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
        readonly AssignmentViewModel assignmentViewModel;
        readonly MenuViewModel menuViewModel;

        public AssignmentsActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
            assignmentViewModel.HoursChanged += HoursChanged;
            menuViewModel = ServiceContainer.Resolve<MenuViewModel> ();
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

            assignmentViewModel.LoadTimerEntryAsync ().ContinueWith (_ => {
                RunOnUiThread (() => {
                    if (assignmentViewModel.Recording) {
                        timer.Checked = true;
                    } else {
                        timer.Checked = false;
                    }
                });
            });

            timer.CheckedChange += (sender, e) => {
                if (e.IsChecked != assignmentViewModel.Recording) {
                    if (assignmentViewModel.Recording) {
                        assignmentViewModel.PauseAsync ();
                    } else {
                        assignmentViewModel.RecordAsync ();
                    }
                }
            };

            activeSpinner.ItemSelected += (sender, e) => {
                if (assignment != null) {
                    var selected = assignmentViewModel.AvailableStatuses.ElementAtOrDefault (e.Position);
                    if (selected != assignment.Status) {
                        switch (selected) {
                            case AssignmentStatus.Hold:
                                assignment.Status = selected;
                                assignmentViewModel.SaveAssignmentAsync (assignment).ContinueWith (_ => RunOnUiThread (ReloadAssignments));
                                break;
                            case AssignmentStatus.Complete:
                                //go to confirmations, this is getting called multiple times.

                                var intent = new Intent (this, typeof (SummaryActivity));
                                menuViewModel.MenuIndex = Constants.Navigation.IndexOf (Constants.Confirmations);
                                StartActivity (intent);
                                break;
                            default:
                                break;
                        }
                    }
                }
            };

            mapButton.Click += (sender, e) => {
                var activity = (AssignmentTabActivity)Parent;
                var intent = new Intent (activity, typeof (SummaryActivity));
                menuViewModel.MenuIndex = Constants.Navigation.IndexOf ("Map");
                assignmentViewModel.SelectedAssignment = assignmentViewModel.ActiveAssignment;
                activity.StartActivity (intent);
            };

            phoneButton.Click += (sender, e) => {
                Extensions.MakePhoneCall (this, phone.Text);
            };
        }

        /// <summary>
        /// Overrides resume so we can refresh the list when the activity is loaded.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();

            assignmentViewModel.LoadAssignmentsAsync ().ContinueWith (_ =>
            {
                RunOnUiThread(() =>
                    {
                        if (assignmentViewModel.ActiveAssignment != null)
                        {
                            SetActiveAssignmentVisible(true);
                        }
                        else
                        {
                            SetActiveAssignmentVisible(false);
                        }
                        assignmentsListView.Adapter = new AssignmentsAdapter (this, Resource.Layout.AssignmentItemLayout, assignmentViewModel.Assignments);
                    });
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
        
        /// <summary>
        /// manual refreshing assignments list
        /// </summary>
        public void ReloadAssignments ()
        {
            assignmentViewModel.LoadAssignmentsAsync ().ContinueWith (_ => {
                RunOnUiThread (() => {
                    if (assignmentViewModel.ActiveAssignment != null) {
                        SetActiveAssignmentVisible (true);
                    } else {
                        SetActiveAssignmentVisible (false);
                    }
                    assignmentsListView.Adapter = new AssignmentsAdapter (this, Resource.Layout.AssignmentItemLayout, assignmentViewModel.Assignments);
                });
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
                assignment = assignmentViewModel.ActiveAssignment;

                buttonLayout.Visibility = ViewStates.Gone;
                timerLayout.Visibility = ViewStates.Visible;

                var adapter = new SpinnerAdapter<AssignmentStatus> (assignmentViewModel.AvailableStatuses, this, Resource.Layout.SimpleSpinnerItem);
                adapter.TextColor = Resources.GetColor (Resource.Color.greyspinnertext);
                adapter.Background = Resources.GetColor (Resource.Color.assignmentblue);
                activeSpinner.Adapter = adapter;
                activeSpinner.SetSelection (assignmentViewModel.AvailableStatuses.ToList ().IndexOf (assignment.Status));
                activeSpinner.SetBackgroundResource (Resource.Drawable.triangleblue);
                spinnerImage.SetImageResource (Resource.Drawable.EnrouteImage);

                number.Text = assignment.Priority.ToString ();
                job.Text = string.Format ("#{0} {1}\n{2}", assignment.JobNumber, assignment.StartDate.ToShortDateString (), assignment.CompanyName);
                name.Text = assignment.ContactName;
                phone.Text = assignment.ContactPhone;
                address.Text = string.Format ("{0}\n{1}, {2} {3}", assignment.Address, assignment.City, assignment.State, assignment.Zip);
                timerText.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");

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
            var activity = (AssignmentTabActivity)Parent; 
            activity.MapData = null;
            if (index != -1) {
                assignmentViewModel.SelectedAssignment = assignmentViewModel.Assignments [index];
            } else {
                assignmentViewModel.SelectedAssignment = assignmentViewModel.ActiveAssignment;
            }
            menuViewModel.MenuIndex = 0;
            StartActivity (intent);
        }
    }
}