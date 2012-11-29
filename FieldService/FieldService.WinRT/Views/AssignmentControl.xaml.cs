using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.WinRT.Utilities;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FieldService.WinRT.Views {
    public sealed partial class AssignmentControl : UserControl {
        AssignmentViewModel assignmentViewModel;
        bool ignoreNext = true;

        public AssignmentControl ()
        {
            this.InitializeComponent ();

            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }

        private async void SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            if (ignoreNext) {
                ignoreNext = false;
                return;
            }
            var assignmentStatus = status.SelectedItem;
            switch ((AssignmentStatus)assignmentStatus) {
                case AssignmentStatus.Active:
                    timerControl.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    await assignmentViewModel.SaveAssignmentAsync (assignmentViewModel.SelectedAssignment);
                    await assignmentViewModel.LoadAssignmentsAsync ();
                    break;
                case AssignmentStatus.Hold:
                    timerControl.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    await assignmentViewModel.SaveAssignmentAsync (assignmentViewModel.SelectedAssignment);
                    await assignmentViewModel.LoadAssignmentsAsync ();
                    break;
                case AssignmentStatus.Complete:
                    //take you to the confirmations page.
                    break;
                default:
                    break;
            }            
        }
    }
}
