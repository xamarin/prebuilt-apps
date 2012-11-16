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
    public class PhotoViewModelTests {

        PhotoViewModel viewModel;

        [SetUp]
        public void SetUp ()
        {
            ServiceContainer.Register<ISynchronizeInvoke> (() => new Mocks.MockSynchronizeInvoke ());
            ServiceContainer.Register<IAssignmentService> (() => new Mocks.MockAssignmentService ());

            viewModel = new PhotoViewModel ();
        }

        [Test]
        public void LoadPhotos ()
        {
            var task = viewModel.LoadPhotosAsync (new Assignment());

            task.Wait ();

            Assert.That (viewModel.Photos.Count, Is.GreaterThan (0));
        }

        [Test]
        public void SavePhoto ()
        {
            var assignment = new Assignment ();
            var task = viewModel.SavePhotoAsync (assignment, new Photo ());

            task.Wait ();

            Assert.That (viewModel.Photos.Count, Is.EqualTo (1));
        }

        [Test]
        public void DeletePhoto ()
        {
            var assignment = new Assignment { TotalItems = 1 };
            var photo = new Photo ();
            viewModel.Photos = new List<Photo> { photo };
            var task = viewModel.DeletePhotoAsync (assignment, photo);

            task.Wait ();

            Assert.That (viewModel.Photos.Count, Is.EqualTo (0));
        }
    }
}
