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
using SQLite;

namespace FieldService.Data {
    /// <summary>
    /// Holds a photo for an assignment
    /// </summary>
    public class Photo {
        /// <summary>
        /// Photo Id
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Link to assignment
        /// </summary>
        public int AssignmentId { get; set; }

        /// <summary>
        /// Actual image
        /// </summary>
        public byte [] Image { get; set; }

        /// <summary>
        /// Date image was saved
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Description for photo
        /// </summary>
        public string Description { get; set; }

        #region UI properties
        /// <summary>
        /// The time component of the DateTime on the photo
        /// </summary>
        [Ignore]
        public string TimeFormatted
        {
            get { return Date.ToString ("t"); }
        }

        /// <summary>
        /// The date component of the DateTime on the photo
        /// </summary>
        [Ignore]
        public string DateFormatted
        {
            get { return Date.ToString ("d"); }
        }
        #endregion
    }
}
