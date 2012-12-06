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
using System.ComponentModel;
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
            ServiceContainer.Register<ISynchronizeInvoke> (() => new Mocks.MockSynchronizeInvoke ());
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

            var task = viewModel.SaveAssignmentAsync (assignment);
            task.Wait ();

            Assert.That (viewModel.IsBusy, Is.False);
        }

        [Test]
        public void SaveAssignmentAsActive ()
        {
            var loadTask = viewModel.LoadAssignmentsAsync ();
            loadTask.Wait ();

            var activeAssignment = viewModel.ActiveAssignment;
            var assignment = viewModel.Assignments.First ();
            assignment.Status = AssignmentStatus.Active;

            var task = viewModel.SaveAssignmentAsync (assignment);
            task.Wait ();

            Assert.That (viewModel.IsBusy, Is.False);
            Assert.That (activeAssignment.Status, Is.EqualTo (AssignmentStatus.Hold));
            Assert.That (activeAssignment.Id, Is.Not.EqualTo (assignment.Id));
            Assert.That (viewModel.ActiveAssignment.Status, Is.EqualTo (AssignmentStatus.Active));
        }

        [Test]
        public void Record ()
        {
            var loadTask = viewModel.LoadAssignmentsAsync ();

            loadTask.Wait ();

            var task = viewModel.RecordAsync ();

            task.Wait ();

            System.Threading.Thread.Sleep (1500);

            Assert.That (viewModel.Recording, Is.True);
            Assert.That ((int)viewModel.Hours.TotalSeconds, Is.EqualTo (1));
        }

        [Test]
        public void Pause ()
        {
            var loadTask = viewModel.LoadAssignmentsAsync ();

            loadTask.Wait ();

            var task = viewModel.RecordAsync ();

            task.Wait ();

            System.Threading.Thread.Sleep (1500);

            task = viewModel.PauseAsync ();

            task.Wait ();

            Assert.That (viewModel.Recording, Is.False);
            Assert.That (viewModel.CurrentHours, Is.EqualTo(TimeSpan.Zero));
            Assert.That (viewModel.Hours, Is.Not.EqualTo (TimeSpan.Zero));
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
