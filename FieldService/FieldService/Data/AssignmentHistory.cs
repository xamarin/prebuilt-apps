using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using FieldService.Utilities;

namespace FieldService.Data {
    /// <summary>
    /// A history record for an assignment
    /// </summary>
    public class AssignmentHistory {
        /// <summary>
        /// Assignment Id
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Type of history record
        /// </summary>
        public AssignmentHistoryType Type { get; set; }

        /// <summary>
        /// Link to an assignment or zero if it were a phone call
        /// </summary>
        [Indexed]
        public int AssignmentId { get; set; }

        /// <summary>
        /// Length of the call
        /// </summary>
        [Ignore]
        public TimeSpan CallLength
        {
            get { return TimeSpan.FromTicks (CallLengthTicks); }
            set { CallLengthTicks = value.Ticks; }
        }

        /// <summary>
        /// Length of the call in ticks
        /// </summary>
        public long CallLengthTicks { get; private set; }

        /// <summary>
        /// Description of the call
        /// </summary>
        public string CallDescription { get; set; }

        /// <summary>
        /// Date of the record
        /// </summary>
        public DateTime Date { get; set; }

        #region Joined Fields

        /// <summary>
        /// A job number
        /// </summary>
        public string JobNumber { get; private set; }

        /// <summary>
        /// Company Name for the job
        /// </summary>
        public string CompanyName { get; set; }

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
        /// Description for the assignment
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

        #endregion

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
        public string DateFormatted
        {
            get
            {
                return Date.ToShortDateString ();
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
        /// A formatted version of the start date for WinRT
        /// </summary>
        public string CallLengthFormatted
        {
            get
            {
                return string.Format ("Length: {0}:{1}", CallLength.Hours.ToString ("#0"), CallLength.Minutes.ToString ("00"));
            }
        }

        /// <summary>
        /// Ui property for showing different icons
        /// </summary>
        public bool IsCall
        {
            get { return Type == AssignmentHistoryType.PhoneCall; }
        }
        #endregion
    }
}
