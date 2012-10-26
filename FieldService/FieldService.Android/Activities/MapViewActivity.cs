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
using Android.GoogleMaps;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android {
    [Activity (Label = "Map View", Theme = "@style/CustomHoloTheme")]
    public class MapViewActivity : MapActivity, View.IOnClickListener {
        AssignmentViewModel assignmentViewModel;
        LinearLayout assignmentMapViewLayout;
        MapView mapView;
        MyLocationOverlay myLocation;
        TextView timerText;
        ToggleButton timer;

        public MapViewActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Create your application here
            SetContentView (Resource.Layout.MapViewLayout);

            assignmentMapViewLayout = FindViewById<LinearLayout> (Resource.Id.mapViewAssignmentLayout);
            mapView = FindViewById<MapView> (Resource.Id.googleMapsView);

            assignmentMapViewLayout.SetOnClickListener (this);
            myLocation = new MyLocationOverlay (this, mapView);
            myLocation.RunOnFirstFix (() => {
                mapView.Controller.AnimateTo (myLocation.MyLocation);
            });
            mapView.Overlays.Add (myLocation);
            mapView.Controller.SetZoom (5);
            mapView.Clickable = true;
            mapView.Enabled = true;
            mapView.SetBuiltInZoomControls (true);
        }

        protected override void OnResume ()
        {
            base.OnResume ();

            UpdateLocations ();
            myLocation.EnableMyLocation ();

            assignmentViewModel.HoursChanged += HoursChanged;

            if (assignmentViewModel.ActiveAssignment != null) {
                SetAssignment (true);
            } else {
                SetAssignment (false);
            }
        }

        private void UpdateLocations ()
        {
            assignmentViewModel.LoadAssignments ().ContinueOnUIThread (_ => {
                foreach (var item in assignmentViewModel.Assignments) {
                    var overlay = new OverlayItem (new GeoPoint (item.Latitude.ToIntE6 (), item.Longitude.ToIntE6 ()),
                        item.Title, string.Format ("{0} {1}, {2} {3}", item.Address, item.City, item.State, item.Zip));
                    Drawable drawable = null;
                    switch (item.Status) {
                        case AssignmentStatus.Hold:
                            drawable = Resources.GetDrawable (Resource.Drawable.AcceptedAssignmentIcon);
                            break;
                        default:
                            drawable = Resources.GetDrawable (Resource.Drawable.NewAssignmentIcon);
                            break;
                    }
                    mapView.Overlays.Add (new MapOverlayItem (this, drawable, overlay, mapView));
                }
                if (assignmentViewModel.ActiveAssignment != null) {
                    var activeOverlay = new OverlayItem (new GeoPoint (assignmentViewModel.ActiveAssignment.Latitude.ToIntE6 (), assignmentViewModel.ActiveAssignment.Longitude.ToIntE6 ()),
                        assignmentViewModel.ActiveAssignment.Title, string.Format ("{0} {1}, {2} {3}", assignmentViewModel.ActiveAssignment.Address,
                        assignmentViewModel.ActiveAssignment.City, assignmentViewModel.ActiveAssignment.State, assignmentViewModel.ActiveAssignment.Zip));
                    mapView.Overlays.Add (new MapOverlayItem (this, Resources.GetDrawable (Resource.Drawable.ActiveAssignmentIcon), activeOverlay, mapView));
                }
            });
        }

        protected override void OnPause ()
        {
            base.OnPause ();

            assignmentViewModel.HoursChanged -= HoursChanged;
        }

        protected override void OnStop ()
        {
            myLocation.DisableMyLocation ();
            mapView.Overlays.Clear ();
            assignmentMapViewLayout.RemoveAllViews ();
            base.OnStop ();
        }

        private void HoursChanged (object sender, EventArgs e)
        {
            if (timerText != null) {
                RunOnUiThread (() => {
                    timerText.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
                });
            }
        }
        
        protected override bool IsRouteDisplayed
        {
            get { return false; }
        }

        private void SetAssignment (bool visible)
        {
            if (visible) {
                assignmentMapViewLayout.Visibility = ViewStates.Visible;
                var assignment = assignmentViewModel.ActiveAssignment;
                View view = null;
                //look at layouts children to get view
                if (assignmentMapViewLayout.ChildCount > 0) {
                    view = assignmentMapViewLayout.GetChildAt (0);
                } else {
                    view = new View (this);
                    LayoutInflater inflator = (LayoutInflater)GetSystemService (Context.LayoutInflaterService);
                    view = inflator.Inflate (Resource.Layout.AssignmentItemLayout, null);
                    assignmentMapViewLayout.AddView (view);
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
                    timer.Enabled = false;
                    if (assignmentViewModel.Recording) {
                        assignmentViewModel.Pause ().ContinueOnUIThread (t => timer.Enabled = true);
                    } else {
                        assignmentViewModel.Record ().ContinueOnUIThread (t => timer.Enabled = true);
                    }
                };

                buttonLayout.Visibility = ViewStates.Gone;
                timerLayout.Visibility = ViewStates.Visible;

                var adapter = new SpinnerAdapter (assignmentViewModel.AvailableStatuses, this, Resource.Layout.SimpleSpinnerItem);
                adapter.TextColor = Resources.GetColor (Resource.Color.greyspinnertext);
                spinner.Adapter = adapter;
                spinner.SetSelection (assignmentViewModel.AvailableStatuses.ToList ().IndexOf (assignment.Status));
                spinner.SetBackgroundResource (Resource.Drawable.spinner_blue);
                spinnerImage.SetImageResource (Resource.Drawable.EnrouteImage);

                spinner.ItemSelected += (sender, e) => {
                    var selected = assignmentViewModel.AvailableStatuses.ElementAtOrDefault (e.Position);
                    if (selected != assignment.Status) {
                        switch (selected) {
                            case AssignmentStatus.Active:
                                break;
                            default:
                                assignment.Status = selected;
                                assignmentViewModel.SaveAssignment (assignment).ContinueOnUIThread (_ => {
                                    SetAssignment (false);
                                    mapView.Overlays.Clear ();
                                    mapView.Overlays.Add (myLocation);
                                    UpdateLocations ();
                                });
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
                assignmentMapViewLayout.Visibility = ViewStates.Gone;
            }
        }

        public void OnClick (View v)
        {
            if (v.Id == Resource.Id.mapViewAssignmentLayout) {
                var intent = new Intent (this, typeof (SummaryActivity));
                intent.PutExtra (Constants.BundleIndex, -1);
                StartActivity (intent);
            }
        }
    }
}