using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.WinRT.ViewModels;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FieldService.WinRT.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DocumentsPage : Page {
        readonly DocumentViewModel documentViewModel;
        readonly AssignmentViewModel assignmentViewModel;

        public DocumentsPage ()
        {
            this.InitializeComponent ();

            DataContext = 
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

            documentsListView.DataContext =
                documentViewModel = ServiceContainer.Resolve<DocumentViewModel> ();

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
        }

        private async void OnDocumentClick (object sender, ItemClickEventArgs e)
        {
            var document = e.ClickedItem as Document;
            if (document != null) {
                var options = new Windows.System.LauncherOptions { DisplayApplicationPicker = true };
                var file = await Package.Current.InstalledLocation.GetFileAsync (document.Path.Replace ('/', '\\'));
                var success = await Windows.System.Launcher.LaunchFileAsync (file, options);
            }
        }
    }
}
