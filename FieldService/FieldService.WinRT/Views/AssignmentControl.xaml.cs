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

        public AssignmentControl ()
        {
            this.InitializeComponent ();

            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            this.Loaded += (sender, e) => {
                //toggle button state changed isn't being called unless is checked changes, this does not call the record button by setting this property
                //this was the only way at the moment to get the toggle button to have its recording state changed when navigating to and from this screen.
                record.IsChecked = !assignmentViewModel.Recording;
                record.IsChecked = assignmentViewModel.Recording;
            };
        }
        
        private async void SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            var assignmentStatus = status.SelectedItem;
            if (assignmentStatus != null && (AssignmentStatus)assignmentStatus != assignmentViewModel.SelectedAssignment.Status) {
                switch ((AssignmentStatus)assignmentStatus) {
                    case AssignmentStatus.Active:
                        timerControl.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        assignmentViewModel.SelectedAssignment.Status = AssignmentStatus.Active;
                        await assignmentViewModel.SaveAssignmentAsync (assignmentViewModel.SelectedAssignment);
                        await assignmentViewModel.LoadAssignmentsAsync ();
                        break;
                    case AssignmentStatus.Hold:
                        timerControl.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        assignmentViewModel.SelectedAssignment.Status = AssignmentStatus.Hold;
                        await assignmentViewModel.SaveAssignmentAsync (assignmentViewModel.SelectedAssignment);
                        await assignmentViewModel.LoadAssignmentsAsync ();
                        break;
                    case AssignmentStatus.Complete:
                        //Set back to old status and navigate
                        assignmentViewModel.SelectedAssignment.Status = (AssignmentStatus)e.RemovedItems.First ();
                        Helpers.NavigateTo<ConfirmationsPage> ();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
