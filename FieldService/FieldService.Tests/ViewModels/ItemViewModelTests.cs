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
    public class ItemViewModelTests {

        ItemViewModel viewModel;

        [SetUp]
        public void SetUp ()
        {
            ServiceContainer.Register<ISynchronizeInvoke> (() => new Mocks.MockSynchronizeInvoke ());
            ServiceContainer.Register<IAssignmentService> (() => new Mocks.MockAssignmentService ());

            viewModel = new ItemViewModel ();
        }

        [Test]
        public void LoadAssignmentItems ()
        {
            var task = viewModel.LoadAssignmentItems (new Assignment ());

            task.Wait ();

            Assert.That (viewModel.AssignmentItems.Count, Is.GreaterThan (0));
        }

        [Test]
        public void LoadItems ()
        {
            var task = viewModel.LoadItems ();

            task.Wait ();

            Assert.That (viewModel.Items.Count, Is.GreaterThan (0));
        }

        [Test]
        public void SaveAssignmentItem ()
        {
            var assignment = new Assignment();
            var task = viewModel.SaveAssignmentItem (assignment, new AssignmentItem ());

            task.Wait ();

            Assert.That (assignment.TotalItems, Is.EqualTo (1));
        }

        [Test]
        public void DeleteAssignmentItem ()
        {
            var assignment = new Assignment { TotalItems = 1 };
            var task = viewModel.DeleteAssignmentItem (assignment, new AssignmentItem ());

            task.Wait ();

            Assert.That (assignment.TotalItems, Is.EqualTo (0));
        }
    }
}
