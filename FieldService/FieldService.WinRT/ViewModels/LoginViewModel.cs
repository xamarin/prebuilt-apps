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

        public ICommand LoginCommand
        {
            get { return loginCommand; }
        }

        protected override void Validate ()
        {
            base.Validate ();

            if (loginCommand != null) {
                loginCommand.InvalidateCanExecute ();
            }
        }
    }
}
