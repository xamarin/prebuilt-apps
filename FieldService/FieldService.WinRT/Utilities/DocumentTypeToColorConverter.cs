using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace FieldService.WinRT.Utilities {
    public class DocumentTypeToColorConverter : IValueConverter {
        public object Convert (object value, Type targetType, object parameter, string language)
        {
            if (value != null) {
                var type = (DocumentType)value;
                switch (type) {
                    case DocumentType.Contract: {
                            var resources = Application.Current.Resources ["ItemsBrush"];
                            if (resources != null) {
                                return resources as LinearGradientBrush;
                            }
                        }
                        break;
                    case DocumentType.ServiceAgreement: {
                            var resources = Application.Current.Resources ["HoursBrush"];
                            if (resources != null) {
                                return resources as LinearGradientBrush;
                            }
                        }
                        break;
                    default: {
                            var resources = Application.Current.Resources ["ExpensesBrush"];
                            if (resources != null) {
                                return resources as SolidColorBrush;
                            }
                        }
                        break;
                }
            }
            return null;
        }

        public object ConvertBack (object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException ();
        }
    }
}
