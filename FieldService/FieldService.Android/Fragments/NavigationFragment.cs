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
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Fragments {
    /// <summary>
    /// Fragment for the navigation menu as well as the active assignment's timer
    /// </summary>
    public class NavigationFragment : Fragment, AdapterView.IOnItemClickListener, AdapterView.IOnItemSelectedListener {
        ListView navigationListView;
        Spinner navigationStatus;
        ImageView navigationStatusImage;
        TextView timerHours;
        ToggleButton timer;
        RelativeLayout timerLayout;
        int lastposition,
            listViewIndex;
        Assignment assignment;
        AssignmentViewModel assignmentViewModel;
        public event EventHandler<EventArgs<int>> NavigationSelected;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            assignmentViewModel.HoursChanged += (sender, e) => {
                if (timerHours != null && Activity != null) {
                    Activity.RunOnUiThread (() => {
                        timerHours.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
                    });
                }
            };

            if (savedInstanceState != null && savedInstanceState.ContainsKey (Constants.BundleIndex)) {
                listViewIndex = savedInstanceState.GetInt (Constants.BundleIndex);
            }

            assignment = assignmentViewModel.ActiveAssignment;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.NavigationLayout, null, true);
            navigationListView = view.FindViewById<ListView> (Resource.Id.navigationListView);
            navigationStatus = view.FindViewById<Spinner> (Resource.Id.fragmentStatus);
            navigationStatusImage = view.FindViewById<ImageView> (Resource.Id.fragmentStatusImage);
            timerLayout = view.FindViewById<RelativeLayout> (Resource.Id.fragmentTimerTextLayout);
            timer = view.FindViewById<ToggleButton> (Resource.Id.fragmentTimer);
            timerHours = view.FindViewById<TextView> (Resource.Id.fragmentHours);

            navigationStatusImage.SetImageResource (Resource.Drawable.HoldImage);
            var spinnerAdapter = new SpinnerAdapter (assignmentViewModel.AvailableStatuses, Activity, Resource.Layout.SimpleSpinnerItem);
            spinnerAdapter.TextColor = Color.White;
            navigationStatus.Adapter = spinnerAdapter;
            navigationStatus.OnItemSelectedListener = this;
            timerLayout.Visibility = ViewStates.Gone;

            var adapter = new NavigationAdapter (Activity, Resource.Layout.NavigationListItemLayout, Constants.Navigation);
            navigationListView.OnItemClickListener = this;
            navigationListView.Adapter = adapter;

            SetActiveAssignment ();

            return view;
        }

        /// <summary>
        /// Sets up the UI for the active assignment
        /// </summary>
        private void SetActiveAssignment ()
        {
            if (assignment != null && assignment.Status == AssignmentStatus.Active) {
                timerLayout.Visibility = ViewStates.Visible;
                navigationStatusImage.SetImageResource (Resource.Drawable.EnrouteImage);
                navigationStatus.SetSelection (assignmentViewModel.AvailableStatuses.ToList ().IndexOf (AssignmentStatus.Active));
                timerHours.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");

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
            } else {
                navigationStatusImage.SetImageResource (Resource.Drawable.HoldImage);
                timerLayout.Visibility = ViewStates.Gone;
            }
        }

        /// <summary>
        /// Saves the assignment
        /// </summary>
        private void SaveAssignment ()
        {
            assignmentViewModel.SaveAssignment (assignment).ContinueOnUIThread (_ => {
                SetActiveAssignment ();
            });
        }

        /// <summary>
        /// Navigates to the appropriate section when clicked
        /// </summary>
        public void OnItemClick (AdapterView parent, View view, int position, long id)
        {
            if (position != lastposition) {

                if (navigationListView.FirstVisiblePosition <= lastposition && lastposition <= navigationListView.LastVisiblePosition) {
                    var oldView = navigationListView.GetChildAt (lastposition);
                    if (view != null) {
                        navigationListView.Adapter.GetView (lastposition, oldView, navigationListView);
                    }
                }
                var image = view.FindViewById<ImageView> (Resource.Id.navigationListViewImage);
                image.Visibility = ViewStates.Visible;
                lastposition = position;
                //need to switch fragments here
                OnNavigationSelected (position);
            }
        }

        /// <summary>
        /// Change the assignment status
        /// </summary>
        public void OnItemSelected (AdapterView parent, View view, int position, long id)
        {
            var status = assignmentViewModel.AvailableStatuses [position];
            if (assignment != null && assignment.Status != status) {
                assignment.Status = status;
                SaveAssignment ();
            }
        }

        /// <summary>
        /// Required for AdapterView.IOnItemSelectedListener interface
        /// </summary>
        /// <param name="parent"></param>
        public void OnNothingSelected (AdapterView parent)
        {
            //do nothing
        }

        /// <summary>
        /// Sets the selected navigation item
        /// </summary>
        public void SetNavigation (int index)
        {
            navigationListView.SetSelection (index);
            OnItemClick (navigationListView, navigationListView.GetChildAt (index), index, 0);
        }

        /// <summary>
        /// Fires the NavigationSelected event
        /// </summary>
        private void OnNavigationSelected (int index)
        {
            var method = NavigationSelected;
            if (method != null)
                method (this, new EventArgs<int> { Value = index });
        }

        /// <summary>
        /// Save the selected index
        /// </summary>
        /// <param name="outState"></param>
        public override void OnSaveInstanceState (Bundle outState)
        {
            base.OnSaveInstanceState (outState);
            outState.PutInt (Constants.BundleIndex, lastposition);
        }

        /// <summary>
        /// Clear out the assignment
        /// </summary>
        public override void OnPause ()
        {
            assignment = null;
            base.OnPause ();
        }
    }
}