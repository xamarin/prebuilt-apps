using System;
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
        public event EventHandler<EventArgs<int>> NavigationSelected = delegate { };

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            if (savedInstanceState != null && savedInstanceState.ContainsKey (Constants.BUNDLE_INDEX)) {
                listViewIndex = savedInstanceState.GetInt (Constants.BUNDLE_INDEX);
            }
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);
            var view = inflater.Inflate (Resource.Layout.NavigationLayout, container, true);
            navigationListView = view.FindViewById<ListView> (Resource.Id.navigationListView);
            navigationStatus = view.FindViewById<Spinner> (Resource.Id.fragmentStatus);
            navigationStatusImage = view.FindViewById<ImageView> (Resource.Id.fragmentStatusImage);
            timerLayout = view.FindViewById<RelativeLayout> (Resource.Id.fragmentTimerTextLayout);
            timer = view.FindViewById<ToggleButton> (Resource.Id.fragmentTimer);
            timerHours = view.FindViewById<TextView> (Resource.Id.fragmentHours);

            navigationStatusImage.SetImageResource (Resource.Drawable.HoldImage);
            var spinnerAdapter = new SpinnerAdapter (Assignment.AvailableStatuses, Activity, Resource.Layout.SimpleSpinnerItem);
            spinnerAdapter.TextColor = Color.White;
            navigationStatus.Adapter = spinnerAdapter;
            navigationStatus.OnItemSelectedListener = this;
            timerLayout.Visibility = ViewStates.Gone;

            var adapter = new NavigationAdapter (this.Activity, Resource.Layout.NavigationListItemLayout, Constants.Navigation);
            navigationListView.OnItemClickListener = this;
            navigationListView.Adapter = adapter;
            return view;
        }

        private void SaveAssignment ()
        {
            assignmentViewModel.SaveAssignment (assignment).ContinueOnUIThread (_ => {
                Assignment = assignment;
                });
        }

        public Assignment Assignment
        {
            get { return assignment; }
            set
            {
                if (value != null && value.Status == AssignmentStatus.Active) {
                    timerLayout.Visibility = ViewStates.Visible;
                    navigationStatusImage.SetImageResource (Resource.Drawable.EnrouteImage);
                    navigationStatus.SetSelection (Assignment.AvailableStatuses.ToList ().IndexOf (AssignmentStatus.Active));
                    timerHours.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");

                    assignmentViewModel.LoadTimerEntry ().ContinueOnUIThread (_ => {
                        if (assignmentViewModel.Recording) {
                            timer.Checked = true;
                        } else {
                            timer.Checked = false;
                        }
                    });

                    timer.CheckedChange += (sender, e) => {
                        timer.Enabled = false;
                        if (assignmentViewModel.Recording) {
                            assignmentViewModel.Pause ().ContinueOnUIThread (t => timer.Enabled = true);
                        } else {
                            assignmentViewModel.Record ().ContinueOnUIThread (t => timer.Enabled = true);
                        }
                    };
                } else {
                    navigationStatusImage.SetImageResource (Resource.Drawable.HoldImage);
                    timerLayout.Visibility = ViewStates.Gone;
                }
                assignment = value;
            }
        }

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

        public void OnItemSelected (AdapterView parent, View view, int position, long id)
        {
            var status = Assignment.AvailableStatuses[position];
            if (Assignment.Status != status) {
                assignment.Status = status;
                SaveAssignment ();
            }
        }

        public void OnNothingSelected (AdapterView parent)
        {
            //do nothing
        }

        public void SetNavigation (int index)
        {
            navigationListView.SetSelection (index);
            OnItemClick (navigationListView, navigationListView.GetChildAt (index), index, 0);
        }

        private void OnNavigationSelected (int index)
        {
            NavigationSelected (this, new EventArgs<int> { Value = index });
        }

        public override void OnSaveInstanceState (Bundle outState)
        {
            base.OnSaveInstanceState (outState);
            outState.PutInt (Constants.BUNDLE_INDEX, lastposition);
        }

        public override void OnResume ()
        {
            var navItemView = navigationListView.GetChildAt (listViewIndex);
            if (navItemView != null) {
                OnItemClick (navigationListView, navItemView, listViewIndex, 0);
            }
            assignmentViewModel.HoursChanged += HoursChanged;
            base.OnResume ();
        }

        public override void OnPause ()
        {
            assignmentViewModel.HoursChanged -= HoursChanged;
            Assignment = null;
            base.OnPause ();
        }

        private void HoursChanged (object sender, System.EventArgs e)
        {
            if (timerHours != null) {
                Activity.RunOnUiThread (() => {
                    timerHours.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
                    });
            }
        }
    }
}