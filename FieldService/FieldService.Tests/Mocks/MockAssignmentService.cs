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
using System.Threading;
using System.Threading.Tasks;
using FieldService.Data;

namespace FieldService.Tests.Mocks {
    /// <summary>
    /// This is just a mock service to bypass the SQLite database for speed during test runs
    /// </summary>
    class MockAssignmentService : IAssignmentService {
        public Task<List<Assignment>> GetAssignmentsAsync (CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new List<Assignment> ()
            {
                new Assignment
                {
                    Id = 1,
                    Status = AssignmentStatus.Active,
                    CompanyName = "active test assignment",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddHours(2),
                },
                new Assignment
                {
                    Id = 2,
                    Status = AssignmentStatus.New,
                    CompanyName = "test assignment",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddHours(2),
                },
            });
        }

        public Task<List<Item>> GetItemsAsync (CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new List<Item> ()
            {
                new Item
                {
                    Id = 1,
                    Name = "test item",
                    Number = "1234",
                },
            });
        }

        public Task<List<AssignmentItem>> GetItemsForAssignmentAsync (Assignment assignment, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new List<AssignmentItem> ()
            {
                new AssignmentItem
                {
                    Id = 1,
                    AssignmentId = 1,
                    ItemId = 1,
                },
            });
        }

        public Task<List<Labor>> GetLaborForAssignmentAsync (Assignment assignment, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new List<Labor> ()
            {
                new Labor
                {
                    Id = 1,
                    AssignmentId = 1,
                    Description = "New Labor",
                    Hours = TimeSpan.FromHours(1),
                    Type = LaborType.Hourly,
                },
            });
        }

        public Task<List<Expense>> GetExpensesForAssignmentAsync (Assignment assignment, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new List<Expense> ()
            {
                new Expense
                {
                    Id = 1,
                    AssignmentId = 1,
                    Description = "New Expense",
                    Category = ExpenseCategory.Other,
                },
            });
        }

        public Task<int> SaveAssignmentAsync (Assignment assignment, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> SaveAssignmentItemAsync (AssignmentItem assignmentItem, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> SaveLaborAsync (Labor labor, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> SaveExpenseAsync (Expense expense, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> DeleteAssignmentAsync (Assignment assignment, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> DeleteAssignmentItemAsync (AssignmentItem assignmentItem, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> DeleteLaborAsync (Labor labor, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> DeleteExpenseAsync (Expense expense, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> SaveTimerEntryAsync (TimerEntry entry, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> DeleteTimerEntryAsync (TimerEntry entry, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<TimerEntry> GetTimerEntryAsync (CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new TimerEntry { Date = DateTime.Now.AddHours (-1) });
        }

        public Task<List<Photo>> GetPhotosForAssignmentAsync (Assignment assignment, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new List<Photo> { new Photo { AssignmentId = assignment.Id, Image = new byte [0] } });
        }

        public Task<int> SavePhotoAsync (Photo photo, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<int> DeletePhotoAsync (Photo photo, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<List<Document>> GetDocumentsAsync (CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new List<Document> { new Document { Title = "Test Document", Path = "Test.pdf" } });
        }

        public Task<List<AssignmentHistory>> GetAssignmentHistoryAsync (Assignment assignment, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new List<AssignmentHistory> { new AssignmentHistory () });
        }

        public Task<int> SaveSignatureAsync (Signature signature, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<Signature> GetSignatureAsync (Assignment assignment, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new Signature { AssignmentId = assignment.Id });
        }

        public Task<int> SaveExpensePhotoAsync (ExpensePhoto photo, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => 1);
        }

        public Task<ExpensePhoto> GetExpensePhotoAsync (Expense expense, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new ExpensePhoto { ExpenseId = expense.Id });
        }

        public Task<List<Assignment>> GetAssignmentFromHistory (AssignmentHistory assignmentHistory, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew (() => new List<Assignment> { new Assignment { Id = assignmentHistory.AssignmentId } });
        }
    }
}