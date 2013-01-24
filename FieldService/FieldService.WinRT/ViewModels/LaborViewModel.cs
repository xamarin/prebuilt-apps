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
using System.Globalization;
using System.Linq;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FieldService.WinRT.ViewModels {
    /// <summary>
    /// WinRT version of the LaborViewModel
    /// - We setup ICommand here
    /// </summary>
    public class LaborViewModel : FieldService.ViewModels.LaborViewModel {
        readonly AssignmentViewModel assignmentViewModel;
        readonly DelegateCommand cancelAddLaborCommand, saveAddLaborCommand, deleteAddLaborCommand, addLaborCommand;
        Popup addLaborPopUp;
        Labor selectedLabor;
        bool canDelete = false;
        string currentLaborHours = string.Empty, 
            addLaborHeader = "Add Labor";
        LaborType [] laborTypes = new LaborType [] { LaborType.Hourly, LaborType.OverTime, LaborType.HolidayTime };

        public LaborViewModel ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
            cancelAddLaborCommand = new DelegateCommand (_ => {
                addLaborPopUp.IsOpen = false;
            });
            saveAddLaborCommand = new DelegateCommand (async _ => {
                selectedLabor.Hours = TimeSpan.FromHours (currentLaborHours.ToDouble (CultureInfo.InvariantCulture));
                selectedLabor.AssignmentId = assignmentViewModel.SelectedAssignment.Id;
                await SaveLaborAsync (assignmentViewModel.SelectedAssignment, selectedLabor);
                await LoadLaborHoursAsync (assignmentViewModel.SelectedAssignment);
                addLaborPopUp.IsOpen = false;
            });
            deleteAddLaborCommand = new DelegateCommand (async _ => {
                await DeleteLaborAsync (assignmentViewModel.SelectedAssignment, selectedLabor);
                await LoadLaborHoursAsync (assignmentViewModel.SelectedAssignment);
                addLaborPopUp.IsOpen = false;
            });
            addLaborCommand = new DelegateCommand (obj => {
                var labor = obj as Labor;
                if (labor != null) {
                    SelectedLabor = labor;
                    AddLaborHeader = "Labor";
                    CanDelete = true;
                } else {
                    SelectedLabor = new Labor ();
                    AddLaborHeader = "Add Labor";
                    CanDelete = false;
                }
                if (addLaborPopUp != null && addLaborPopUp.IsOpen) {
                    addLaborPopUp.IsOpen = false;
                }
                addLaborPopUp = new Popup ();
                addLaborPopUp.Height = Window.Current.Bounds.Height;
                addLaborPopUp.Width = Constants.PopUpWidth;
                AddLaborFlyoutPanel flyoutpanel = new AddLaborFlyoutPanel ();
                flyoutpanel.Width = addLaborPopUp.Width;
                flyoutpanel.Height = addLaborPopUp.Height;
                addLaborPopUp.Child = flyoutpanel;
                addLaborPopUp.SetValue (Canvas.LeftProperty, Window.Current.Bounds.Width - Constants.PopUpWidth);
                addLaborPopUp.SetValue (Canvas.TopProperty, 0);
                addLaborPopUp.IsOpen = true;
            });
        }
        
        /// <summary>
        /// Flyout panel for adding a labor item
        /// </summary>
        public Popup LaborPopUp
        {
            get { return addLaborPopUp; }
        }

        /// <summary>
        /// list of top 5 labor hour items
        /// </summary>
        public IEnumerable<Labor> TopLaborHours
        {
            get
            {
                if (LaborHours == null)
                    return null;

                return LaborHours.Take (5);
            }
        }

        /// <summary>
        /// header for add labor
        /// </summary>
        public string AddLaborHeader
        {
            get { return addLaborHeader; }
            set { addLaborHeader = value; OnPropertyChanged ("AddLaborHeader"); }
        }

        /// <summary>
        /// bool value for delete visibilty
        /// </summary>
        public bool CanDelete
        {
            get { return canDelete; }
            set { canDelete = value; OnPropertyChanged ("CanDelete"); }
        }

        /// <summary>
        /// save new/edit labor item
        /// </summary>
        public DelegateCommand SaveAddLaborCommand
        {
            get { return saveAddLaborCommand; }
        }

        /// <summary>
        /// delete the current labor item
        /// </summary>
        public DelegateCommand DeleteAddLaborCommand
        {
            get { return deleteAddLaborCommand; }
        }

        /// <summary>
        /// cancel labor flyout panel
        /// </summary>
        public DelegateCommand CancelAddLaborCommand
        {
            get { return cancelAddLaborCommand; }
        }

        /// <summary>
        /// open the labor flyout panel
        /// </summary>
        public DelegateCommand AddLaborCommand
        {
            get { return addLaborCommand; }
        }

        /// <summary>
        /// array of labor types for combo box
        /// </summary>
        public LaborType [] LaborTypes
        {
            get { return laborTypes; }
        }

        /// <summary>
        /// selected labor item
        /// </summary>
        public override Labor SelectedLabor
        {
            get { return selectedLabor; }
            set
            {
                selectedLabor = value;
                OnPropertyChanged ("SelectedLabor");
                if (value != null)
                    CurrentLaborHours = value.Hours.TotalHours.ToString ("0.0");
                else
                    CurrentLaborHours = string.Empty;
            }
        }

        /// <summary>
        /// formatted labor hours from selected labor
        /// </summary>
        public string CurrentLaborHours
        {
            get { return currentLaborHours; }
            set { currentLaborHours = value; OnPropertyChanged ("CurrentLaborHours"); }
        }

        /// <summary>
        /// Value for not is history
        /// </summary>
        public bool IsNotReadonly
        {
            get { return assignmentViewModel.IsNotReadonly; }
        }

        protected override void OnPropertyChanged (string propertyName)
        {
            base.OnPropertyChanged (propertyName);

            //Make sure property changed is raised for new properties
            if (propertyName == "LaborHours") {
                OnPropertyChanged ("TopLaborHours");
            }
        }
    }
}
