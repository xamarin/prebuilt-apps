using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Utilities;
using Windows.UI.Xaml.Controls;

namespace FieldService.WinRT.Utilities {
    /// <summary>
    /// Set of helper methods for WinRT
    /// </summary>
    public static class Helpers {
        /// <summary>
        /// Helper method to navigate to a particular page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void NavigateTo<T> ()
            where T : Page
        {
            var app = ServiceContainer.Resolve<App> ();

            if (!app.RootFrame.Navigate (typeof (T), null)) {
                throw new InvalidOperationException ("Failed to navigation to: " + typeof (T));
            }
        }
    }
}
