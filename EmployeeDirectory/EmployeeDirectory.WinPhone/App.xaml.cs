//
//  Copyright 2012, Xamarin Inc.
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
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EmployeeDirectory.Data;
using EmployeeDirectory.ViewModels;

namespace EmployeeDirectory.WinPhone {
    public partial class App : Application {
        /// <summary>
        /// Gets the App object for the current application.
        /// </summary>
        public new static App Current { get { return (App)Application.Current; } }

        /// <summary>
        /// Gets the <see cref="IFavoritesRepository" /> for the application.
        /// </summary>
        public IFavoritesRepository FavoritesRepository { get; private set; }

        /// <summary>
        /// Gets the <see cref="IDirectoryService" /> for the application.
        /// </summary>
        public IDirectoryService DirectoryService { get; private set; }

        /// <summary>
        /// Gets the last <see cref="Search" /> performed.
        /// </summary>
        public Search SavedSearch { get; private set; }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// last time the device was used.
        /// </summary>
        private DateTime lastUseTime;

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App ()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent ();

            // Phone-specific initialization
            InitializePhoneApplication ();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached) {
                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        void OnPersonListItemTap (object sender, System.Windows.Input.GestureEventArgs e)
        {
            var person = ((FrameworkElement)sender).DataContext as Person;
            if (person == null) return;

            var url = string.Format (
                    "/PersonPage.xaml?id={0}",
                    Uri.EscapeDataString (person.Id));

            ((Page)RootFrame.Content).NavigationService.Navigate (new Uri (url, UriKind.Relative));
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching (object sender, LaunchingEventArgs e)
        {
            // Load the directory
            var dataUri = new Uri ("EmployeeDirectory.WinPhone;component/Data/XamarinDirectory.csv", UriKind.Relative);
            var dataInfo = GetResourceStream (dataUri);
            using (var reader = new System.IO.StreamReader (dataInfo.Stream)) {
                DirectoryService = new MemoryDirectoryService (new CsvReader<Person> (reader).ReadAll ());
            }

            // Load the favorites
            FavoritesRepository = XmlFavoritesRepository.OpenFile ("EmployeeDirectory.WinPhone;component/Data/XamarinFavorites.xml");

            // Load the search
            try {
                SavedSearch = Search.Open ("SavedSearch.xml");
            } catch (Exception) {
                SavedSearch = new Search ("SavedSearch.xml");
            }
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated (object sender, ActivatedEventArgs e)
        {
            if (LoginViewModel.ShouldShowLogin (lastUseTime)) {
                ((Page)RootFrame.Content).NavigationService.Navigate (new Uri ("/LoginPage.xaml", UriKind.Relative));
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated (object sender, DeactivatedEventArgs e)
        {
            lastUseTime = DateTime.UtcNow;
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing (object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed (object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached) {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break ();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException (object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached) {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break ();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication ()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame ();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication (object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}
