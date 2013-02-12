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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.GoogleMaps;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using Java.Util;

namespace FieldService.Android {
    /// <summary>
    /// "Fragment" for the map - it is required to be an activity although used as a fragment
    /// </summary>
    [Activity (Label = "Map View Fragment", Theme = "@style/CustomHoloTheme")]
    public class MapFragmentActivity : BaseMapActivity {

        readonly AssignmentViewModel assignmentViewModel;
        MapView mapView;
        MyLocationOverlay myLocation;
        Assignment assignment;

        public MapFragmentActivity ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            assignment = assignmentViewModel.SelectedAssignment;
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.MapFragmentLayout);

            mapView = FindViewById<MapView> (Resource.Id.fragmentMapView);
            myLocation = new MyLocationOverlay (this, mapView);
            myLocation.RunOnFirstFix (() => {
                //mapView.Controller.AnimateTo (myLocation.MyLocation);

                var maxLat = Math.Max (myLocation.MyLocation.LatitudeE6, assignment.Latitude.ToIntE6 ());
                var minLat = Math.Min (myLocation.MyLocation.LatitudeE6, assignment.Latitude.ToIntE6 ());
                var maxLong = Math.Max (myLocation.MyLocation.LongitudeE6, assignment.Longitude.ToIntE6 ());
                var minLong = Math.Min (myLocation.MyLocation.LongitudeE6, assignment.Longitude.ToIntE6 ());

                mapView.Controller.ZoomToSpan (Math.Abs (maxLat - minLat), Math.Abs (maxLong - minLong));

                mapView.Controller.AnimateTo (new GeoPoint ((maxLat + minLat) / 2, (maxLong + minLong) / 2));
            });

            mapView.Overlays.Add (myLocation);
            mapView.Controller.SetZoom (5);
            mapView.Clickable = true;
            mapView.Enabled = true;
            mapView.SetBuiltInZoomControls (true);
        }

        /// <summary>
        /// Enable location tracking and place any overlays
        /// </summary>
        protected override void OnResume ()
        {
            base.OnResume ();
            myLocation.EnableMyLocation ();
            if (assignment != null) {
                var overlayItem = new OverlayItem (new GeoPoint (assignment.Latitude.ToIntE6 (), assignment.Longitude.ToIntE6 ()), assignment.CompanyName,
                    string.Format ("{0} {1}, {2} {3}", assignment.Address, assignment.City, assignment.State, assignment.Zip));
                MapOverlayItem overlay = new MapOverlayItem (this, Resources.GetDrawable (Resource.Drawable.ActiveAssignmentIcon), overlayItem, mapView, true);
                mapView.Overlays.Add (overlay);
            }

            var dialog = new AlertDialog.Builder (this)
                .SetTitle ("Google Maps")
                .SetMessage (Resources.GetString (Resource.String.GoogleMapsKeyIssue))
                .SetPositiveButton ("Ok", (sender, e) => { })
                .Show ();
        }

        /// <summary>
        /// Clear overlays and disable location tracking
        /// </summary>
        protected override void OnPause ()
        {
            myLocation.DisableMyLocation ();
            mapView.Overlays.Clear ();
            base.OnPause ();
        }
    }
}