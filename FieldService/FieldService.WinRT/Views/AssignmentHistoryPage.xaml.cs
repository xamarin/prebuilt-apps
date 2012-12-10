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
    /// History Page for viewing all history items
    /// </summary>
    public sealed partial class AssignmentHistoryPage : Page {
        HistoryViewModel historyViewModel;
        AssignmentViewModel assignmentViewModel;

        public AssignmentHistoryPage ()
        {
            this.InitializeComponent ();

            DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            
            historyListView.DataContext =
                historyViewModel = ServiceContainer.Resolve<HistoryViewModel> ();
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
        /// History list view item click 
        /// </summary>
        private void OnItemClick (object sender, ItemClickEventArgs e)
        {
            var history = e.ClickedItem as AssignmentHistory;
            if (history != null) {

            }
        }
    }
}
