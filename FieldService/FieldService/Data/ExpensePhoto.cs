using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace FieldService.Data
{
    public class ExpensePhoto
    {
        /// <summary>
        /// Expense Photo Id
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Link to an expense
        /// </summary>
        [Indexed]
        public int ExpenseId { get; set; }

        /// <summary>
        /// Gets or sets image for the expense
        /// </summary>
        public byte[] Image { get; set; }
    }
}
