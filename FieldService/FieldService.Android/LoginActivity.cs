using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android {
    [Activity (Label = "Login", MainLauncher = true, Theme = "@style/CustomHoloTheme", Icon = "@drawable/icon")]
    public class LoginActivity : Activity, TextView.IOnEditorActionListener {
        readonly LoginViewModel loginViewModel;
        EditText password, userName;
        Button login;
        ProgressBar progressIndicator;

        /// <summary>
        /// Class constructor
        /// </summary>
        public LoginActivity ()
        {
            loginViewModel = new LoginViewModel (new DefaultService ());

            //busy changed to hide log in button and show spinner
            loginViewModel.IsBusyChanged += (sender, e) => {
                if (login != null) {
                    login.Visibility = loginViewModel.IsBusy ? ViewStates.Invisible : ViewStates.Visible;
                    progressIndicator.Visibility = loginViewModel.IsBusy ? ViewStates.Visible : ViewStates.Invisible;
                }
            };

            //sets valid changed to show the login button.
            loginViewModel.IsValidChanged += (sender, e) => {
                if (login != null) {
                    login.Visibility = loginViewModel.IsValid ? ViewStates.Visible : ViewStates.Invisible;
                }
            };
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Login);

            // Get our controls from the layout resource,
            // and attach an event to it
            login = FindViewById<Button> (Resource.Id.logIn);
            userName = FindViewById<EditText> (Resource.Id.userName);
            password = FindViewById<EditText> (Resource.Id.password);
            progressIndicator = FindViewById<ProgressBar> (Resource.Id.loginProgress);

            //Set edit action listener to allow the next & go buttons on the input keyboard to interact with login.
            userName.SetOnEditorActionListener (this);
            password.SetOnEditorActionListener (this);

            userName.TextChanged += (sender, e) => {
                loginViewModel.Username = userName.Text;
            };
            password.TextChanged += (sender, e) => {
                loginViewModel.Password = password.Text;
            };

            //initially set username & login to set isvalid on the view model.
            loginViewModel.Username = userName.Text;
            loginViewModel.Password = password.Text;
            
            //LogIn button click event
            login.Click += (sender, e) => Login ();
            
            //request focus to the edit text to start on username.
            userName.RequestFocus ();
        }

        private void Login ()
        {
            loginViewModel.LoginAsync ().ContinueOnUIThread (_ => {
                login.Enabled = true;
                Toast.MakeText (this, "Success!", ToastLength.Short).Show ();
            });
        }

        /// <summary>
        /// Observes the TextView's ImeAction so an action can be taken on keypress.
        /// </summary>
        /// <param name="v">TextView that the action was called from</param>
        /// <param name="actionId">Ime Action Id, Next, Go, Done, etc.</param>
        /// <param name="e">Key event being called from the Ime Action Id.</param>
        /// <returns></returns>
        public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
        {
            //go edit action will login
            if (actionId == ImeAction.Go) {
                if (loginViewModel.IsValid) {
                    Login ();
                    var methodManager = (InputMethodManager)GetSystemService (Context.InputMethodService);
                    methodManager.HideSoftInputFromInputMethod (v.WindowToken, HideSoftInputFlags.None);
                }
                return true;
                //next action will set focus to password edit text.
            } else if (actionId == ImeAction.Next) {
                password.RequestFocus ();
                return true;
            }
            return false;
        }
    }
}

