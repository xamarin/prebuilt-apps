using FieldService.Data;
using FieldService.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Tests.ViewModels {
    [TestFixture]
    class LoginViewModelTests {
        LoginViewModel viewModel;

        [SetUp]
        public void SetUp ()
        {
            viewModel = new LoginViewModel (new DefaultService ()); ;
        }

        [Test]
        public void NoUsername ()
        {
            Assert.That (viewModel.IsValid, Is.False);
            Assert.That (viewModel ["Username"], Is.EqualTo ("Please enter a username."));
            Assert.That (viewModel ["Password"], Is.EqualTo ("Please enter a password."));
        }

        [Test]
        public void NoPassword ()
        {
            viewModel.Username = "chucknorris";

            Assert.That (viewModel.IsValid, Is.False);
            Assert.That (viewModel ["Username"], Is.EqualTo (string.Empty));
            Assert.That (viewModel ["Password"], Is.EqualTo ("Please enter a password."));
        }

        [Test]
        public void Valid ()
        {
            viewModel.Username = "chucknorris";
            viewModel.Password = "#define CHUCK";

            Assert.That (viewModel.IsValid, Is.True);
        }

        [Test]
        public void Login ()
        {
            viewModel.Username = "chucknorris";
            viewModel.Password = "#define CHUCK";

            var task = viewModel.LoginAsync ();

            Task.WaitAll (task);

            Assert.That (task.Result);
        }
    }
}
