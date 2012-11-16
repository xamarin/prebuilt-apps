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
    public class DocumentViewModelTests {

        DocumentViewModel viewModel;

        [SetUp]
        public void SetUp ()
        {
            ServiceContainer.Register<ISynchronizeInvoke> (() => new Mocks.MockSynchronizeInvoke ());
            ServiceContainer.Register<IAssignmentService> (() => new Mocks.MockAssignmentService ());

            viewModel = new DocumentViewModel ();
        }

        [Test]
        public void LoadDocuments ()
        {
            var task = viewModel.LoadDocumentsAsync ();

            task.Wait ();

            Assert.That (viewModel.Documents.Count, Is.GreaterThan (0));
        }
    }
}
