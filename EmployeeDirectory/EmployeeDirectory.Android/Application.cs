using System;
using System.Collections.Generic;
using System.IO;
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
        private static IFavoritesRepository repo;

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

            var filePath = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), "XamarinFavorites.xml");
            using (var stream = Assets.Open ("XamarinFavorites.xml")) {
                using (var filestream = File.Open (filePath, FileMode.Create)) {
                    stream.CopyTo (filestream);
                }
            }
            repo = XmlFavoritesRepository.OpenFile (filePath);
        }

        public static IDirectoryService Service
        {
            get;
            set;
        }

        public static IFavoritesRepository SharedFavoritesRepository
        {
            get { return repo; }
        }
    }
}