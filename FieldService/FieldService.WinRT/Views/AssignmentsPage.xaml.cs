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
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
            //Do this to fix animation when navigating to screen
            assignmentViewModel.Assignments = null;

            if (!timerLoaded) {
                assignmentViewModel.LoadTimerEntry ().ContinueWith (assignmentViewModel.LoadAssignments ());
                timerLoaded = true;
            } else {
                assignmentViewModel.LoadAssignments ();
            }
        }

        private void OnItemClick (object sender, ItemClickEventArgs e)
        {
            assignmentViewModel.SelectedAssignment = e.ClickedItem as Assignment;

            Helpers.NavigateTo<AssignmentPage> ();
        }
    }
}