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
using FieldService.Data;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FieldService.WinRT.Views {
    public sealed partial class MapPopup : UserControl {
        public MapPopup ()
        {
            this.InitializeComponent ();
        }

        private async void OnDirectionClick (object sender, RoutedEventArgs e)
        {
            var assignment = DataContext as Assignment;
            if (assignment != null)
                await Launcher.LaunchUriAsync (new Uri (string.Format("bingmaps:?cp={0}~{1}&where={2} {3}, {4} {5}", 
                    assignment.Latitude, assignment.Longitude, assignment.Address, assignment.City, assignment.State, assignment.Zip)));
            else
                await Launcher.LaunchUriAsync (new Uri ("bingmaps:"));
        }
    }
}
