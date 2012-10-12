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

using FieldService.Data;
using FieldService.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Utilities;

namespace FieldService.Tests.ViewModels {
    /// <summary>
    /// Test for LoginViewModel
    /// </summary>
    [TestFixture]
    class LoginViewModelTests {
        LoginViewModel viewModel;

        [SetUp]
        public void SetUp ()
        {
            ServiceContainer.Register<ILoginService> (() => new SampleLoginService ());

            viewModel = new LoginViewModel ();
        }

        [Test]
        public void NoUsername ()
        {
            Assert.That (viewModel.IsValid, Is.False);
            Assert.That (viewModel.Error, Is.EqualTo ("Please enter a username." + Environment.NewLine + "Please enter a password."));
        }

        [Test]
        public void NoPassword ()
        {
            viewModel.Username = "chucknorris";

            Assert.That (viewModel.IsValid, Is.False);
            Assert.That (viewModel.Error, Is.EqualTo ("Please enter a password."));
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