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
        Button logIn;

        public LoginActivity ()
        {
            loginViewModel = new LoginViewModel (new DefaultService());
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Get our controls from the layout resource,
            // and attach an event to it
            logIn = FindViewById<Button> (Resource.Id.logIn);
            userName = FindViewById<EditText> (Resource.Id.userName);
            password = FindViewById<EditText> (Resource.Id.password);

            //Set edit action listener to allow the next & go buttons on the input keyboard to interact with login.
            userName.SetOnEditorActionListener (this);
            password.SetOnEditorActionListener (this);
            
            logIn.Click += delegate {
                loginViewModel.Username = userName.Text;
                loginViewModel.Password = password.Text;
                if (loginViewModel.IsValid) {
                    logIn.Enabled = false;
                    loginViewModel.LoginAsync ().ContinueOnUIThread (_ => {
                        logIn.Enabled = true;
                        Toast.MakeText (this, "Success!", ToastLength.Short).Show ();
                    });
                }
            };
        }

        public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
        {
            //go edit action will login
            if (actionId == ImeAction.Go) {
                loginViewModel.Username = userName.Text;
                loginViewModel.Password = password.Text;
                if (loginViewModel.IsValid) {
                    logIn.Enabled = false;
                    loginViewModel.LoginAsync ().ContinueOnUIThread (_ => {
                        logIn.Enabled = true;
                        Toast.MakeText (this, "Success!", ToastLength.Short).Show ();
                    });
                   var methodManager = (InputMethodManager)GetSystemService (Context.InputMethodService);
                   methodManager.HideSoftInputFromInputMethod (v.WindowToken, HideSoftInputFlags.None);
                }
                return true;
                //next action will set focus to password edit text.
            } else if(actionId == ImeAction.Next) {
                password.RequestFocus ();
                return true;
            }
            return false;
        }
    }
}

