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
using EmployeeDirectory.Data;

namespace EmployeeDirectory.Android {
    [Application (Label = "Employees", Theme = "@style/CustomHoloTheme")]
    public class Application : global::Android.App.Application {

        public Application (IntPtr javaReference, JniHandleOwnership transfer)
            :base(javaReference, transfer)
        {

        }

        public override void OnCreate ()
        {
            base.OnCreate ();

            using (var reader = new System.IO.StreamReader (Assets.Open ("XamarinDirectory.csv"))) {
                Service = new MemoryDirectoryService (new CsvReader<Person> (reader).ReadAll ());
            }
        }

        public static IDirectoryService Service
        {
            get;
            set;
        }
    }
}