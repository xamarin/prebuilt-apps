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
    /// The page with a list of assignments
    /// </summary>
    public sealed partial class AssignmentsPage : Page {
        readonly AssignmentViewModel assignmentViewModel;
        bool timerLoaded = false;

        public AssignmentsPage ()
        {
            this.InitializeComponent ();

            DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected async override void OnNavigatedTo (NavigationEventArgs e)
        {
            //Do this to fix animation when navigating to screen
            assignmentViewModel.Assignments = null;

            if (!timerLoaded) {
                await assignmentViewModel.LoadTimerEntryAsync ();
                await assignmentViewModel.LoadAssignmentsAsync ();
                activeAssignment.Visibility = assignmentViewModel.ActiveAssignment != null ? Visibility.Visible : Visibility.Collapsed;
                timerLoaded = true;
            } else {
                await assignmentViewModel.LoadAssignmentsAsync ();
                activeAssignment.Visibility = assignmentViewModel.ActiveAssignment != null ? Visibility.Visible : Visibility.Collapsed;
            }
            //toggle button state changed isn't being called unless is checked changes, this does not call the record button by setting this property
            //this was the only way at the moment to get the toggle button to have its recording state changed when navigating to and from this screen.
            record.IsChecked = !assignmentViewModel.Recording;
            record.IsChecked = assignmentViewModel.Recording;
        }

        private void OnItemClick (object sender, ItemClickEventArgs e)
        {
            assignmentViewModel.SelectedAssignment = e.ClickedItem as Assignment;

            Helpers.NavigateTo<AssignmentPage> ();
        }

        private async void Status_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            if (activeAssignment.Visibility == Windows.UI.Xaml.Visibility.Visible) {
                if (assignmentViewModel.ActiveAssignment != null && status.SelectedItem != null) {
                    var assignmentStatus = status.SelectedItem;
                    switch ((AssignmentStatus)assignmentStatus) {
                        case AssignmentStatus.Hold:
                            activeAssignment.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                            assignmentViewModel.ActiveAssignment.Status = AssignmentStatus.Hold;
                            await assignmentViewModel.SaveAssignmentAsync (assignmentViewModel.ActiveAssignment);
                            await assignmentViewModel.LoadAssignmentsAsync ();
                            break;
                        case AssignmentStatus.Complete:
                            assignmentViewModel.ActiveAssignment.Status = AssignmentStatus.Active;
                            assignmentViewModel.SelectedAssignment = assignmentViewModel.ActiveAssignment;
                            await assignmentViewModel.SaveAssignmentAsync (assignmentViewModel.ActiveAssignment);
                            await assignmentViewModel.LoadAssignmentsAsync ();
                            Helpers.NavigateTo<AssignmentPage> ();
                            Helpers.NavigateTo<ConfirmationsPage> ();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ActiveAssignmentClick (object sender, RoutedEventArgs e)
        {
            assignmentViewModel.SelectedAssignment = assignmentViewModel.ActiveAssignment;

            Helpers.NavigateTo<AssignmentPage> ();
        }
    }
}