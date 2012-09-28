using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FieldService.Data;
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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page {
        readonly LoginViewModel loginViewModel;

        public LoginPage ()
        {
            this.InitializeComponent ();

            DataContext =
                loginViewModel = new LoginViewModel (new DefaultService ());

            username.TextChanged += (sender, e) => loginViewModel.Username = username.Text;
            password.PasswordChanged += (sender, e) => loginViewModel.Password = password.Password;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
            username.Text = string.Empty;
            password.Password = string.Empty;
        }
    }
}