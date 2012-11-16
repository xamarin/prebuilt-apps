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
            var task = viewModel.LoadLaborHoursAsync (new Assignment ());

            task.Wait ();

            Assert.That (viewModel.LaborHours.Count, Is.GreaterThan (0));
        }

        [Test]
        public void SaveLabor ()
        {
            var assignment = new Assignment ();
            var loadTask = viewModel.LoadLaborHoursAsync (assignment);

            loadTask.Wait ();

            var task = viewModel.SaveLaborAsync (assignment, new Labor { Hours = TimeSpan.FromHours (1) });

            task.Wait ();

            Assert.That (assignment.TotalHours, Is.EqualTo (TimeSpan.FromHours(2)));
        }

        [Test]
        public void DeleteLabor ()
        {
            var assignment = new Assignment();
            var loadTask = viewModel.LoadLaborHoursAsync (assignment);

            loadTask.Wait ();
            
            var task = viewModel.DeleteLaborAsync (assignment, viewModel.LaborHours.First());

            task.Wait ();

            Assert.That (assignment.TotalHours, Is.EqualTo (TimeSpan.FromHours(0)));
        }
    }
}
