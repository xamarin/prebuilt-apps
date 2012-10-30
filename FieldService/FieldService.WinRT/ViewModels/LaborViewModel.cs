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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using Windows.UI.Xaml.Controls.Primitives;

namespace FieldService.WinRT.ViewModels
{
    /// <summary>
    /// WinRT version of the LaborViewModel
    /// - We setup ICommand here
    /// </summary>
    public class LaborViewModel : FieldService.ViewModels.LaborViewModel
    {
        readonly DelegateCommand cancelAddLaborCommand;
        readonly DelegateCommand saveAddLaborCommand;
        readonly DelegateCommand deleteAddLaborCommand;
        Popup addLaborPopUp;

        public LaborViewModel()
        {
            cancelAddLaborCommand = new DelegateCommand(_ =>
            {
                addLaborPopUp.IsOpen = false;
            });
            saveAddLaborCommand = new DelegateCommand(_ =>
            {
                addLaborPopUp.IsOpen = false;
            });
            deleteAddLaborCommand = new DelegateCommand(_ =>
            {
                addLaborPopUp.IsOpen = false;
            });
        }

        public IEnumerable<Labor> TopLaborHours
        {
            get
            {
                if (LaborHours == null)
                    return null;

                return LaborHours.Take(5);
            }
        }

        public DelegateCommand SaveAddLaborCommand
        {
            get { return saveAddLaborCommand; }
        }

        public DelegateCommand DeleteAddLaborCommand
        {
            get { return deleteAddLaborCommand; }
        }

        public DelegateCommand CancelAddLaborCommand
        {
            get { return cancelAddLaborCommand; }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            //Make sure property changed is raised for new properties
            if (propertyName == "LaborHours")
            {
                OnPropertyChanged("TopLaborHours");
            }
        }
    }
}
