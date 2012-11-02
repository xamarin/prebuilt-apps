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
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Utilities;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using Windows.Storage.Streams;

namespace FieldService.WinRT.Utilities {
    /// <summary>
    /// Set of helper methods for WinRT
    /// </summary>
    public static class Helpers {
        /// <summary>
        /// Helper method to navigate to a particular page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void NavigateTo<T> (object parameter = null)
            where T : Page
        {
            var app = ServiceContainer.Resolve<App> ();

            if (!app.RootFrame.Navigate (typeof (T), parameter)) {
                throw new InvalidOperationException ("Failed to navigation to: " + typeof (T));
            }
        }

        public static bool CanGoBack
        {
            get
            {
                var app = ServiceContainer.Resolve<App> ();
                return app.RootFrame.CanGoBack;
            }
        }

        public static void GoBack ()
        {
            var app = ServiceContainer.Resolve<App> ();
            app.RootFrame.GoBack ();
        }

        public static SolidColorBrush GetBrushForStatus (this AssignmentStatus status)
        {
            var app = ServiceContainer.Resolve<App> ();

            switch (status) {
                case AssignmentStatus.Hold:
                    return app.Resources ["YellowBrush"] as SolidColorBrush;
                case AssignmentStatus.Declined:
                    return app.Resources ["RedBrush"] as SolidColorBrush;
                case AssignmentStatus.New:
                case AssignmentStatus.Active:
                case AssignmentStatus.Complete:
                default:
                    return app.Resources ["GreenBrush"] as SolidColorBrush;
            }
        }

        public static BitmapImage LoadImage (this byte [] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;
            var image = new BitmapImage ();
            using (var randomAccessStream = new InMemoryRandomAccessStream()) {
                var writeStream = randomAccessStream.AsStreamForWrite ();
                writeStream.Write (bytes, 0, bytes.Length);
                writeStream.Flush ();
                randomAccessStream.Seek (0);
                image.SetSource (randomAccessStream);
            }
            return image;
        }

        public async static Task<byte []> LoadBytes (this Stream stream)
        {
            if (stream == null) {
                return null;
            }
            using (var reader = new DataReader(stream.AsInputStream())) {
                await reader.LoadAsync ((uint)stream.Length);
                var bytes = new byte [stream.Length];
                reader.ReadBytes (bytes);
                return bytes;
            }
        }
    }
}
