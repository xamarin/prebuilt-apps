using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.GoogleMaps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FieldService.Android.Utilities;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android {
    /// <summary>
    /// Base activity for all "map" activities, handles global OnPause/OnResume
    /// </summary>
    public class BaseMapActivity : MapActivity {
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

        /// <summary>
        /// Required abstract member, we don't need to display routes
        /// </summary>
        protected override bool IsRouteDisplayed
        {
            get { return false; }
        }
    }
}