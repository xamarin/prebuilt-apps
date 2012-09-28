using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FieldService.Utilities;
using FieldService.ViewModels;
using FieldService.Data;
using System.Threading.Tasks;
using Android.Views.InputMethods;

namespace FieldService.Android {
    [Activity (Label = "Login", MainLauncher = true, Theme="@android:style/Theme.Holo", Icon = "@drawable/icon")]
    public class LoginActivity : Activity, TextView.IOnEditorActionListener {
        private readonly LoginViewModel loginViewModel;
        private readonly DefaultService serivce;
        private EditText password,
            userName;
        private Button button;

        public LoginActivity ()
        {
            serivce = new DefaultService();
            loginViewModel = new LoginViewModel (serivce);
            
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            button = FindViewById<Button> (Resource.Id.MyButton);
            userName = FindViewById<EditText> (Resource.Id.userName);
            password = FindViewById<EditText> (Resource.Id.password);

            userName.SetOnEditorActionListener (this);
            password.SetOnEditorActionListener (this);
            
            button.Click += delegate {
                loginViewModel.Username = userName.Text;
                loginViewModel.Password = password.Text;
                if (loginViewModel.IsValid) {
                    button.Enabled = false;
                    loginViewModel.LoginAsync ().ContinueOnUIThread (_ => {
                        button.Enabled = true;
                        Toast.MakeText (this, "Success!", ToastLength.Short).Show ();
                    });
                }
            };
        }

        public bool OnEditorAction (TextView v, global::Android.Views.InputMethods.ImeAction actionId, KeyEvent e)
        {
            if (actionId == global::Android.Views.InputMethods.ImeAction.Go) {
                loginViewModel.Username = userName.Text;
                loginViewModel.Password = password.Text;
                if (loginViewModel.IsValid) {
                    button.Enabled = false;
                    loginViewModel.LoginAsync ().ContinueOnUIThread (_ => {
                        button.Enabled = true;
                        Toast.MakeText (this, "Success!", ToastLength.Short).Show ();
                    });
                   InputMethodManager methodManager = (InputMethodManager)GetSystemService (Context.InputMethodService);
                   methodManager.HideSoftInputFromInputMethod (v.WindowToken, HideSoftInputFlags.None);
                }
                return true;
            } else if(actionId == global::Android.Views.InputMethods.ImeAction.Next) {
                password.RequestFocus ();
                return true;
            }
            return false;
        }
    }
}

