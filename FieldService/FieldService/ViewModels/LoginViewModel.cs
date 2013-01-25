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
        TimeSpan autoLogoutTime = TimeSpan.FromMinutes(2); //This value can be changed.
        DateTime dateInactive = DateTime.Now;

        /// <summary>
        /// Constructor, requires an IService
        /// </summary>
        /// <param name="service">IService implementation</param>
        public LoginViewModel ()
        {
            service = ServiceContainer.Resolve<ILoginService> ();
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
                .ContinueOnCurrentThread (t => {
                    IsBusy = false; 
                    return t.Result;
                });
        }

        /// <summary>
        /// True if the login screen should be presented
        /// </summary>
        public bool IsInactive
        {
            get { return DateTime.Now - dateInactive > autoLogoutTime; }
        }

        /// <summary>
        /// Should be called to reset the last inactive time, so on DidEnterBackground on iOS, for example
        /// </summary>
        public void ResetInactiveTime ()
        {
            dateInactive = DateTime.Now;
        }

        /// <summary>
        /// Validation logic
        /// </summary>
        protected override void Validate ()
        {
            ValidateProperty (() => string.IsNullOrEmpty (username), Catalog.GetString ("UsernameValidation", comment: "Error message for username"));
            ValidateProperty (() => string.IsNullOrEmpty (password), Catalog.GetString ("PasswordValidation", comment: "Error message for password"));

            base.Validate ();
        }
    }
}