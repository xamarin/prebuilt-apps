using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FieldService.Data;
using FieldService.WinRT.Utilities;
using Windows.UI.Popups;

namespace FieldService.WinRT.ViewModels {
    /// <summary>
    /// WinRT version of the LoginViewModel
    /// - We setup ICommand here
    /// </summary>
    public class LoginViewModel : FieldService.ViewModels.LoginViewModel {

        readonly RelayCommand loginCommand;

        public LoginViewModel (IService service)
            : base (service)
        {
            loginCommand = new RelayCommand (async () => {

                bool success = await LoginAsync ();
                if (success)
                    await new MessageDialog ("Success!").ShowAsync ();
                else
                    await new MessageDialog (Error).ShowAsync ();

            }, () => !IsBusy && IsValid);
        }

        /// <summary>
        /// LoginCommand for binding to a button
        /// </summary>
        public RelayCommand LoginCommand
        {
            get { return loginCommand; }
        }

        protected override void Validate ()
        {
            base.Validate ();

            if (loginCommand != null)
                loginCommand.InvalidateCanExecute ();
        }

        protected override void OnIsBusyChanged ()
        {
            base.OnIsBusyChanged ();

            if (loginCommand != null)
                loginCommand.InvalidateCanExecute ();
        }
    }
}
