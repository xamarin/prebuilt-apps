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
using System.Diagnostics;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.ViewModels;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Xamarin.Media;

namespace FieldService.WinRT.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssignmentPage : Page {
        readonly AssignmentViewModel assignmentViewModel;
        readonly ItemViewModel itemViewModel;
        readonly LaborViewModel laborViewModel;
        readonly PhotoViewModel photoViewModel;
        readonly ExpenseViewModel expenseViewModel;
        readonly DocumentViewModel documentViewModel;
        readonly HistoryViewModel historyViewModel;
        MediaPicker picker;

        public AssignmentPage ()
        {
            this.InitializeComponent ();

            DataContext =
                assignmentControl.DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            
            itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();
            
            laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();
            
            photoViewModel = ServiceContainer.Resolve<PhotoViewModel> ();
            
            expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel> ();
            
            documentViewModel = ServiceContainer.Resolve<DocumentViewModel> ();
            
            historyViewModel = ServiceContainer.Resolve<HistoryViewModel> ();

            picker = new MediaPicker ();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
            itemViewModel.LoadAssignmentItemsAsync (assignmentViewModel.SelectedAssignment);

            laborViewModel.LoadLaborHoursAsync (assignmentViewModel.SelectedAssignment);

            photoViewModel.LoadPhotosAsync (assignmentViewModel.SelectedAssignment);

            expenseViewModel.LoadExpensesAsync (assignmentViewModel.SelectedAssignment);

            documentViewModel.LoadDocumentsAsync ();

            historyViewModel.LoadHistoryAsync (assignmentViewModel.SelectedAssignment);
        }
        
        private async void OnDocumentItemClick (object sender, ItemClickEventArgs e)
        {
            var document = e.ClickedItem as Document;
            if (document != null) {
                var options = new Windows.System.LauncherOptions { DisplayApplicationPicker = true };
                var file = await Package.Current.InstalledLocation.GetFileAsync (document.Path.Replace('/', '\\'));
                var success = await Windows.System.Launcher.LaunchFileAsync (file, options);
            }
        }

        private void OnSummaryClick (object sender, ItemClickEventArgs e)
        {
            var element = e.ClickedItem as FrameworkElement;
            switch (element.Name) {
                case "mapTile":
                case "mapButton":
                    Helpers.NavigateTo<AssignmentMapPage> ();
                    break;
                case "totalHours":
                    Helpers.NavigateTo<LaborPage> ();
                    break;
                case "totalItems":
                    Helpers.NavigateTo<ItemsPage> ();
                    break;
                case "confirmation":
                    Helpers.NavigateTo<ConfirmationsPage> ();
                    break;
                case "expenses":
                    Helpers.NavigateTo<ExpensesPage> ();
                    break;
                case "documents":
                    Helpers.NavigateTo<DocumentsPage> ();
                    break;
                case "history":
                    Helpers.NavigateTo<AssignmentHistoryPage> ();
                    break;
                default:
                    break;
            }
        }
    }
}
