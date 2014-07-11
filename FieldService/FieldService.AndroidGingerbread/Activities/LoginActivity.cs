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

using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android {
    /// <summary>
    /// Activity for the login screen
    /// </summary>
    [Activity (Label = "Field Service", LaunchMode = LaunchMode.SingleTop, MainLauncher = true, Theme = "@style/CustomHoloTheme", Icon = "@drawable/icon")]
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
            loginViewModel = ServiceContainer.Resolve<LoginViewModel> ();

            //sets valid changed to show the login button.
            loginViewModel.IsValidChanged += (sender, e) => {
                if (login != null) {
                    login.Enabled = loginViewModel.IsValid ? true : false;
                }
            };
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.LoginLayout);

            // Get our controls from the layout resource,
            // and attach an event to it
            login = FindViewById<Button> (Resource.Id.logIn);
            userName = FindViewById<EditText> (Resource.Id.userName);
            password = FindViewById<EditText> (Resource.Id.password);
            progressIndicator = FindViewById<ProgressBar> (Resource.Id.loginProgress);
            var loginHelp = FindViewById<ImageButton> (Resource.Id.loginQuestion);

            //Set edit action listener to allow the next & go buttons on the input keyboard to interact with login.
            userName.SetOnEditorActionListener (this);
            password.SetOnEditorActionListener (this);

            userName.TextChanged += (sender, e) => {
                loginViewModel.Username = userName.Text;
            };
            password.TextChanged += (sender, e) => {
                loginViewModel.Password = password.Text;
            };
            loginHelp.Click += (sender, e) => {
                    var builder = new AlertDialog.Builder (this)
                        .SetTitle ("Need Help?")
                        .SetMessage ("Enter any username or password.")
                        .SetPositiveButton ("Ok", (innerSender, innere) => { });
                    var dialog = builder.Create ();
                    dialog.Show ();
                };

            //initially set username & login to set isvalid on the view model.
            loginViewModel.Username = userName.Text;
            loginViewModel.Password = password.Text;

            //LogIn button click event
            login.Click += (sender, e) => Login ();

            //request focus to the edit text to start on username.
            userName.RequestFocus ();
        }

        /// <summary>
        /// Override on back pressed so when the user is on the inactive login screen we don't let them back out of it.
        /// </summary>
        public override void OnBackPressed ()
        {
            //if(!preventOnBackPressed)
                base.OnBackPressed ();
        }

        /// <summary>
        /// Perform the login and dismiss the keyboard
        /// </summary>
        private void Login ()
        {
            //this hides the keyboard
            var imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
            imm.HideSoftInputFromWindow (password.WindowToken, HideSoftInputFlags.NotAlways);
            login.Visibility = ViewStates.Invisible;
            progressIndicator.Visibility = ViewStates.Visible;

            loginViewModel
                .LoginAsync ()
                .ContinueWith (_ => {
                    RunOnUiThread (() => {
                        StartActivity (typeof (AssignmentTabActivity));
                    });
                });
        }

        protected override void OnResume ()
        {
            base.OnResume ();

            password.Text =
                userName.Text = string.Empty;

            login.Visibility = ViewStates.Visible;
            progressIndicator.Visibility = ViewStates.Invisible;
        }

        /// <summary>
        /// Observes the TextView's ImeAction so an action can be taken on keypress.
        /// </summary>
        public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
        {
            //go edit action will login
            if (actionId == ImeAction.Go) {
                if (loginViewModel.IsValid) {
                    Login ();
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