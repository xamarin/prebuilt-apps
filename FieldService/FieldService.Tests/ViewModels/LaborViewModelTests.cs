using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using NUnit.Framework;

namespace FieldService.Tests.ViewModels {
    [TestFixture]
    public class LaborViewModelTests {

        LaborViewModel viewModel;

        [SetUp]
        public void SetUp ()
        {
            ServiceContainer.Register<ISynchronizeInvoke> (() => new Mocks.MockSynchronizeInvoke ());
            ServiceContainer.Register<IAssignmentService> (() => new Mocks.MockAssignmentService ());

            viewModel = new LaborViewModel ();
        }

        [Test]
        public void LoadLaborHours ()
        {
            var task = viewModel.LoadLaborHours (new Assignment ());

            task.Wait ();

            Assert.That (viewModel.LaborHours.Count, Is.GreaterThan (0));
        }

        [Test]
        public void SaveLabor ()
        {
            var assignment = new Assignment ();
            var loadTask = viewModel.LoadLaborHours (assignment);

            loadTask.Wait ();

            var task = viewModel.SaveLabor (assignment, new Labor { Hours = TimeSpan.FromHours (1) });

            task.Wait ();

            Assert.That (assignment.TotalHours, Is.EqualTo (TimeSpan.FromHours(2)));
        }

        [Test]
        public void DeleteAssignmentItem ()
        {
            var assignment = new Assignment();
            var loadTask = viewModel.LoadLaborHours (assignment);

            loadTask.Wait ();
            
            var task = viewModel.DeleteLabor (assignment, viewModel.LaborHours.First());

            task.Wait ();

            Assert.That (assignment.TotalHours, Is.EqualTo (TimeSpan.FromHours(0)));
        }
    }
}
