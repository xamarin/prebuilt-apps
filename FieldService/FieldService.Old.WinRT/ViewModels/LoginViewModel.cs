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
using System;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.Views;
using Windows.UI.Popups;

namespace FieldService.WinRT.ViewModels {
    /// <summary>
    /// WinRT version of the LoginViewModel
    /// - We setup ICommand here
    /// </summary>
    public class LoginViewModel : FieldService.ViewModels.LoginViewModel {

        readonly DelegateCommand loginCommand, helpCommand;

        public LoginViewModel ()
        {
            loginCommand = new DelegateCommand (async _ => {

                
                bool success = await LoginAsync ();
                if (success)
                    Helpers.NavigateTo<AssignmentsPage> ();
                else
                    await new MessageDialog (Error).ShowAsync ();

            }, _ => !IsBusy && IsValid);

            helpCommand = new DelegateCommand(async _ =>
                {
                    var dialog = new MessageDialog("Enter any username or password.", "Need Help?");
                    await dialog.ShowAsync();
                });
        }

        /// <summary>
        /// LoginCommand for binding to a button
        /// </summary>
        public DelegateCommand LoginCommand
        {
            get { return loginCommand; }
        }

        /// <summary>
        /// HelpCommand for binding to a button
        /// </summary>
        public DelegateCommand HelpCommand
        {
            get { return helpCommand; }
        }

        protected override void Validate ()
        {
            base.Validate ();

            if (loginCommand != null)
                loginCommand.RaiseCanExecuteChanged ();
        }

        protected override void OnIsBusyChanged ()
        {
            base.OnIsBusyChanged ();

            if (loginCommand != null)
                loginCommand.RaiseCanExecuteChanged ();
        }
    }
}
