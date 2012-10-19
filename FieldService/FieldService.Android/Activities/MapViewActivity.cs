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
//    limitations under the License.using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.GoogleMaps;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.Android.Utilities;
using FieldService.ViewModels;
using Android.Hardware;
using Android.Graphics.Drawables;
using System;

namespace FieldService.Android {
    [Activity (Label = "Map View", Theme = "@style/CustomHoloTheme")]
    public class MapViewActivity : MapActivity {
        AssignmentViewModel assignmentViewModel;
        LinearLayout assignmentMapViewLayout;
        MapView mapView;
        MyLocationOverlay myLocation;

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

            foreach (var item in assignmentViewModel.Assignments) {
                var overlay = new OverlayItem (new GeoPoint (item.Latitude.ToIntE6 (), item.Longitude.ToIntE6 ()),
                    item.Title, string.Format ("{0} {1}, {2} {3}", item.Address, item.City, item.State, item.Zip));
                Drawable drawable = null;
                switch (item.Status) {
                    case AssignmentStatus.Hold:
                        drawable = Resources.GetDrawable (Resource.Drawable.AcceptedAssignmentIcon);
                        break;
                    case AssignmentStatus.Active:
                        drawable = Resources.GetDrawable (Resource.Drawable.ActiveAssignmentIcon);
                        break;
                    default:
                        drawable = Resources.GetDrawable (Resource.Drawable.NewAssignmentIcon);
                        break;
                }
                mapView.Overlays.Add (new MapOverlayItem (this, drawable, overlay));
            }
            myLocation.EnableMyLocation ();
            if (assignmentViewModel.ActiveAssignment != null) {
                SetAssignment (true);
            } else {
                SetAssignment (false);
            }
        }

        protected override void OnStop ()
        {
            myLocation.DisableMyLocation ();
            mapView.Overlays.Clear ();
            assignmentMapViewLayout.RemoveAllViews ();
            base.OnStop ();
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

                spinner.Adapter = new SpinnerAdapter(Assignment.AvailableStatuses, this);
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
                                    SetAssignment (false);
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
                timerText.Text = string.Format ("{0} hr {1} min\n{2}", "10", "59", "START");
            } else {
                assignmentMapViewLayout.Visibility = ViewStates.Gone;
            }
        }
    }
}