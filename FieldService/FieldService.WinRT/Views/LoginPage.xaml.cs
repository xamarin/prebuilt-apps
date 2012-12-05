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
using FieldService.WinRT.ViewModels;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace FieldService.WinRT.Views {
    /// <summary>
    /// The initial login page
    /// </summary>
    public sealed partial class LoginPage : Page {
        bool navigatedTo = false;
        readonly LoginViewModel loginViewModel;

        public LoginPage ()
        {
            this.InitializeComponent ();

            DataContext =
                loginViewModel = ServiceContainer.Resolve<LoginViewModel> ();

            //Generally I would use two-way bindings here, but UpdateSourceTrigger is not available in WinRT
            username.TextChanged += (sender, e) => loginViewModel.Username = username.Text;
            password.PasswordChanged += (sender, e) => loginViewModel.Password = password.Password;

            //Hook up keyboard events
            var inputPane = InputPane.GetForCurrentView ();
            inputPane.Showing += (sender, e) => {
                if (navigatedTo) {
                    keyboardShowingFrame.Value = -e.OccludedRect.Height / 2;
                    keyboardShowing.Begin ();
                }
            };
            inputPane.Hiding += (sender, e) => {
                if (navigatedTo)
                    keyboardHiding.Begin ();
            };
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
            navigatedTo = true;
            username.Text = string.Empty;
            password.Password = string.Empty;
        }

        protected override void OnNavigatedFrom (NavigationEventArgs e)
        {
            base.OnNavigatedFrom (e);

            navigatedTo = false;
        }

        /// <summary>
        /// Used for hooking up enter key to login
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown (KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                e.Handled = true;
                if (string.IsNullOrEmpty (password.Password) && FocusManager.GetFocusedElement () is TextBox) {

                    password.Focus (Windows.UI.Xaml.FocusState.Keyboard);
                    e.Handled = true;
                } else {
                    loginViewModel.LoginCommand.Invoke ();
                }
            }

            base.OnKeyDown (e);
        }
    }
}