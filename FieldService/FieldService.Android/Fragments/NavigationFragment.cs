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
    public class NavigationFragment : Fragment {
        ListView navigationListView;
        Spinner navigationStatus;
        ImageView navigationStatusImage;
        TextView timerHours;
        ToggleButton timer;
        RelativeLayout timerLayout;
        int lastposition;
        AssignmentViewModel assignmentViewModel;
        public event EventHandler<EventArgs<int>> NavigationSelected;
        NavigationItemSelectorListener navigationSelector;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            var tabActivity = ServiceContainer.Resolve<AssignmentTabActivity> ();
            assignmentViewModel = tabActivity.AssignmentViewModel;
            assignmentViewModel.HoursChanged += (sender, e) => {
                if (timerHours != null && Activity != null) {
                    Activity.RunOnUiThread (() => {
                        timerHours.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
                    });
                }
            };

            if (savedInstanceState != null && savedInstanceState.ContainsKey (Constants.BundleIndex)) {
                savedInstanceState.GetInt (Constants.BundleIndex);
            }

            navigationSelector = new NavigationItemSelectorListener (this);
        }

        public Assignment Assignment
        {
            get;
            set;
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
            var spinnerAdapter = new SpinnerAdapter<AssignmentStatus> (assignmentViewModel.AvailableStatuses, Activity, Resource.Layout.SimpleSpinnerItem);
            spinnerAdapter.TextColor = Color.White;
            navigationStatus.Adapter = spinnerAdapter;
            if (Assignment != null && !Assignment.IsHistory) {
                navigationStatus.ItemSelected += (sender, e) => {
                    var status = assignmentViewModel.AvailableStatuses [e.Position];
                    if (Assignment != null && Assignment.Status != status && Assignment.Status != AssignmentStatus.New) {
                        Assignment.Status = status;
                        switch (status) {
                            case AssignmentStatus.Complete:
                                //go to confirmations screen
                                var currentPosition = navigationListView.SelectedItemPosition;
                                var confirmationPosition = Constants.Navigation.IndexOf ("Confirmations");
                                if (currentPosition != confirmationPosition) {
                                    navigationSelector.OnItemClick (navigationListView, navigationListView.GetChildAt (confirmationPosition), confirmationPosition, 0);
                                }
                                break;
                            default:
                                SaveAssignment ();
                                break;
                        }
                    }
                };
            }
            timerLayout.Visibility = ViewStates.Gone;

            var adapter = new NavigationAdapter (Activity, Resource.Layout.NavigationListItemLayout, Constants.Navigation);
            if (Assignment != null && Assignment.IsHistory) {
                adapter = new NavigationAdapter (Activity, Resource.Layout.NavigationListItemLayout, Constants.HistoryNavigation);
            }
            navigationListView.OnItemClickListener = navigationSelector;
            navigationListView.Adapter = adapter;

            SetActiveAssignment ();

            return view;
        }
        
        /// <summary>
        /// Sets up the UI for the active assignment
        /// </summary>
        private void SetActiveAssignment ()
        {
            if (Assignment != null) {
                if (Assignment.IsHistory) {
                    navigationStatusImage.SetImageResource (Resource.Drawable.EnrouteImage);
                    navigationStatus.SetSelection (assignmentViewModel.AvailableStatuses.ToList ().IndexOf (AssignmentStatus.Complete));
                    timerLayout.Visibility = ViewStates.Gone;
                } else {
                    if (Assignment.Status == AssignmentStatus.Active || Assignment.Status == AssignmentStatus.Complete) {
                        timerLayout.Visibility = ViewStates.Visible;
                        navigationStatusImage.SetImageResource (Resource.Drawable.EnrouteImage);
                        navigationStatus.SetSelection (assignmentViewModel.AvailableStatuses.ToList ().IndexOf (AssignmentStatus.Active));
                        timerHours.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");

                        assignmentViewModel.LoadTimerEntryAsync ().ContinueOnUIThread (_ => {
                            if (assignmentViewModel.Recording) {
                                timer.Checked = true;
                            } else {
                                timer.Checked = false;
                            }
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
                    } else {
                        navigationStatusImage.SetImageResource (Resource.Drawable.HoldImage);
                        timerLayout.Visibility = ViewStates.Gone;
                    }
                }
            }
        }

        /// <summary>
        /// Saves the assignment
        /// </summary>
        private void SaveAssignment ()
        {
            assignmentViewModel.SaveAssignmentAsync (Assignment).ContinueOnUIThread (_ => {
                SetActiveAssignment ();
            });
        }

        /// <summary>
        /// Sets the selected navigation item
        /// </summary>
        public void SetNavigation (int index)
        {
            var currentPosition = navigationListView.SelectedItemPosition;
            if (currentPosition != index) {
                navigationSelector.OnItemClick (navigationListView, navigationListView.GetChildAt (index), index, 0);
            }
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
            Assignment = null;
            base.OnPause ();
        }

        private class NavigationItemSelectorListener : Java.Lang.Object, AdapterView.IOnItemClickListener {

            NavigationFragment fragment;
            public NavigationItemSelectorListener (NavigationFragment fragment)
            {
                this.fragment = fragment;
            }

            public void OnItemClick (AdapterView parent, View view, int position, long id)
            {
                if (position != fragment.lastposition) {

                    if (fragment.navigationListView.FirstVisiblePosition <= fragment.lastposition && fragment.lastposition <= fragment.navigationListView.LastVisiblePosition) {
                        var oldView = fragment.navigationListView.GetChildAt (fragment.lastposition);
                        if (view != null) {
                            fragment.navigationListView.Adapter.GetView (fragment.lastposition, oldView, fragment.navigationListView);
                        }
                    }
                    if (view != null) {
                        var image = view.FindViewById<ImageView> (Resource.Id.navigationListViewImage);
                        image.Visibility = ViewStates.Visible;
                    }
                    fragment.lastposition = position;
                    //need to switch fragments here
                    fragment.OnNavigationSelected (position);
                }
            }
        }
    }
}