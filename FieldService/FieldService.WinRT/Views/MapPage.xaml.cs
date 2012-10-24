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
using Bing.Maps;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.ViewModels;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace FieldService.WinRT.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page {
        readonly AssignmentViewModel assignmentViewModel;
        Geolocator locator;
        Pushpin userPin;

        public MapPage ()
        {
            this.InitializeComponent ();

            locator = new Geolocator ();

            userPin = new Pushpin ();

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
            //Clear the map controls
            map.Children.Clear ();

            //Add pins for the assignments
            foreach (var assignment in assignmentViewModel.Assignments) {
                var pin = new Pushpin {
                    Background = assignment.Status.GetBrushForStatus(),
                };
                map.Children.Add (pin);
                MapLayer.SetPosition (pin, new Location (assignment.Latitude, assignment.Longitude));
            }

            UpdatePosition ();
        }

        private async void UpdatePosition ()
        {
            var position = await locator.GetGeopositionAsync ();
            var location = new Location (position.Coordinate.Latitude, position.Coordinate.Longitude);
            
            //Move the map
            map.SetView (location, 6);

            //Move the user's pin
            map.Children.Add (userPin);
            MapLayer.SetPosition (userPin, location);
        }
    }
}
