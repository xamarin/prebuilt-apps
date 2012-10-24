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
        readonly SolidColorBrush green = new SolidColorBrush (new Color {
            A = 0xFF,
            R = 0x00,
            G = 0xfd,
            B = 0x7d
        });
        readonly SolidColorBrush yellow = new SolidColorBrush (new Color {
            A = 0xFF,
            R = 0xfc,
            G = 0xff,
            B = 0x00
        });
        readonly SolidColorBrush red = new SolidColorBrush (new Color {
            A = 0xFF,
            R = 0xd5,
            G = 0x30,
            B = 0x30
        });

        public object Convert (object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return green;

            var status = (AssignmentStatus)value;
            switch (status) {                    
                case AssignmentStatus.Hold:
                    return yellow;
                case AssignmentStatus.Declined:
                    return red;
                case AssignmentStatus.New:
                case AssignmentStatus.Active:
                case AssignmentStatus.Complete:
                default:
                    return green;
            }
        }

        public object ConvertBack (object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException ();
        }
    }
}
