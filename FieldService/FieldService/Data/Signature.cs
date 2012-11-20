using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace FieldService.Data {
    public class Signature {
        /// <summary>
        /// Signature Id
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Link to an assignment
        /// </summary>
        [Indexed]
        public int AssignmentId { get; set; }

        /// <summary>
        /// Gets or sets the signature image
        /// </summary>
        public byte [] Image { get; set; }
    }
}
