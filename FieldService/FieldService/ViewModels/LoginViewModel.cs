using FieldService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FieldService.ViewModels {
    public class LoginViewModel : ViewModelBase {
        readonly IService service;
        string username;
        string password;

        public LoginViewModel (IService service)
        {
            this.service = service;

            Username = string.Empty;
            Password = string.Empty;
        }

        public string Username
        {
            get { return username; }
            set { username = value; OnUsernameChanged (); }
        }

        public string Password
        {
            get { return password; }
            set { password = value; OnPasswordChanged (); }
        }

        public Task<bool> LoginAsync ()
        {
            return service.LoginAsync (username, password);
        }

        void OnUsernameChanged ()
        {
            string propertyName = "Username";
            if (string.IsNullOrEmpty (username)) {
                Errors [propertyName] = "Please enter a username.";
            } else {
                Errors.Remove (propertyName);
            }
            OnPropertyChanged (propertyName);
        }

        void OnPasswordChanged ()
        {
            string propertyName = "Password";
            if (string.IsNullOrEmpty (password)) {
                Errors [propertyName] = "Please enter a password.";
            } else {
                Errors.Remove (propertyName);
            }
            OnPropertyChanged (propertyName);
        }
    }
}
