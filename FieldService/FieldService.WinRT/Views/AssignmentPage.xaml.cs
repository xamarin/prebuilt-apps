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
using FieldService.Utilities;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.ViewModels;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FieldService.WinRT.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssignmentPage : Page {
        readonly AssignmentViewModel assignmentViewModel;
        readonly ItemViewModel itemViewModel;
        readonly LaborViewModel laborViewModel;

        public AssignmentPage ()
        {
            this.InitializeComponent ();

            DataContext =
                assignmentControl.DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            itemsListView.DataContext =
            summaryAddItems.DataContext = 
                itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();

            laborListView.DataContext =
                laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
            itemViewModel.LoadAssignmentItems (assignmentViewModel.SelectedAssignment);

            laborViewModel.LoadLaborHours (assignmentViewModel.SelectedAssignment);
        }

        private void OnItemClick (object sender, ItemClickEventArgs e)
        {
            AssignmentItem item = e.ClickedItem as AssignmentItem;
            if (item != null) {
                item.Used = !item.Used;
                itemViewModel.SaveAssignmentItemCommand.Invoke (item);
            }
        }

        private async void OnSummaryClick (object sender, ItemClickEventArgs e)
        {
            var element = e.ClickedItem as FrameworkElement;
            switch (element.Name) {
                case "mapTile":
                case "mapButton":
                    Helpers.NavigateTo<AssignmentMapPage> ();
                    break;
                default:
                    await new MessageDialog ("Coming soon!").ShowAsync ();
                    break;
            }
        }
    }
}
