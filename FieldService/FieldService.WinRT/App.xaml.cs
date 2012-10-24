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

using FieldService.Utilities;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.ViewModels;
using FieldService.WinRT.Views;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FieldService.WinRT {
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App ()
        {
            InitializeComponent ();
            RootFrame = new Frame ();
            Suspending += OnSuspending;

            //Register services for core library
            Bootstrapper.Startup ();

            //WinRT specific services
            ServiceContainer.Register<LoginViewModel> ();
            ServiceContainer.Register(this);
        }

        public Frame RootFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched (LaunchActivatedEventArgs args)
        {
            Window.Current.Content = RootFrame;

            if (RootFrame.Content == null) {
                Helpers.NavigateTo<LoginPage> ();
            }

            // Ensure the current window is active
            Window.Current.Activate ();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending (object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral ();
            //TODO: Save application state and stop any background activity
            deferral.Complete ();
        }
    }
}