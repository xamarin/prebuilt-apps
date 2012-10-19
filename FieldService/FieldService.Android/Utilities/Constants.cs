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

namespace FieldService.Android.Utilities {
    public static class Constants {
        public static List<string> Navigation = new List<string> () { "Summary", "Map", "Items", "Labor Hours", "Expenses", "History", "Documents", "Confirmations" };
    }
}