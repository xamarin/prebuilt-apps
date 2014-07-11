using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android {
    /// <summary>
    /// Base activity for all activities, handles global OnPause/OnResume
    /// </summary>
    public class BaseActivity : Activity {
        readonly LoginViewModel loginViewModel = ServiceContainer.Resolve<LoginViewModel> ();

        protected override void OnPause ()
        {
            base.OnPause ();

            loginViewModel.ResetInactiveTime ();
        }

        protected override void OnResume ()
        {
            base.OnResume ();

            if (loginViewModel.IsInactive) {
                var intent = new Intent (this, typeof (LoginActivity));
                intent.SetFlags (ActivityFlags.ClearTop);
                StartActivity (intent);
            }

            //Just to be safe 
            loginViewModel.ResetInactiveTime ();
        }
    }
}