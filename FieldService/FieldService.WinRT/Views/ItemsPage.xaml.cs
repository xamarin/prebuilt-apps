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

using FieldService.Data;
using FieldService.Utilities;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FieldService.WinRT.Views {
    /// <summary>
    /// The page for the items
    /// </summary>
    public sealed partial class ItemsPage : Page {
        readonly AssignmentViewModel assignmentViewModel;
        readonly ItemViewModel itemViewModel;

        public ItemsPage ()
        {
            this.InitializeComponent ();

            DataContext =
                assignmentControl.DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            itemsListView.DataContext =
                itemsAddItem.DataContext =
                itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();

            applicationBar.Closed += (sender, e) => {
                itemViewModel.SelectedItem = null;
                applicationBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            };

            Window.Current.SizeChanged += (sender, e) => {
                if (itemViewModel.ItemPopUp != null && itemViewModel.ItemPopUp.IsOpen) {
                    itemViewModel.ItemPopUp.SetValue (Canvas.LeftProperty, Window.Current.Bounds.Width - Constants.PopUpWidth);
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
            if (itemViewModel.ItemPopUp != null && itemViewModel.ItemPopUp.IsOpen) {
                itemViewModel.ItemPopUp.IsOpen = false;
            }
            base.OnNavigatedFrom (e);
        }

        private void ItemClick (object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as AssignmentItem;
            if (item != null) {
                itemViewModel.SelectedItem = item;
                applicationBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
                applicationBar.IsOpen = true;
            }
        }
        private void Delete_Click (object sender, RoutedEventArgs e)
        {
            itemViewModel.DeleteItemCommand.Invoke ();
            applicationBar.IsOpen = false;
        }
    }
}
