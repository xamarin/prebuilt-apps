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
using System.IO;
using System.Linq;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.WinRT.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FieldService.WinRT.Views {
    /// <summary>
    /// Images page
    /// </summary>
    public sealed partial class ImagesPage : Page {
        AssignmentViewModel assignmentViewModel;
        PhotoViewModel photoViewModel;

        public ImagesPage ()
        {
            this.InitializeComponent ();

            imagesGoBack.DataContext =
                background.DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            DataContext =
                photosListView.DataContext =
                photoViewModel = ServiceContainer.Resolve<PhotoViewModel> ();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {

        }

        private void OnItemClick (object sender, ItemClickEventArgs e)
        {
            Photo photo = e.ClickedItem as Photo;
            if (photo != null) {
                photoViewModel.PhotoSelectedCommand.Invoke (photo);
            }
        }
    }
}
