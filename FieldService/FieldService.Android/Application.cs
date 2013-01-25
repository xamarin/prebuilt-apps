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

namespace FieldService.Android {
    [Application(Label="Field Service")]
    public class Application : global::Android.App.Application {

        /// <summary>
        /// Must implement this constructor for subclassing the application class.
        /// Will act as a global application class throughout the app.
        /// </summary>
        /// <param name="javaReference">pointer to java</param>
        /// <param name="transfer">transfer enumeration</param>
        public Application (IntPtr javaReference, JniHandleOwnership transfer)
            :base(javaReference, transfer)
        { }

        /// <summary>
        /// Override on create to instantiate the service container to be persistant.
        /// </summary>
        public override void OnCreate ()
        {
            base.OnCreate ();

            //Registers services for core library
            ServiceRegistrar.Startup ();
        }
    }
}