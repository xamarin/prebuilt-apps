using FieldService.Data;
using FieldService.WinRT.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace FieldService.WinRT.Views {
    /// <summary>
    /// The initial login page
    /// </summary>
    public sealed partial class LoginPage : Page {
        readonly LoginViewModel loginViewModel;

        public LoginPage ()
        {
            this.InitializeComponent ();

            DataContext =
                loginViewModel = new LoginViewModel (new SampleLoginService ());

            //Generally I would use two-way bindings here, but UpdateSourceTrigger is not available in WinRT
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

        /// <summary>
        /// Used for hooking up enter key to login
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown (KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                e.Handled = true;
                loginViewModel.LoginCommand.Invoke ();
            }

            base.OnKeyDown (e);
        }
    }
}