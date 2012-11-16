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
using System.Globalization;
using FieldService.Data;

namespace FieldService.Utilities {
    public static class DataExtensions {
        public static string ToUserString (this LaborType type)
        {
            switch (type) {
                case LaborType.Hourly:
                    return Catalog.GetString ("LaborTypeHourly");
                case LaborType.OverTime:
                    return Catalog.GetString ("LaborTypeOverTime");
                case LaborType.HolidayTime:
                    return Catalog.GetString ("LaborTypeHolidayTime");
                default:
                    return type.ToString ();
            }
        }

        /// <summary>
        /// Helper method to safely convert a string to a double
        /// </summary>
        public static double ToDouble (this string text, IFormatProvider provider)
        {
            double x;
            double.TryParse (text, NumberStyles.Any, provider, out x);
            return x;
        }

        /// <summary>
        /// Helper method to safely convert a string to a decimal
        /// </summary>
        public static decimal ToDecimal (this string text, IFormatProvider provider)
        {
            decimal x;
            decimal.TryParse (text, NumberStyles.Any, provider, out x);
            return x;
        }

        /// <summary>
        /// Helper method to safely convert a string to a int
        /// </summary>
        public static int ToInt (this string text, IFormatProvider provider)
        {
            int value = 0;
            int.TryParse (text, NumberStyles.Any, provider, out value);
            return value;
        }

#if NETFX_CORE
        /// <summary>
        /// Missing functionality for WinRT
        /// </summary>
        public static string ToShortDateString (this DateTime date)
        {
            return date.ToString (DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
        }

        /// <summary>
        /// Missing functionality for WinRT
        /// </summary>
        public static string ToShortTimeString (this DateTime date)
        {
            return date.ToString (DateTimeFormatInfo.CurrentInfo.ShortTimePattern);
        }
#endif
    }
}