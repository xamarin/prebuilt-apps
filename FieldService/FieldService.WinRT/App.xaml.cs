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
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FieldService.WinRT {
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application {
        object lastPage;
        readonly LoginViewModel loginViewModel;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App ()
        {
            InitializeComponent ();

            //Create our RootFrame
            RootFrame = new Frame ();

            //Register services for core library
            ServiceRegistrar.Startup ();

            //WinRT specific services
            ServiceContainer.Register<LoginViewModel> ();
            ServiceContainer.Register<AssignmentViewModel> ();
            ServiceContainer.Register<ItemViewModel> ();
            ServiceContainer.Register<LaborViewModel> ();
            ServiceContainer.Register<PhotoViewModel>();
            ServiceContainer.Register<ExpenseViewModel> ();
            ServiceContainer.Register<DocumentViewModel> ();
            ServiceContainer.Register<HistoryViewModel> ();
            ServiceContainer.Register(this);

            loginViewModel = ServiceContainer.Resolve<LoginViewModel> ();
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
                Window.Current.CoreWindow.SizeChanged += OnSizeChanged;
                Window.Current.VisibilityChanged += OnVisibilityChanged;
                Helpers.NavigateTo<LoginPage> ();
            }

            // Ensure the current window is active
            Window.Current.Activate ();
        }

        /// <summary>
        /// Event when the visibility is changed on the main window
        /// </summary>
        private void OnVisibilityChanged (object sender, VisibilityChangedEventArgs e)
        {
            if (e.Visible) {
                if (loginViewModel.IsInactive) {
                    Helpers.NavigateTo<LoginPage> ();
                }
            } else {
                System.Diagnostics.Debug.WriteLine ("");
            }

            loginViewModel.ResetInactiveTime ();
        }

        private void OnSizeChanged (CoreWindow sender, WindowSizeChangedEventArgs args)
        {
            switch (ApplicationView.Value) {
                case ApplicationViewState.Filled:
                case ApplicationViewState.FullScreenLandscape:
                case ApplicationViewState.FullScreenPortrait:
                    if (lastPage != null) {
                        Helpers.NavigateTo (lastPage.GetType ());
                        lastPage = null;
                    }
                    break;
                case ApplicationViewState.Snapped:
                    lastPage = RootFrame.Content;
                    Helpers.NavigateTo<SnapPage> ();
                    break;
                default:
                    break;
            }
        }
    }
}