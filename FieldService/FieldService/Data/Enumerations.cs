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

namespace FieldService.Data {
    /// <summary>
    /// An enumeration for Assignment status
    /// </summary>
    public enum AssignmentStatus {
        /// <summary>
        /// The assignment is new, it has not been accepted yet
        /// </summary>
        New = 0,
        /// <summary>
        /// The assignment has been accepted
        /// </summary>
        Accepted = 1,
        /// <summary>
        /// The user is enroute to the assignment
        /// </summary>
        Enroute = 2,
        /// <summary>
        /// The user is currently working on the assignment
        /// </summary>
        InProgress = 3,
        /// <summary>
        /// The user completed the assignment
        /// </summary>
        Complete = 4,
        /// <summary>
        /// The assignment was declined by the user
        /// </summary>
        Declined = 9999,
    }

    /// <summary>
    /// An enumeration for Assignment status
    /// </summary>
    public enum LaborType {
        /// <summary>
        /// The labor counts as standard hourly labor
        /// </summary>
        Hourly = 0,
        /// <summary>
        /// The labor counts as over time
        /// </summary>
        OverTime = 1,
        /// <summary>
        /// The labor counts as holiday time
        /// </summary>
        HolidayTime = 2,
    }
}