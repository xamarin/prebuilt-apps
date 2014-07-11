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
using FieldService.Utilities;

namespace FieldService.Data {
    /// <summary>
    /// An assignment is the "thing" or "job" the user is going to work on
    /// </summary>
    public class Assignment {
        /// <summary>
        /// Assignment Id
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// The assignment's status
        /// </summary>
        public AssignmentStatus Status { get; set; }

        /// <summary>
        /// A job number
        /// </summary>
        public string JobNumber { get; set; }

        /// <summary>
        /// Company Name for the job
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Priority for the assignment
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// An extended description for the job
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Date and time the assignment should start
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date and time the assignment should end
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Name of the contact
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Phone number for the contact
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// Address for assignment
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// City for the assignment
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State of the assignment
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Zip code for the assignment
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Latitude of assignment
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Longitude of assignment
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// Total labor hours for the assignment
        /// </summary>
        [Ignore]
        public TimeSpan TotalHours
        {
            get { return TimeSpan.FromTicks (TotalTicks); }
            set { TotalTicks = value.Ticks; }
        }

        /// <summary>
        /// Total labor hours for the assignment (in ticks)
        /// </summary>
        public long TotalTicks { get; private set; }

        /// <summary>
        /// Total number of items
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Total cost of expenses
        /// </summary>
        public decimal TotalExpenses { get; set; }

        /// <summary>
        /// If true, this is an assignment history record that is readonly
        /// </summary>
        [Ignore]
        public bool IsHistory { get; set; }

        #region UI properties

        /// <summary>
        /// A formatted version of the job number for WinRT
        /// </summary>
        public string JobNumberFormatted
        {
            get
            {
                if (string.IsNullOrEmpty (JobNumber))
                    return JobNumber;
                return "#" + JobNumber;
            }
        }

        /// <summary>
        /// A formatted version of the start date for WinRT
        /// </summary>
        public string StartDateFormatted
        {
            get
            {
                return StartDate.ToShortDateString ();
            }
        }

        /// <summary>
        /// A formatted version of the times for WinRT
        /// </summary>
        public string TimesFormatted
        {
            get
            {
                return StartDate.ToShortTimeString () + 
                    Environment.NewLine +
                    "· · ·" +
                    Environment.NewLine + 
                    EndDate.ToShortTimeString ();
            }
        }

        /// <summary>
        /// A formatted version of the start time for WinRT
        /// </summary>
        public string StartTimeFormatted
        {
            get { return StartDate.ToShortTimeString (); }
        }

        /// <summary>
        /// A formatted version of the end time for WinRT
        /// </summary>
        public string EndTimeFormatted
        {
            get { return EndDate.ToShortTimeString (); }
        }

        /// <summary>
        /// A formatted version of the address for WinRT
        /// </summary>
        public string AddressFormatted
        {
            get
            {
                return Address +
                    Environment.NewLine +
                    City + ", " + State + " " + Zip;
            }
        }

        /// <summary>
        /// Formatted version of total hours for WinRT
        /// </summary>
        public string TotalHoursFormatted
        {
            get { return TotalHours.TotalHours.ToString ("0.0"); }
        }

        /// <summary>
        /// Formatted version of expenses for WinRT
        /// </summary>
        public string TotalExpensesFormatted
        {
            get { return TotalExpenses.ToString ("$0.00"); }
        }

	/// <summary>
	/// If true, this assignment is not editable - it is complete or a history record
	/// </summary>
	public bool IsReadonly
	{
	    get { return Status == AssignmentStatus.Complete || IsHistory; }
	}

        /// <summary>
        /// If true, it's ok to complete this assignment
        /// </summary>
        public bool CanComplete
        {
            get { return Status != AssignmentStatus.New && !IsReadonly; }
        }

        #endregion
    }
}