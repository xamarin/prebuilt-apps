using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace FieldService.WinRT.Utilities {

    /// <summary>
    /// Converts a Boolean into a Visibility.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter {
        /// <summary>
        /// If set to True, conversion is reversed: True will become Collapsed.
        /// </summary>
        public bool IsReversed { get; set; }

        public object Convert (object value, Type targetType, object parameter, string language)
        {
            bool boolean = System.Convert.ToBoolean (value);
            if (IsReversed) {
                boolean = !boolean;
            }

            if (boolean) {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack (object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException ();
        }
    }
}
