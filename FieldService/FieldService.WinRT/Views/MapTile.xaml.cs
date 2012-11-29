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
using FieldService.Data;
using FieldService.WinRT.Utilities;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace FieldService.WinRT.Views {
    public sealed partial class MapTile : UserControl {
        readonly Pushpin pin;

        public MapTile ()
        {
            this.InitializeComponent ();

            map.Credentials = Constants.BingMapsKey;
            pin = new Pushpin ();
            pin.Tapped += OnPinTapped;
            map.Children.Add (pin);
            map.ShowNavigationBar = false;
            map.ShowScaleBar = false;
        }

        private void OnLoaded (object sender, RoutedEventArgs e)
        {
            var assignment = DataContext as Assignment;
            if (assignment != null) {
                pin.Visibility = Visibility.Visible;
                pin.Background = assignment.Status.GetBrushForStatus ();

                var location = new Location (assignment.Latitude, assignment.Longitude);
                MapLayer.SetPosition (pin, location);
                map.SetView (location, 10);
            } else {
                pin.Visibility = Visibility.Collapsed;
            }
        }

        private void OnPinTapped (object sender, TappedRoutedEventArgs e)
        {
            Helpers.NavigateTo<AssignmentMapPage>();
        }
    }
}
