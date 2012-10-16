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
using System.Threading.Tasks;
using FieldService.Data;

namespace FieldService.Tests.Mocks {
    /// <summary>
    /// This is just a mock service to bypass the SQLite database for speed during test runs
    /// </summary>
    class MockAssignmentService : IAssignmentService {
        public Task<List<Assignment>> GetAssignmentsAsync ()
        {
            return Task.Factory.StartNew (() => new List<Assignment> ()
            {
                new Assignment
                {
                    ID = 1,
                    Title = "test assignment",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddHours(2),
                },
            });
        }

        public Task<List<Item>> GetItemsAsync ()
        {
            return Task.Factory.StartNew (() => new List<Item> ()
            {
                new Item
                {
                    ID = 1,
                    Name = "test item",
                    Number = "1234",
                },
            });
        }

        public Task<List<AssignmentItem>> GetItemsForAssignmentAsync (Assignment assignment)
        {
            return Task.Factory.StartNew (() => new List<AssignmentItem> ()
            {
                new AssignmentItem
                {
                    ID = 1,
                    Assignment = 1,
                    Item = 1,
                },
            });
        }

        public Task<List<Labor>> GetLaborForAssignmentAsync (Assignment assignment)
        {
            return Task.Factory.StartNew (() => new List<Labor> ()
            {
                new Labor
                {
                    ID = 1,
                    Assignment = 1,
                    Description = "New Labor",
                    Type = LaborType.Hourly,
                },
            });
        }

        public Task<List<Expense>> GetExpensesForAssignmentAsync (Assignment assignment)
        {
            return Task.Factory.StartNew (() => new List<Expense> ()
            {
                new Expense
                {
                    ID = 1,
                    Assignment = 1,
                    Description = "New Expense",
                    Category = ExpenseCategory.Other,
                },
            });
        }


        public Task<int> SaveAssignment (Assignment assignment)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> SaveAssignmentItem (AssignmentItem assignmentItem)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> SaveLabor (Labor labor)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> SaveExpense (Expense expense)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> DeleteAssignment (Assignment assignment)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> DeleteAssignmentItem (AssignmentItem assignmentItem)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> DeleteLabor (Labor labor)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> DeleteExpense (Expense expense)
        {
            return Task.Factory.StartNew (() => 1);
        }
    }
}