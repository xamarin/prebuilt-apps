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
    public class HistoryViewModelTests {

        HistoryViewModel viewModel;

        [SetUp]
        public void SetUp ()
        {
            ServiceContainer.Register<ISynchronizeInvoke> (() => new Mocks.MockSynchronizeInvoke ());
            ServiceContainer.Register<IAssignmentService> (() => new Mocks.MockAssignmentService ());

            viewModel = new HistoryViewModel ();
        }

        [Test]
        public void LoadHistory ()
        {
            var task = viewModel.LoadHistoryAsync (new Assignment());

            task.Wait ();

            Assert.That (viewModel.History.Count, Is.GreaterThan (0));
        }
    }
}
