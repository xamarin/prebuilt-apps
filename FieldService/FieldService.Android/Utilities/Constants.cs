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
    /// <summary>
    /// Contains a few constants used throughout the app
    /// </summary>
    public static class Constants {
        /// <summary>
        /// List of navigation items
        /// </summary>
        public static List<string> Navigation = new List<string> () { "Summary", "Map", "Items", "Labor Hours", "Confirmations" };
        /// <summary>
        /// Constants string for getting navigation index
        /// </summary>
        public const string Confirmations = "Confirmations";
        /// <summary>
        /// Key for PutExtra
        /// </summary>
        public const string BundleIndex = "index";
        /// <summary>
        /// Key for PutExtra for the fragments
        /// </summary>
        public const string FragmentIndex = "fragment";
        /// <summary>
        /// Key for PutInt
        /// </summary>
        public const string CurrentTab = "currentTab";
        /// <summary>
        /// Maximum width for images
        /// </summary>
        public const int MaxWidth = 400;
        /// <summary>
        /// Maximum height for images
        /// </summary>
        public const int MaxHeight = 400;
    }
}