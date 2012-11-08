using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace FieldService.Data {
    /// <summary>
    /// A history record for an assignment
    /// </summary>
    public class AssignmentHistory {
        /// <summary>
        /// Assignment ID
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Type of history record
        /// </summary>
        public AssignmentHistoryType Type { get; set; }

        /// <summary>
        /// Link to an assignment or zero if it were a phone call
        /// </summary>
        public int Assignment { get; set; }

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
        /// Title for the job
        /// </summary>
        public string Title { get; private set; }

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

        #endregion
    }
}
