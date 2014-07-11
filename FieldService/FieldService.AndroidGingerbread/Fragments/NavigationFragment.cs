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
        LinearLayout navigationStatusLayout;
        RelativeLayout timerLayout;
        int lastposition = -1;
        AssignmentViewModel assignmentViewModel;
        MenuViewModel menuViewModel;
        public event EventHandler<EventArgs<int>> NavigationSelected;
        NavigationItemSelectorListener navigationSelector;

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            menuViewModel = ServiceContainer.Resolve<MenuViewModel> ();

            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            assignmentViewModel.HoursChanged += (sender, e) => {
                if (timerHours != null && Activity != null) {
                    Activity.RunOnUiThread (() => {
                        timerHours.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
                    });
                }
            };

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
            navigationStatusLayout = view.FindViewById<LinearLayout> (Resource.Id.navigationStatusLayout);
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
                        switch (status) {
                            case AssignmentStatus.Complete:
                                //go to confirmations screen
                                var currentPosition = navigationListView.SelectedItemPosition;
                                var confirmationPosition = Constants.Navigation.IndexOf ("Confirmations");
                                if (currentPosition != confirmationPosition) {
                                    navigationSelector.OnItemClick (navigationListView, navigationListView.GetChildAt (confirmationPosition), confirmationPosition, 0);
                                }
                                navigationStatus.SetSelection (assignmentViewModel.AvailableStatuses.ToList ().IndexOf (Assignment.Status));
                                break;
                            default:
                                Assignment.Status = status;
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

            timer.CheckedChange += (sender, e) => {
                if (e.IsChecked != assignmentViewModel.Recording) {
                    if (assignmentViewModel.Recording) {
                        assignmentViewModel.PauseAsync ();
                    } else {
                        assignmentViewModel.RecordAsync ();
                    }
                }
            };

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
                    navigationStatus.Enabled =false;
                    timerLayout.Visibility = ViewStates.Gone;
                } else {
                    switch (Assignment.Status) {
                        case AssignmentStatus.New:
                            navigationStatusLayout.Visibility =
                                timerLayout.Visibility = ViewStates.Gone;
                            break;
                        case AssignmentStatus.Hold:
                            navigationStatusImage.SetImageResource (Resource.Drawable.HoldImage);
                            navigationStatusLayout.Visibility = ViewStates.Visible;
                            timerLayout.Visibility = ViewStates.Gone;
                            break;
                        case AssignmentStatus.Active:
                        case AssignmentStatus.Complete:
                            navigationStatusLayout.Visibility =
                                timerLayout.Visibility = ViewStates.Visible;
                            navigationStatusImage.SetImageResource (Resource.Drawable.EnrouteImage);
                            navigationStatus.SetSelection (assignmentViewModel.AvailableStatuses.ToList ().IndexOf (AssignmentStatus.Active));
                            timerHours.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");

                            Activity.RunOnUiThread (() => {
                                if (assignmentViewModel.Recording) {
                                    timer.Checked = true;
                                } else {
                                    timer.Checked = false;
                                }
                            });
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Saves the assignment
        /// </summary>
        private void SaveAssignment ()
        {
            assignmentViewModel.SaveAssignmentAsync (Assignment).ContinueWith (_ => Activity.RunOnUiThread (SetActiveAssignment));
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
        /// Clear out the assignment
        /// </summary>
        public override void OnPause ()
        {
            Assignment = null;
            base.OnPause ();
        }

        public override void OnResume ()
        {
            base.OnResume ();
            var index = menuViewModel.MenuIndex;
            //RunOnUIThread doesn't allow the listview to be inflated with the views when it is called.  So when we try to get the view from the listview to display
            //the arrow indicating which position we are at in the list the view returned is null. Post from the listview allows the views inside the listview to be inflated
            //and we are able to then access all views inside the listview.  the caveat here is some people have stated that post does not work on all android devices.
            navigationListView.Post (() => {
                navigationSelector.OnItemClick (navigationListView, navigationListView.GetChildAt (index), index, 0);
            });
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
                    //need to switch fragments here unless the fragment is being created for instance on a device rotation.
                    if (fragment.lastposition != -1) {
                        fragment.OnNavigationSelected (position);
                    }
                    fragment.lastposition = position;
                }
            }
        }
    }
}