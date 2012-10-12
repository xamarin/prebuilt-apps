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
using FieldService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace FieldService.ViewModels {
    /// <summary>
    /// ViewModel class for the Login screen
    /// </summary>
    public class LoginViewModel : ViewModelBase {
        readonly ILoginService service;
        string username;
        string password;

        /// <summary>
        /// Constructor, requires an IService
        /// </summary>
        /// <param name="service">IService implementation</param>
        public LoginViewModel (ILoginService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Username property
        /// </summary>
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                Validate ();
                OnPropertyChanged ("Username");
            }
        }

        /// <summary>
        /// Password property
        /// </summary>
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                Validate ();
                OnPropertyChanged ("Password");
            }
        }

        /// <summary>
        /// Performs an asynchronous login
        /// </summary>
        /// <returns></returns>
        public Task<bool> LoginAsync ()
        {
            IsBusy = true;
            return service
                .LoginAsync (username, password)
                .ContinueOnUIThread (t => {
                    IsBusy = false; 
                    return t.Result;
                });
        }

        /// <summary>
        /// Validation logic
        /// </summary>
        protected override void Validate ()
        {
            ValidateProperty (() => string.IsNullOrEmpty (username), Strings.UsernameValidation);
            ValidateProperty (() => string.IsNullOrEmpty (password), Strings.PasswordValidation);

            base.Validate ();
        }
    }
}