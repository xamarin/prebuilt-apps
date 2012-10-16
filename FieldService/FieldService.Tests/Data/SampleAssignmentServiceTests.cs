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
using NUnit.Framework;
using FieldService.Data;
using System.Threading.Tasks;

namespace FieldService.Tests.Data {
    [TestFixture]
    public class SampleAssignmentServiceTests {
        IAssignmentService service;

        [SetUp]
        public void SetUp ()
        {
            service = new SampleAssignmentService ();
        }

        [Test]
        public void GetAssignments ()
        {
            var task = service.GetAssignmentsAsync ();

            Task.WaitAll (task);

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));
        }

        [Test]
        public void SaveAssignment ()
        {
            var task = service.GetAssignmentsAsync ();
            Task.WaitAll (task);
            var assignment = task.Result.FirstOrDefault ();
            assignment.Title = "New Title";
            var saveTask = service.SaveAssignment (assignment);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveLaborInsert ()
        {
            var labor = new Labor ();
            labor.Description = "New Description";
            var saveTask = service.SaveLabor (labor);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveLaborUpdate ()
        {
            var labor = new Labor ();
            labor.Description = "New Description";
            var saveTask = service.SaveLabor (labor);
            saveTask.Wait ();
            labor.Description = "New Description 2";
            saveTask = service.SaveLabor (labor);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveExpenseInsert ()
        {
            var expense = new Expense ();
            expense.Description = "New Description";
            var saveTask = service.SaveExpense (expense);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveExpenseUpdate ()
        {
            var expense = new Expense ();
            expense.Description = "New Description";
            var saveTask = service.SaveExpense (expense);
            saveTask.Wait ();
            expense.Description = "New Description 2";
            saveTask = service.SaveExpense (expense);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void GetItems ()
        {
            var task = service.GetItemsAsync ();

            Task.WaitAll (task);

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));
        }

        [Test]
        public void GetAssignmentItems ()
        {
            var assignmentTask = service.GetAssignmentsAsync ();

            Task.WaitAll (assignmentTask);

            var task = service.GetItemsForAssignmentAsync (assignmentTask.Result.First ());

            Task.WaitAll (task);

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));

            foreach (var item in task.Result) {
                Assert.That (item.Name, Is.Not.Null);
                Assert.That (item.Number, Is.Not.Null);
            }
        }
    }
}