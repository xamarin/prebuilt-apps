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

using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Data {
    /// <summary>
    /// An item to be used on an assignment
    /// </summary>
    public class Item {
        /// <summary>
        /// Id of the item
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Number of the item
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Number & Name of Item for WinRT
        /// </summary>
        [Ignore]
        public string NumberName { get { return string.Format("#{0} {1}", Number, Name); } }
    }
}