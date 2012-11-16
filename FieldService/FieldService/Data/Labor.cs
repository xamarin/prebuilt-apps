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
using FieldService.Utilities;

namespace FieldService.Data {
    /// <summary>
    /// Labor to be performed on an assignment
    /// </summary>
    public class Labor {
        /// <summary>
        /// Id of the item
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Type of labor performed
        /// </summary>
        public LaborType Type { get; set; }

        /// <summary>
        /// An extended description for the job
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Link to an assignment
        /// </summary>
        [Indexed]
        public int AssignmentId { get; set; }

        /// <summary>
        /// The duration of this labor expressed in hours
        /// </summary>
        [Ignore]
        public TimeSpan Hours
        {
            get { return TimeSpan.FromTicks (Ticks); }
            set { Ticks = value.Ticks; }
        }

        /// <summary>
        /// Labor hours for the assignment (in ticks)
        /// </summary>
        public long Ticks { get; set; }

        /// <summary>
        /// A nicely string formatted version of Type
        /// </summary>
        [Ignore]
        public string TypeAsString
        {
            get
            {
                return Type.ToUserString ();
            }
        }

        #region UI properties

        public string HoursFormatted
        {
            get
            {
                return Hours.TotalHours.ToString ("0.0") + " hrs";
            }
        }

        #endregion
    }
}