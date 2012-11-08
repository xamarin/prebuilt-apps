//
//  Copyright 2012  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

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
    public class DocumentSubheaderColorConverter : IValueConverter {
        public object Convert (object value, Type targetType, object parameter, string language)
        {
            if (value != null) {
                var type = (DocumentType)value;
                switch (type) {
                    case DocumentType.Contract: {
                            var resources = Application.Current.Resources ["DarkBlueItemsBrush"];
                            if (resources != null) {
                                return resources as SolidColorBrush;
                            }
                        }
                        break;
                    case DocumentType.ServiceAgreement: {
                        var resources = Application.Current.Resources ["DarkGreenButtonBorderBrush"];
                            if (resources != null) {
                                return resources as SolidColorBrush;
                            }
                        }
                        break;
                    default: {
                            var resources = Application.Current.Resources ["DarkRedExpensesBrush"];
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
