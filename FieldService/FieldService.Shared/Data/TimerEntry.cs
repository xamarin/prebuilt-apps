using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace FieldService.Data {
    /// <summary>
    /// Class for holding a TimeStamp to persist the timer while the app is closed
    /// </summary>
    public class TimerEntry {
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// The time the entry was started
        /// </summary>
        public DateTime Date { get; set; }
    }
}
