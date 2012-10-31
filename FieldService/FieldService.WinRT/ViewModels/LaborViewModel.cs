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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FieldService.WinRT.Views;
using FieldService.Utilities;

namespace FieldService.WinRT.ViewModels
{
    /// <summary>
    /// WinRT version of the LaborViewModel
    /// - We setup ICommand here
    /// </summary>
    public class LaborViewModel : FieldService.ViewModels.LaborViewModel
    {
        readonly AssignmentViewModel assignmentViewModel;
        readonly DelegateCommand cancelAddLaborCommand;
        readonly DelegateCommand saveAddLaborCommand;
        readonly DelegateCommand deleteAddLaborCommand;
        readonly DelegateCommand addLaborCommand;
        Popup addLaborPopUp;
        Labor selectedLabor;
        string currentLaborHours = string.Empty;
        int popUpWidth = 485;
        LaborType[] laborTypes = new LaborType[] { LaborType.Hourly, LaborType.OverTime, LaborType.HolidayTime };

        public LaborViewModel()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
            cancelAddLaborCommand = new DelegateCommand(_ =>
            {
                addLaborPopUp.IsOpen = false;
            });
            saveAddLaborCommand = new DelegateCommand(_ =>
            {
                selectedLabor.Hours = TimeSpan.FromHours(currentLaborHours.ToDouble());
                selectedLabor.Assignment = assignmentViewModel.SelectedAssignment.ID;
                SaveLabor(assignmentViewModel.SelectedAssignment, selectedLabor);
                LoadLaborHours(assignmentViewModel.SelectedAssignment);
                addLaborPopUp.IsOpen = false;
            });
            deleteAddLaborCommand = new DelegateCommand(_ =>
            {
                DeleteLabor(assignmentViewModel.SelectedAssignment, selectedLabor);
                LoadLaborHours(assignmentViewModel.SelectedAssignment);
                addLaborPopUp.IsOpen = false;
            });
            addLaborCommand = new DelegateCommand(obj =>
                {
                    var labor = obj as Labor;
                    if (labor != null)
                        SelectedLabor = labor;
                    else
                        SelectedLabor = new Labor();
                    addLaborPopUp = new Popup();
                    addLaborPopUp.Height = Window.Current.Bounds.Height;
                    addLaborPopUp.Width = popUpWidth;
                    AddLaborFlyoutPanel flyoutpanel = new AddLaborFlyoutPanel();
                    flyoutpanel.Width = addLaborPopUp.Width;
                    flyoutpanel.Height = addLaborPopUp.Height;
                    addLaborPopUp.Child = flyoutpanel;
                    addLaborPopUp.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width - popUpWidth);
                    addLaborPopUp.SetValue(Canvas.TopProperty, 0);
                    addLaborPopUp.IsOpen = true;
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

        public DelegateCommand AddLaborCommand
        {
            get { return addLaborCommand; }
        }

        public LaborType[] LaborTypes
        {
            get { return laborTypes; }
        }

        public Labor SelectedLabor
        {
            get { return selectedLabor; }
            set
            {
                selectedLabor = value;
                if (value != null)
                    CurrentLaborHours = value.Hours.TotalHours.ToString("0.0");
                else
                    CurrentLaborHours = string.Empty;
            }
        }

        public string CurrentLaborHours
        {
            get { return currentLaborHours; }
            set { currentLaborHours = value; OnPropertyChanged("CurrentLaborHours"); }
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
