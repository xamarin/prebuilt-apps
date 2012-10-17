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
using System.Text.RegularExpressions;
using FieldService.Data;

namespace FieldService.Utilities
{
	public static class DataExtensions
	{
            private static Regex _enumRegex = new Regex (@"[^\w\d_]", RegexOptions.Compiled);

                public static string ToUserString (this LaborType type)
                {
                    switch (type) {
                        case LaborType.Hourly:
                            return "Hourly";
                        case LaborType.OverTime:
                            return "Over Time";
                        case LaborType.HolidayTime:
                            return "Holiday Time";
                        default:
                            return type.ToString ();
                    }
                }

                public static string ToUserString (this ExpenseCategory category)
                {
                    switch (category) {
                        case ExpenseCategory.Food:
                            return "Foor";
                        case ExpenseCategory.Gas:
                            return "Gas";
                        case ExpenseCategory.Supplies:
                            return "Supplies";
                        case ExpenseCategory.Other:
                            return "Other";
                        default:
                            return category.ToString ();
                    }
                }

                public static object ToEnum (Type enumType, string value)
                {
                    try {
                        if (!string.IsNullOrEmpty (value)) {
                            return Enum.Parse (enumType, _enumRegex.Replace (value, string.Empty), true);
                        } else {
                            return Activator.CreateInstance (enumType);
                        }
                    } catch {
                        return Activator.CreateInstance (enumType);
                    }
                }
	}
}

