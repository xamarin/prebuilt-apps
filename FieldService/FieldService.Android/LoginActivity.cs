using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FieldService.ViewModels;
using FieldService.Data;
using System.Threading.Tasks;

namespace FieldService.Android {
    [Activity (Label = "Login", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : Activity {
        private readonly LoginViewModel loginViewModel;
        private readonly DefaultService serivce;
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
            Button button = FindViewById<Button> (Resource.Id.MyButton);
            EditText userName = FindViewById<EditText> (Resource.Id.userName);
            EditText password = FindViewById<EditText> (Resource.Id.password);

            button.Click += delegate {
                loginViewModel.Username = userName.Text;
                loginViewModel.Password = password.Text;
                if (loginViewModel.IsValid) {
                    Task<bool> success = loginViewModel.LoginAsync ();
                    if (success.IsCompleted) {
                        Toast.MakeText (this, "Login Success", ToastLength.Long).Show ();
                    }
                }
            };
        }
    }
}

