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
using System.Threading;
using System.Threading.Tasks;
using FieldService.Utilities;
using SQLite;

namespace FieldService.Data
{
    /// <summary>
    /// A helper class for working with SQLite
    /// </summary>
    public static class Database
    {
#if NETFX_CORE
        private static readonly string Path = "Database.db"; //TODO: change this later
#elif NCRUNCH
        private static readonly string Path = System.IO.Path.GetTempFileName();
#else
        private static readonly string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Database.db");
#endif
        private static bool initialized = false;
        private static readonly Type [] tableTypes = new Type []
        {
            typeof(Assignment),
            typeof(Item),
            typeof(AssignmentItem),
            typeof(Labor),
            typeof(Expense),
            typeof(ExpensePhoto),
            typeof(TimerEntry),
            typeof(Photo),
            typeof(AssignmentHistory),
            typeof(Signature),
        };

        /// <summary>
        /// For use within the app on startup, this will create the database
        /// </summary>
        /// <returns></returns>
        public static Task Initialize (CancellationToken cancellationToken)
        {
            return CreateDatabase(new SQLiteAsyncConnection(Path, true), cancellationToken);
        }

        /// <summary>
        /// Global way to grab a connection to the database, make sure to wrap in a using
        /// </summary>
        public static SQLiteAsyncConnection GetConnection (CancellationToken cancellationToken)
        {
            var connection = new SQLiteAsyncConnection(Path, true);
            if (!initialized)
            {
                CreateDatabase(connection, cancellationToken).Wait();
            }
            return connection;
        }

        private static Task CreateDatabase (SQLiteAsyncConnection connection, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                //Create the tables
                var createTask = connection.CreateTablesAsync (tableTypes);
                createTask.Wait();

                //Count number of assignments
                var countTask = connection.Table<Assignment>().CountAsync();
                countTask.Wait();

                //If no assignments exist, insert our initial data
                if (countTask.Result == 0)
                {
                    var insertTask = connection.InsertAllAsync(TestData.All);

                    //Wait for inserts
                    insertTask.Wait();

                    //Mark database created
                    initialized = true;
                }
            });
        }
    }
}