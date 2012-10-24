using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Utilities;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FieldService.WinRT.Utilities {
    /// <summary>
    /// Set of helper methods for WinRT
    /// </summary>
    public static class Helpers {
        /// <summary>
        /// Helper method to navigate to a particular page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void NavigateTo<T> (object parameter = null)
            where T : Page
        {
            var app = ServiceContainer.Resolve<App> ();

            if (!app.RootFrame.Navigate (typeof (T), parameter)) {
                throw new InvalidOperationException ("Failed to navigation to: " + typeof (T));
            }
        }

        public static bool CanGoBack
        {
            get
            {
                var app = ServiceContainer.Resolve<App> ();
                return app.RootFrame.CanGoBack;
            }
        }

        public static void GoBack ()
        {
            var app = ServiceContainer.Resolve<App> ();
            app.RootFrame.GoBack ();
        }

        public static SolidColorBrush GetBrushForStatus (this AssignmentStatus status)
        {
            var app = ServiceContainer.Resolve<App> ();

            switch (status) {
                case AssignmentStatus.Hold:
                    return app.Resources ["YellowBrush"] as SolidColorBrush;
                case AssignmentStatus.Declined:
                    return app.Resources ["RedBrush"] as SolidColorBrush;
                case AssignmentStatus.New:
                case AssignmentStatus.Active:
                case AssignmentStatus.Complete:
                default:
                    return app.Resources ["GreenBrush"] as SolidColorBrush;
            }
        }
    }
}
