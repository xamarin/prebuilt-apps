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
        readonly IService service;
        string username;
        string password;

        /// <summary>
        /// Constructor, requires an IService
        /// </summary>
        /// <param name="service">IService implementation</param>
        public LoginViewModel (IService service)
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
            var task = service.LoginAsync (username, password);
            task.ContinueOnUIThread (t => IsBusy = false);
            return task;
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