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
using FieldService.Data;
using FieldService.WinRT.Utilities;

namespace FieldService.WinRT.Views {
    /// <summary>
    /// The page for the labor hours
    /// </summary>
    public sealed partial class LaborPage : Page {
        readonly AssignmentViewModel assignmentViewModel;
        readonly LaborViewModel laborViewModel;

        public LaborPage ()
        {
            this.InitializeComponent ();

            DataContext =
                assignmentControl.DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            laborListView.DataContext =
                laborAddLabor.DataContext =
                laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();

            Window.Current.SizeChanged += (sender, e) => {
                if (laborViewModel.LaborPopUp != null && laborViewModel.LaborPopUp.IsOpen) {
                    laborViewModel.LaborPopUp.SetValue (Canvas.LeftProperty, Window.Current.Bounds.Width - Constants.PopUpWidth);
                }
            };
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
        }

        /// <summary>
        /// Invoked when this page is unloaded.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom (NavigationEventArgs e)
        {
            if (laborViewModel.LaborPopUp != null && laborViewModel.LaborPopUp.IsOpen) {
                laborViewModel.LaborPopUp.IsOpen = false;
            }
            base.OnNavigatedFrom (e);
        }

        /// <summary>
        /// Item click event for the labor list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var labor = e.ClickedItem as Labor;
            if (labor != null)
            {
                laborViewModel.AddLaborCommand.Invoke(labor);
            }
        }
    }
}
