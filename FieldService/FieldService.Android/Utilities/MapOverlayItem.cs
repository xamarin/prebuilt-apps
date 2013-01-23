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

using Android.Content;
using Android.GoogleMaps;
using Android.Graphics.Drawables;
using Android.Net;
using Android.Views;
using Android.Widget;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Utilities {
    /// <summary>
    /// Class for creating pins in the Google Maps control, also creates a popup bubble when clicked
    /// </summary>
    public class MapOverlayItem : ItemizedOverlay {
        readonly AssignmentViewModel assignmentViewModel;
        OverlayItem item;
        Context context;
        MapView mapView;
        bool getDirections;
        string intentURI = "http://maps.google.com/maps?saddr={0}&daddr={1}";

        public MapOverlayItem (Context context, Drawable overlayDrawable, OverlayItem overlay, MapView mapView, bool canGetDirection = false)
            : base (overlayDrawable)
        {
            item = overlay;
            this.context = context;
            this.mapView = mapView;
            getDirections = canGetDirection;

            BoundCenterBottom (overlayDrawable);
            Populate ();

            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        protected override Java.Lang.Object CreateItem (int i)
        {
            return item;
        }

        public override int Size ()
        {
            return 1;
        }

        public void AddressSelected ()
        {
            OnTap (0);
        }

        public GeoPoint Point
        {
            get { return item.Point; }
        }

        public int AssignmentIndex
        {
            get;
            set;
        }

        /// <summary>
        /// When tapped, show a popup bubble
        /// </summary>
        protected override bool OnTap (int index)
        {
            if (mapView != null) {
                if (mapView.ChildCount > 0) {
                    mapView.RemoveViewAt (0);
                }
                View bubbleView = null;
                LayoutInflater inflator = (LayoutInflater)context.GetSystemService (Context.LayoutInflaterService);
                bubbleView = inflator.Inflate (Resource.Layout.MapOverlayLayout, null);
                bubbleView.LayoutParameters = new MapView.LayoutParams (MapView.LayoutParams.WrapContent, MapView.LayoutParams.WrapContent, item.Point, -2, -25, MapView.LayoutParams.BottomCenter);
                var button = bubbleView.FindViewById<ImageButton> (Resource.Id.mapOverlayGetDirections);
                var address = bubbleView.FindViewById<TextView> (Resource.Id.mapOverlayAddress);
                var image = bubbleView.FindViewById<ImageView> (Resource.Id.mapOverlayDivider);
                var overlayLayout = bubbleView.FindViewById<LinearLayout> (Resource.Id.overlayDirectionsLayout);
                address.Text = item.Snippet;

                image.Visibility = getDirections ? ViewStates.Visible : ViewStates.Gone;
                button.Visibility = getDirections ? ViewStates.Visible : ViewStates.Gone;
                if (getDirections) {
                    button.Click += (sender, e) => {
                        var intent = new Intent(Intent.ActionView, 
                            Uri.Parse(string.Format(intentURI, string.Empty, item.Snippet)));
                        context.StartActivity(intent);
                    };
                }

                if (!getDirections) {
                    overlayLayout.Click += (sender, e) => {
                        var intent = new Intent (context, typeof (SummaryActivity));
                        assignmentViewModel.SelectedAssignment = assignmentViewModel.Assignments [AssignmentIndex];
                        context.StartActivity (intent);
                    };
                }

                mapView.AddView (bubbleView);
                mapView.Controller.AnimateTo (item.Point);
                mapView.Controller.SetZoom (10);
            }
            return true;
        }
    }
}