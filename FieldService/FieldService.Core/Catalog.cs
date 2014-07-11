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

namespace FieldService {

    /// <summary>
    /// Static class containing strings used throughout the app
    /// - a developer could replace this with *.resx supporting globalization if desired
    /// </summary>
    public class Catalog {

        private const string UsernameValidation = "Please enter a username.";
        private const string PasswordValidation = "Please enter a password.";
        private const string LaborTypeHourly = "Hourly";
        private const string LaborTypeOverTime = "Over Time";
        private const string LaborTypeHolidayTime = "Holiday Time";

        public static string GetString (string key, string comment = null)
        {
            switch (key) {
                case "UsernameValidation":
                    return UsernameValidation;
                case "PasswordValidation":
                    return PasswordValidation;
                case "LaborTypeHourly":
                    return LaborTypeHourly;
                case "LaborTypeOverTime":
                    return LaborTypeOverTime;
                case "LaborTypeHolidayTime":
                    return LaborTypeHolidayTime;
                default:
                    return string.Empty;
            }
        }
    }
}
