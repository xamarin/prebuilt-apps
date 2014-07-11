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
        public static List<string> Navigation = new List<string> () { "Summary", "Map", "Items", "Labor Hours", "Expenses", "History", "Documents", "Confirmations" };
        /// <summary>
        /// List of navigation items
        /// </summary>
        public static List<string> HistoryNavigation = new List<string> () { "Summary", "Map", "Items", "Labor Hours", "Expenses", "Documents", "Confirmations" };
        /// <summary>
        /// Constants string for getting navigation index
        /// </summary>
        public const string Confirmations = "Confirmations";
        /// <summary>
        /// Constants string for document pdf
        /// </summary>
        public const string PdfFile = "FieldServiceDoc.pdf";
        /// <summary>
        /// Constants string for log tag
        /// </summary>
        public const string LogTag = "FieldService";
        /// <summary>
        /// Key for PutExtra
        /// </summary>
        public const string BundleIndex = "index";
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