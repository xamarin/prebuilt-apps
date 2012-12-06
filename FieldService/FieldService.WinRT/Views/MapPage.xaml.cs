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
    public sealed partial class MapPage : Page {
        readonly AssignmentViewModel assignmentViewModel;
        readonly Geolocator locator;
        readonly Pushpin userPin;
        readonly MapPopup popup;

        public MapPage ()
        {
            this.InitializeComponent ();
            map.Credentials = Constants.BingMapsKey;

            locator = new Geolocator ();
            popup = new MapPopup ();

            userPin = new Pushpin ();
            userPin.Tapped += OnPinTapped;

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
            foreach (var pin in map.Children) {
                if (pin != userPin) {
                    pin.Tapped -= OnPinTapped;
                }
            }

            //Clear the map controls
            map.Children.Clear ();

            //Add pins for the assignments
            if (assignmentViewModel.Assignments != null) {
                foreach (var assignment in assignmentViewModel.Assignments) {
                    var pin = new Pushpin {
                        Background = assignment.Status.GetBrushForStatus (),
                        Tag = assignment,
                    };
                    pin.Tapped += OnPinTapped;
                    map.Children.Add (pin);
                    MapLayer.SetPosition (pin, new Location (assignment.Latitude, assignment.Longitude));
                }
            }

            if (assignmentViewModel.ActiveAssignment != null) {
                var pin = new Pushpin {
                    Background = assignmentViewModel.ActiveAssignment.Status.GetBrushForStatus (),
                    Tag = assignmentViewModel.ActiveAssignment,
                };
                pin.Tapped += OnPinTapped;
                map.Children.Add (pin);
                MapLayer.SetPosition (pin, new Location (assignmentViewModel.ActiveAssignment.Latitude, assignmentViewModel.ActiveAssignment.Longitude));
            }

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
                popup.DataContext = pin.Tag;
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
                var position = await locator.GetGeopositionAsync ();
                var location = new Location (position.Coordinate.Latitude, position.Coordinate.Longitude);

                //Move the map
                map.SetView (location, 6);

                //Move the user's pin
                map.Children.Add (userPin);
                MapLayer.SetPosition (userPin, location);

            } catch (Exception exc) {
                System.Diagnostics.Debug.WriteLine ("Error updating position: " + exc.Message);
            }
        }
    }
}