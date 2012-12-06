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

using Bing.Maps;
using FieldService.Utilities;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.ViewModels;
using System;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace FieldService.WinRT.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssignmentMapPage : Page {
        readonly AssignmentViewModel assignmentViewModel;
        readonly Geolocator locator;
        readonly Pushpin userPin, assignmentPin;
        readonly MapPopup popup;

        public AssignmentMapPage ()
        {
            this.InitializeComponent ();
            map.Credentials = Constants.BingMapsKey;

            locator = new Geolocator ();
            popup = new MapPopup ();

            userPin = new Pushpin ();
            userPin.Tapped += OnPinTapped;
            map.Children.Add (userPin);

            assignmentPin = new Pushpin ();
            assignmentPin.Tapped += OnPinTapped;
            map.Children.Add (assignmentPin);

            DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
            var assignment = assignmentViewModel.SelectedAssignment;
            var location = new Location (assignment.Latitude, assignment.Longitude);

            //Set the assignment pin's location
            MapLayer.SetPosition (assignmentPin, location);
            //Move the map
            map.SetView (location, 6);
            //Set color
            assignmentPin.Background = assignment.Status.GetBrushForStatus ();

            UpdatePosition ();
        }

        private void OnPinTapped (object sender, TappedRoutedEventArgs e)
        {
            var pin = sender as Pushpin;
            if (userPin == sender) {
                popup.DataContext = new {
                    Title = "Current Location",
                    AddressFormatted = string.Empty,
                };
            } else {
                popup.DataContext = assignmentViewModel.SelectedAssignment;
            }

            if (!map.Children.Contains (popup))
                map.Children.Add (popup);

            //Set position
            var location = MapLayer.GetPosition (pin);
            MapLayer.SetPosition (popup, location);
            map.SetView (location, TimeSpan.FromSeconds (.3));
        }

        private async void UpdatePosition ()
        {
            try {
                var assignment = assignmentViewModel.SelectedAssignment;
                var position = await locator.GetGeopositionAsync ();

                var location = new Location (position.Coordinate.Latitude, position.Coordinate.Longitude);

                //Set the user's pin
                const double spacing = 3;
                MapLayer.SetPosition (userPin, location);
                var northWest = new Location(Math.Max(assignment.Latitude, location.Latitude), Math.Min(assignment.Longitude, location.Longitude));
                northWest.Longitude -= spacing;
                northWest.Latitude += spacing;
                var southEast = new Location(Math.Min(assignment.Latitude, location.Latitude), Math.Max(assignment.Longitude, location.Longitude));
                southEast.Longitude += spacing;
                southEast.Latitude -= spacing;
                map.SetView (new LocationRect (northWest, southEast));
            } catch (Exception exc) {
                System.Diagnostics.Debug.WriteLine ("Error updating position: " + exc.Message);
            }
        }
    }
}