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
using FieldService.Utilities;
using NUnit.Framework;

namespace FieldService.Tests.Data {
    [TestFixture]
    public class SampleAssignmentServiceTests {
        IAssignmentService service;

        [SetUp]
        public void SetUp ()
        {
            Database.Initialize (CancellationToken.None).Wait ();

            service = new SampleAssignmentService ();
        }

        [Test]
        public void GetAssignments ()
        {
            var task = service.GetAssignmentsAsync (CancellationToken.None);

            task.Wait ();

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));
        }

        [Test]
        public void GetAssignmentAndCheckTotalHours ()
        {
            var assignmentTask = service.GetAssignmentsAsync (CancellationToken.None);

            assignmentTask.Wait ();

            var assignment = assignmentTask.Result.FirstOrDefault (a => a.CompanyName == "Xamarin");

            Assert.That (assignment.TotalHours, Is.EqualTo (TimeSpan.FromHours (14)));
        }

        [Test]
        public void GetAssignmentAndCheckTotalItems ()
        {
            var assignmentTask = service.GetAssignmentsAsync (CancellationToken.None);

            assignmentTask.Wait ();

            var assignment = assignmentTask.Result.FirstOrDefault (a => a.CompanyName == "Xamarin");

            Assert.That (assignment.TotalItems, Is.EqualTo (2));
        }

        [Test]
        public void GetAssignmentAndCheckTotalExpenses ()
        {
            var assignmentTask = service.GetAssignmentsAsync (CancellationToken.None);

            assignmentTask.Wait ();

            var assignment = assignmentTask.Result.FirstOrDefault (a => a.CompanyName == "Xamarin");

            Assert.That (assignment.TotalExpenses, Is.EqualTo (41.49M));
        }

        [Test]
        public void SaveAssignment ()
        {
            var task = service.GetAssignmentsAsync (CancellationToken.None);
            task.Wait ();

            var assignment = task.Result.FirstOrDefault ();
            assignment.CompanyName = "New Company";

            var saveTask = service.SaveAssignmentAsync (assignment, CancellationToken.None);
            saveTask.Wait ();

            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveAssignmentItemInsert ()
        {
            var assignmentItem = new AssignmentItem ();
            assignmentItem.ItemId = 1;
            assignmentItem.AssignmentId = 1;

            var saveTask = service.SaveAssignmentItemAsync (assignmentItem, CancellationToken.None);
            saveTask.Wait ();

            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveAssignmentItemUpdate ()
        {
            var assignmentItem = new AssignmentItem ();
            assignmentItem.ItemId = 1;
            assignmentItem.AssignmentId = 1;

            var saveTask = service.SaveAssignmentItemAsync (assignmentItem, CancellationToken.None);
            saveTask.Wait ();

            assignmentItem.ItemId = 1;
            assignmentItem.AssignmentId = 2;
            saveTask = service.SaveAssignmentItemAsync (assignmentItem, CancellationToken.None);
            saveTask.Wait ();

            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveLaborInsert ()
        {
            var labor = new Labor ();
            labor.Description = "New Description";

            var saveTask = service.SaveLaborAsync (labor, CancellationToken.None);
            saveTask.Wait ();

            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveLaborUpdate ()
        {
            var labor = new Labor ();
            labor.Description = "New Description";

            var saveTask = service.SaveLaborAsync (labor, CancellationToken.None);
            saveTask.Wait ();

            labor.Description = "New Description 2";
            saveTask = service.SaveLaborAsync (labor, CancellationToken.None);
            saveTask.Wait ();

            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveExpenseInsert ()
        {
            var expense = new Expense ();
            expense.Description = "New Description";

            var saveTask = service.SaveExpenseAsync (expense, CancellationToken.None);
            saveTask.Wait ();

            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveExpenseUpdate ()
        {
            var expense = new Expense ();
            expense.Description = "New Description";

            var saveTask = service.SaveExpenseAsync (expense, CancellationToken.None);
            saveTask.Wait ();

            expense.Description = "New Description 2";
            saveTask = service.SaveExpenseAsync (expense, CancellationToken.None);
            saveTask.Wait ();

            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void GetItems ()
        {
            var task = service.GetItemsAsync (CancellationToken.None);

            task.Wait ();

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));
        }

        [Test]
        public void GetAssignmentItems ()
        {
            var assignmentTask = service.GetAssignmentsAsync (CancellationToken.None);

            assignmentTask.Wait ();

            var task = service.GetItemsForAssignmentAsync (assignmentTask.Result.First (a => a.Id == 1), CancellationToken.None);

            task.Wait ();

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));

            foreach (var item in task.Result) {
                Assert.That (item.Name, Is.Not.Null);
                Assert.That (item.Number, Is.Not.Null);
            }
        }

        [Test]
        public void GetAssignmentLabor ()
        {
            var assignmentTask = service.GetAssignmentsAsync (CancellationToken.None);

            assignmentTask.Wait ();

            var task = service.GetLaborForAssignmentAsync (assignmentTask.Result.First (), CancellationToken.None);

            task.Wait ();

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));

            foreach (var labor in task.Result) {
                Assert.That (labor.Description, Is.Not.Null);
                Assert.That (labor.Type, Is.Not.Null);
            }
        }

        [Test]
        public void GetAssignmentExpenses ()
        {
            var assignmentTask = service.GetAssignmentsAsync (CancellationToken.None);

            assignmentTask.Wait ();

            var task = service.GetExpensesForAssignmentAsync (assignmentTask.Result.First (), CancellationToken.None);

            task.Wait ();

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));

            foreach (var expense in task.Result) {
                Assert.That (expense.Description, Is.Not.Null);
                Assert.That (expense.Category, Is.Not.Null);
            }
        }

        [Test]
        public void DeleteAssignment()
        {
            var task = service.GetAssignmentsAsync (CancellationToken.None);
            task.Wait ();

            var assignment = task.Result.FirstOrDefault (a => a.CompanyName != "Xamarin");
            assignment.CompanyName = "New Company";

            var saveTask = service.SaveAssignmentAsync (assignment, CancellationToken.None);
            saveTask.Wait ();

            var deleteTask = service.DeleteAssignmentAsync (assignment, CancellationToken.None);
            deleteTask.Wait ();
            Assert.That (deleteTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void DeleteAssignmentItem ()
        {
            var assignmentItem = new AssignmentItem ();
            var saveTask = service.SaveAssignmentItemAsync (assignmentItem, CancellationToken.None);
            saveTask.Wait ();

            var deleteTask = service.DeleteAssignmentItemAsync (assignmentItem, CancellationToken.None);
            deleteTask.Wait ();
            Assert.That (deleteTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void DeleteLabor ()
        {
            var labor = new Labor ();
            labor.Description = "New Description";

            var saveTask = service.SaveLaborAsync (labor, CancellationToken.None);
            saveTask.Wait ();

            var deleteTask = service.DeleteLaborAsync (labor, CancellationToken.None);
            deleteTask.Wait ();
            Assert.That (deleteTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void DeleteExpense ()
        {
            var expense = new Expense ();
            expense.Description = "New Description";

            var saveTask = service.SaveExpenseAsync (expense, CancellationToken.None);
            saveTask.Wait ();

            var deleteTask = service.DeleteExpenseAsync (expense, CancellationToken.None);
            deleteTask.Wait ();
            Assert.That (deleteTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveTimerEntry ()
        {
            TimerEntry entry;

            var getTask = service.GetTimerEntryAsync();
            getTask.Wait();
            entry = getTask.Result;

            if (entry == null) {
                entry = new TimerEntry { Date = DateTime.Now.AddHours(-1) };
            }

            try {
                var task = service.SaveTimerEntryAsync (entry, CancellationToken.None);

                task.Wait ();

                Assert.That (task.Result, Is.EqualTo (1));
            } finally {
                var deleteTask = service.DeleteTimerEntryAsync (entry, CancellationToken.None);

                deleteTask.Wait ();
            }
        }

        [Test]
        public void UpdateTimerEntry ()
        {
            var entry = new TimerEntry { Date = DateTime.Now.AddHours (-1) };

            try {
                var task = service.SaveTimerEntryAsync (entry, CancellationToken.None);

                task.Wait ();

                entry.Date = entry.Date.AddHours (2);
                task = service.SaveTimerEntryAsync (entry, CancellationToken.None);

                Assert.That (task.Result, Is.EqualTo (1));
            } finally {
                var deleteTask = service.DeleteTimerEntryAsync (entry, CancellationToken.None);

                deleteTask.Wait ();
            }
        }

        [Test]
        public void GetTimerEntry ()
        {
            var entry = new TimerEntry { Date = DateTime.Now.AddHours (-1) };
            var task = service.SaveTimerEntryAsync (entry, CancellationToken.None);

            task.Wait ();

            var getTask = service.GetTimerEntryAsync (CancellationToken.None);

            getTask.Wait ();
            
            var result = getTask.Result;
            Assert.That (entry.Id, Is.EqualTo (result.Id));
            Assert.That (entry.Date, Is.EqualTo (result.Date));
        }

        [Test]
        public void DeleteTimerEntry ()
        {
            var entry = new TimerEntry { Date = DateTime.Now.AddHours (-1) };
            var task = service.SaveTimerEntryAsync (entry, CancellationToken.None);

            task.Wait ();

            var deleteTask = service.DeleteTimerEntryAsync (entry, CancellationToken.None);

            deleteTask.Wait ();

            Assert.That (deleteTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void GetPhotos ()
        {
            var task = service.GetPhotosForAssignmentAsync (new Assignment (), CancellationToken.None);

            task.Wait ();

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.EqualTo (0));
        }

        [Test]
        public void SavePhoto ()
        {
            var assignmentTask = service.GetAssignmentsAsync (CancellationToken.None);

            assignmentTask.Wait ();
             
            var assignment = assignmentTask.Result.First ();

            var task = service.SavePhotoAsync (new Photo { AssignmentId = assignment.Id, Image = new byte [] { 255 } }, CancellationToken.None);

            task.Wait ();

            Assert.That (task.Result, Is.EqualTo (1));
        }

        [Test]
        public void DeletePhoto ()
        {
            var assignmentTask = service.GetAssignmentsAsync (CancellationToken.None);

            assignmentTask.Wait ();

            var assignment = assignmentTask.Result.First ();
            var photo = new Photo { AssignmentId = assignment.Id, Image = new byte [] { 255 } };

            var task = service.SavePhotoAsync (photo, CancellationToken.None);

            task.Wait ();

            var deleteTask = service.DeletePhotoAsync (photo, CancellationToken.None);

            deleteTask.Wait ();

            Assert.That (deleteTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void GetHistory ()
        {
            var assignmentTask = service.GetAssignmentsAsync (CancellationToken.None);

            assignmentTask.Wait ();
            var assignment = assignmentTask.Result.First ();

            var historyTask = service.GetAssignmentHistoryAsync (assignment, CancellationToken.None);
            historyTask.Wait ();

            Assert.That (historyTask.Result.Count, Is.Not.EqualTo (0));

            foreach (var history in historyTask.Result) {
                if (history.Type == AssignmentHistoryType.Assignment) {
                    Assert.That (history.CompanyName, Is.Not.Null);
                }
            }
        }

        [Test]
        public void GetSignature ()
        {
            var assignmentTask = service.GetAssignmentsAsync (CancellationToken.None);
            
            assignmentTask.Wait ();
            var assignment = assignmentTask.Result.First ();

            var signature = new Signature {
                AssignmentId = assignment.Id,
                Image = new byte [1],
            };

            var saveTask = service.SaveSignatureAsync (signature, CancellationToken.None);
            saveTask.Wait ();

            var signatureTask = service.GetSignatureAsync(assignment, CancellationToken.None);
            signatureTask.Wait ();

            Assert.That (signatureTask.Result, Is.Not.Null);
        }

        [Test]
        public void GetExpensePhoto ()
        {
            var assignmentTask = service.GetAssignmentsAsync (CancellationToken.None);

            assignmentTask.Wait ();
            var assignment = assignmentTask.Result.First (a => a.CompanyName == "Xamarin");

            var expenseTask = service.GetExpensesForAssignmentAsync (assignment, CancellationToken.None);
            expenseTask.Wait ();

            var expense = expenseTask.Result.First ();

            var photo = new ExpensePhoto {
                ExpenseId = expense.Id,
                Image = new byte [1],
            };

            service.SaveExpensePhotoAsync (photo, CancellationToken.None).Wait ();

            var expensePhotoTask = service.GetExpensePhotoAsync (expense, CancellationToken.None);
            expensePhotoTask.Wait ();

            Assert.That (expensePhotoTask.Result, Is.Not.Null);
        }
    }
}