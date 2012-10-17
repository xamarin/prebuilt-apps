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
using FieldService.Utilities;
using FieldService.ViewModels;
using NUnit.Framework;

namespace FieldService.Tests.ViewModels {
    /// <summary>
    /// Tests for AssignmentViewModel
    /// </summary>
    [TestFixture]
    public class AssignmentViewModelTests {
        AssignmentViewModel viewModel;

        [SetUp]
        public void SetUp ()
        {
            ServiceContainer.Register<IAssignmentService> (() => new Mocks.MockAssignmentService ());

            viewModel = new AssignmentViewModel ();
        }

        [Test]
        public void Load ()
        {
            var task = viewModel.LoadAssignmentsAsync ();

            task.Wait ();

            Assert.That (viewModel.Assignments, Is.Not.Null);
            Assert.That (viewModel.Assignments.Count, Is.Not.EqualTo (0));
        }

        [Test]
        public void SaveAssignment ()
        {
            var loadTask = viewModel.LoadAssignmentsAsync ();
            loadTask.Wait ();

            var assignment = viewModel.Assignments.First ();
            assignment.Status = AssignmentStatus.Hold;

            var task = viewModel.SaveAssignment (assignment);
            task.Wait ();

            Assert.That (viewModel.IsBusy, Is.False);
        }

        [Test]
        public void SaveAssignmentAsActive ()
        {
            var loadTask = viewModel.LoadAssignmentsAsync ();
            loadTask.Wait ();

            var assignment = viewModel.Assignments.First ();
            assignment.Status = AssignmentStatus.Active;

            var task = viewModel.SaveAssignment (assignment);
            task.Wait ();

            Assert.That (viewModel.IsBusy, Is.False);
            Assert.That (viewModel.ActiveAssignment.Status, Is.EqualTo (AssignmentStatus.Hold));
        }

        [Test]
        public void Record ()
        {
            var task = viewModel.Record ();

            task.Wait ();

            System.Threading.Thread.Sleep (1500);

            Assert.That (viewModel.Recording, Is.True);
            Assert.That ((int)viewModel.Hours.TotalSeconds, Is.EqualTo (1));
        }

        [Test]
        public void Pause ()
        {
            var task = viewModel.Record ();

            task.Wait ();

            task = viewModel.Pause ();

            task.Wait ();

            Assert.That (viewModel.Recording, Is.False);
        }

        [Test]
        public void LoadTimerEntry ()
        {
            var task = viewModel.LoadTimerEntryAsync ();

            task.Wait ();

            Assert.That ((int)viewModel.Hours.TotalHours, Is.EqualTo (1));
        }
    }
}
