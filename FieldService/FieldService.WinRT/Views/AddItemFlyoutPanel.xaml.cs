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
using FieldService.WinRT.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Add item flyout panel
    /// </summary>
    public sealed partial class AddItemFlyoutPanel : UserControl {
        ItemViewModel itemViewModel;
        AssignmentViewModel assignmentViewModel;

        public AddItemFlyoutPanel ()
        {
            this.InitializeComponent ();

            DataContext =
                itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();

            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            itemSearchText.TextChanged += (sender, e) => {
                itemViewModel.SearchText = itemSearchText.Text;
            };
        }

        /// <summary>
        /// List view item click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnItemClick (object sender, ItemClickEventArgs e)
        {
            Item item = e.ClickedItem as Item;
            if (item != null) {
                AssignmentItem assignmentItem = new AssignmentItem { 
                    AssignmentId = assignmentViewModel.SelectedAssignment.Id, 
                    ItemId = item.Id,
                };
                itemViewModel.SaveAssignmentItemCommand.Invoke (assignmentItem);
                await itemViewModel.LoadAssignmentItemsAsync (assignmentViewModel.SelectedAssignment);
                itemViewModel.CancelAddItemCommand.Invoke ();
            }
        }

        /// <summary>
        /// Used for hooking up enter key to search
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown (KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                itemViewModel.SearchItemsCommand.Invoke ();
                e.Handled = true;
            }
            base.OnKeyDown (e);
        }
    }
}
