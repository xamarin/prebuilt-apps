using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace FieldService.WinRT.Utilities {
    /// <summary>
    /// Converter for converting an AssignmentStatus to a Brush
    /// </summary>
    public class StatusToColorConverter : IValueConverter {
        public object Convert (object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return AssignmentStatus.New.GetBrushForStatus();

            var status = (AssignmentStatus)value;
            return status.GetBrushForStatus();
        }

        public object ConvertBack (object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException ();
        }
    }
}
