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
using Android.Content.PM;
using Android.GoogleMaps;
using Android.Graphics;
using Android.Graphics.Drawables;
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
    /// Activity for the map overview
    /// </summary>
    [Activity (Label = "Map View", Theme = "@style/CustomHoloTheme")]
    public class MapViewActivity : BaseMapActivity {
        readonly AssignmentViewModel assignmentViewModel;
        readonly MenuViewModel menuViewModel;
        MapView mapView;
        MyLocationOverlay myLocation;
        LinearLayout assignmentMapViewLayout,
            buttonLayout,
            timerLayout;
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
        AssignmentTabActivity.MapDataWrapper mapData;
        RelativeLayout phoneButton,
            mapButton;

        public MapViewActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            assignmentViewModel.HoursChanged += HoursChanged;
            menuViewModel = ServiceContainer.Resolve<MenuViewModel> ();
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Create your application here
            SetContentView (Resource.Layout.MapViewLayout);

            assignmentMapViewLayout = FindViewById<LinearLayout> (Resource.Id.mapViewAssignmentLayout);
            assignmentMapViewLayout.Click += (sender, e) => {
                var intent = new Intent (this, typeof (SummaryActivity));
                var tabActivity = (AssignmentTabActivity)Parent;
                tabActivity.MapData = null;
                assignmentViewModel.SelectedAssignment = assignmentViewModel.ActiveAssignment;
                menuViewModel.MenuIndex = Constants.Navigation.IndexOf ("Map");
                StartActivity (intent);
            };
            mapView = FindViewById<MapView> (Resource.Id.googleMapsView);

            myLocation = new MyLocationOverlay (this, mapView);

            mapView.Overlays.Add (myLocation);
            mapView.Clickable = true;
            mapView.Enabled = true;
            mapView.SetBuiltInZoomControls (true);


            //View containing the active assignment
            var view = new View (this);
            LayoutInflater inflator = (LayoutInflater)GetSystemService (Context.LayoutInflaterService);
            view = inflator.Inflate (Resource.Layout.AssignmentItemLayout, null);
            assignmentMapViewLayout.AddView (view);
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

            phoneButton.Click += (sender, e) => {
                Extensions.MakePhoneCall (this, phone.Text);
            };

            mapButton.Click += (sender, e) => {
                var intent = new Intent (this, typeof (SummaryActivity));
                var tabActivity =(AssignmentTabActivity)Parent;
                tabActivity.MapData = null;
                assignmentViewModel.SelectedAssignment = assignmentViewModel.ActiveAssignment;
                menuViewModel.MenuIndex = 0;
                StartActivity (intent);
            };

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
                                assignmentViewModel.SaveAssignmentAsync (assignment).ContinueWith (_ => {
                                    RunOnUiThread (() => {
                                        SetAssignment (false);
                                        mapView.Overlays.Clear ();
                                        mapView.Overlays.Add (myLocation);
                                        UpdateLocations ();
                                    });
                                });
                                break;
                            case AssignmentStatus.Complete:
                                //go to confirmations
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
        }

        /// <summary>
        /// Enabling my location and setting active assignment
        /// </summary>
        protected override void OnResume ()
        {
            base.OnResume ();

            UpdateLocations ();
            myLocation.EnableMyLocation ();
            var tabActivity = (AssignmentTabActivity)Parent; 
            AssignmentTabActivity.MapDataWrapper mapDataWrapper = null;
            if (tabActivity != null) {
                mapDataWrapper = tabActivity.MapData;
            }

            if (mapDataWrapper != null) {
                mapView.Controller.SetZoom (mapDataWrapper.Zoom);
                mapView.Controller.AnimateTo (mapDataWrapper.OverlayPoint);
                mapView.AddView (mapDataWrapper.OverlayBubble);
            } else {
                myLocation.RunOnFirstFix (() => {
                    mapView.Controller.AnimateTo (myLocation.MyLocation);
                });
                mapView.Controller.SetZoom (5);
            }

            if (assignmentViewModel.ActiveAssignment != null) {
                SetAssignment (true);
            } else {
                SetAssignment (false);
            }

            var dialog = new AlertDialog.Builder (this)
                .SetTitle ("Google Maps")
                .SetMessage (Resources.GetString (Resource.String.GoogleMapsKeyIssue))
                .SetPositiveButton ("Ok", (sender, e) => { })
                .Show ();
        }

        /// <summary>
        /// Updates location pins on the map.
        /// </summary>
        private void UpdateLocations ()
        {
            assignmentViewModel.LoadAssignmentsAsync ().ContinueWith (_ => {
                RunOnUiThread (() => {
                    int i = 0;
                    foreach (var item in assignmentViewModel.Assignments) {
                        var overlay = new OverlayItem (new GeoPoint (item.Latitude.ToIntE6 (), item.Longitude.ToIntE6 ()),
                            item.CompanyName, string.Format ("{0} {1}, {2} {3}", item.Address, item.City, item.State, item.Zip));
                        Drawable drawable = null;
                        switch (item.Status) {
                            case AssignmentStatus.Hold:
                                drawable = Resources.GetDrawable (Resource.Drawable.AcceptedAssignmentIcon);
                                break;
                            default:
                                drawable = Resources.GetDrawable (Resource.Drawable.NewAssignmentIcon);
                                break;
                        }
                        var mapoverlay = new MapOverlayItem (this, drawable, overlay, mapView);
                        mapoverlay.AssignmentIndex = i;
                        mapView.Overlays.Add (mapoverlay);
                        i++;
                    }
                    if (assignmentViewModel.ActiveAssignment != null) {
                        var activeOverlay = new OverlayItem (new GeoPoint (assignmentViewModel.ActiveAssignment.Latitude.ToIntE6 (), assignmentViewModel.ActiveAssignment.Longitude.ToIntE6 ()),
                            assignmentViewModel.ActiveAssignment.CompanyName, string.Format ("{0} {1}, {2} {3}", assignmentViewModel.ActiveAssignment.Address,
                            assignmentViewModel.ActiveAssignment.City, assignmentViewModel.ActiveAssignment.State, assignmentViewModel.ActiveAssignment.Zip));
                        var mapoverlay = new MapOverlayItem (this, Resources.GetDrawable (Resource.Drawable.ActiveAssignmentIcon), activeOverlay, mapView);
                        mapoverlay.AssignmentIndex = -1;
                        mapView.Overlays.Add (mapoverlay);
                    }
                });
            });
        }
        
        protected override void OnSaveInstanceState (Bundle outState)
        {
            var tabActivity = (AssignmentTabActivity)Parent;
            if (mapView.ChildCount > 0) {
                if (mapData == null) {
                    mapData = new AssignmentTabActivity.MapDataWrapper ();
                }
                mapData.OverlayBubble = mapView.GetChildAt (0);
                mapData.Zoom = 10;
                mapData.OverlayPoint = ((MapView.LayoutParams)mapData.OverlayBubble.LayoutParameters).Point;
            }
            if (tabActivity != null) {
                tabActivity.MapData = mapData;
            }
            base.OnSaveInstanceState (outState);
        }

        /// <summary>
        /// Clearing overlays on map, stopping my location, clearing active assignment in the layout.
        /// </summary>
        protected override void OnStop ()
        {
            myLocation.DisableMyLocation ();
            mapView.Overlays.Clear ();
            base.OnStop ();
        }

        /// <summary>
        /// Clearing overlay bubble when you leave the screen.
        /// </summary>
        protected override void OnPause ()
        {
            if (mapView.ChildCount > 0) {
                mapView.RemoveViewAt (0);
            }
            base.OnPause ();
        }

        /// <summary>
        /// Hours Changed event for tracking time on active assignment
        /// </summary>
        private void HoursChanged (object sender, EventArgs e)
        {
            if (timerText != null) {
                RunOnUiThread (() => {
                    timerText.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
                });
            }
        }

        /// <summary>
        /// Sets the current active assignment
        /// </summary>
        /// <param name="visible"></param>
        private void SetAssignment (bool visible)
        {
            if (visible) {
                assignmentMapViewLayout.Visibility = ViewStates.Visible;
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
                assignmentMapViewLayout.Visibility = ViewStates.Gone;
            }
        }
    }
}