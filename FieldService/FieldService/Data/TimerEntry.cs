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
        public int ID { get; set; }

        [Ignore]
        public TimeSpan AccumulatedHours 
        { 
            get { return TimeSpan.FromTicks(AccumulatedTicks); }
            set { AccumulatedTicks = value.Ticks; }
        }

        public long AccumulatedTicks { get; set; }

        public bool Playing { get; set; }

        /// <summary>
        /// The time the entry was started
        /// </summary>
        public DateTime Date { get; set; }
    }
}
